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
        public readonly Game Game;

        public MainWindow()
        {
            InitializeComponent();

            var width = 15;
            var height = 10;
            var cellSize = 30;

            var beginState = Game.CreateState(width, height);
            beginState[0, 3].State = Brushes.Black;
            beginState[1, 4].State = Brushes.Black;
            beginState[2, 4].State = Brushes.Black;
            beginState[2, 3].State = Brushes.Black;
            beginState[2, 2].State = Brushes.Black;
            Game = new Game(beginState);

            for (int j = 0; j < height; j++)
                Grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(cellSize) });
            for (int i = 0; i < width; i++)
                Grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(cellSize) });

            Grid.Width = width * cellSize;
            Grid.Height = height * cellSize;
            this.Height = Grid.Height + 40;
            this.Width = Grid.Width + 15;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    var shape = new Ellipse()
                    {
                        Width = cellSize,
                        Height = cellSize,
                    };
                    Grid.Children.Add(shape);
                    Grid.SetRow(shape, i);
                    Grid.SetColumn(shape, j);

                    var binding = new Binding
                    {
                        Source = Game.CurrentState[i, j],
                        Path = new PropertyPath("State")
                    };
                    shape.SetBinding(Shape.FillProperty, binding);
                }
            }
            Game.Start();
        }
    }
}
