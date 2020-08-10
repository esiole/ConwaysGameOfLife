using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Shapes;
using Microsoft.Xaml.Behaviors;

namespace ConwaysGameOfLife
{
    public partial class MainWindow : Window
    {
        private readonly ApplicationViewModel appViewModel;

        public MainWindow()
        {
            InitializeComponent();
            CreateMapButton.Click += (sender, e) =>
            {
                StartInfoPanel.Children.Clear();
                ToolBar.Visibility = Visibility.Visible;
                CreateMap(appViewModel.SelectedSize.WidthCellCount, appViewModel.SelectedSize.HeightCellCount, appViewModel.CellSize);
            };
            GridToggleButton.Click += (sender, e) => Map.ShowGridLines = !Map.ShowGridLines;
            appViewModel = new ApplicationViewModel(new DefaultDialogService(), new JsonAsyncFileService());
            DataContext = appViewModel;
        }

        private void CreateMap(int width, int height, int cellSize)
        {
            var beginState = Game.CreateMap(width, height);
            appViewModel.Game.SetMap(beginState);

            for (int j = 0; j < height; j++)
                Map.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(cellSize) });
            for (int i = 0; i < width; i++)
                Map.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(cellSize) });

            Map.Width = width * cellSize;
            Map.Height = height * cellSize;
            this.Height = Map.Height + 40 + ToolBar.Height;
            this.Width = Map.Width + 15;
            var screenHeight = SystemParameters.FullPrimaryScreenHeight;
            var screenWidth = SystemParameters.FullPrimaryScreenWidth;
            this.Top = (screenHeight - this.ActualHeight) / 2;
            this.Left = (screenWidth - this.ActualWidth) / 2;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    int x = i;
                    int y = j;
                    var shape = new Ellipse()
                    {
                        Width = cellSize,
                        Height = cellSize,
                    };
                    Map.Children.Add(shape);
                    Grid.SetRow(shape, x);
                    Grid.SetColumn(shape, y);

                    var trigger = new Microsoft.Xaml.Behaviors.EventTrigger("MouseDown");
                    var commandAction = new InvokeCommandAction()
                    {
                        Command = appViewModel.ToggleCommand,
                        CommandParameter = beginState[x, y],
                    };
                    trigger.Actions.Add(commandAction);
                    Interaction.GetTriggers(shape).Add(trigger);

                    var binding = new Binding
                    {
                        Source = beginState[x, y],
                        Path = new PropertyPath("State")
                    };
                    shape.SetBinding(Shape.FillProperty, binding);
                }
            }
        }
    }
}
