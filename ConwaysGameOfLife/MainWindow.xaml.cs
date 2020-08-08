using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ConwaysGameOfLife
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Game Game;

        public MainWindow()
        {
            InitializeComponent();
            CreateMapButton.Click += (sender, e) =>
            {
                StartInfoPanel.Children.Clear();
                ToolBar.Visibility = Visibility.Visible;
                CreateMap(15, 10, 30);
            };
            StartGameButton.Click += (sender, e) =>
            {
                StartGameButton.IsEnabled = false;
                Map.IsHitTestVisible = false;
                Game.Start();
            };
        }

        private void CreateMap(int width, int height, int cellSize)
        {
            var beginState = Game.CreateState(width, height);
            beginState[3, 3].State = Brushes.Black;
            beginState[3, 4].State = Brushes.Black;
            beginState[3, 5].State = Brushes.Black;
            beginState[3, 6].State = Brushes.Black;
            beginState[3, 7].State = Brushes.Black;
            beginState[4, 3].State = Brushes.Black;
            beginState[4, 4].State = Brushes.Black;
            beginState[4, 5].State = Brushes.Black;
            beginState[4, 6].State = Brushes.Black;
            beginState[4, 7].State = Brushes.Black;
            beginState[5, 3].State = Brushes.Black;
            beginState[5, 4].State = Brushes.Black;
            beginState[5, 5].State = Brushes.Black;
            beginState[5, 6].State = Brushes.Black;
            beginState[5, 7].State = Brushes.Black;
            beginState[6, 3].State = Brushes.Black;
            beginState[6, 4].State = Brushes.Black;
            beginState[6, 5].State = Brushes.Black;
            beginState[6, 6].State = Brushes.Black;
            beginState[6, 7].State = Brushes.Black;
            beginState[7, 3].State = Brushes.Black;
            beginState[7, 4].State = Brushes.Black;
            beginState[7, 5].State = Brushes.Black;
            beginState[7, 6].State = Brushes.Black;
            beginState[7, 7].State = Brushes.Black;
            Game = new Game(beginState);

            for (int j = 0; j < height; j++)
                Map.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(cellSize) });
            for (int i = 0; i < width; i++)
                Map.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(cellSize) });

            Map.Width = width * cellSize;
            Map.Height = height * cellSize;
            this.Height = Map.Height + 40;
            this.Width = Map.Width + 15;

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
                    shape.MouseDown += (sender, e) => Game.CurrentState[x, y].ToggleState();
                    var binding = new Binding
                    {
                        Source = Game.CurrentState[x, y],
                        Path = new PropertyPath("State")
                    };
                    shape.SetBinding(Shape.FillProperty, binding);
                }
            }
        }
    }
}
