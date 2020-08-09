﻿using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Xaml.Behaviors;

namespace ConwaysGameOfLife
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int widthMap = 10;
        private int heightMap = 10;
        private int cellSize = 30;
        private Game Game;

        public MainWindow()
        {
            InitializeComponent();
            CreateMapButton.Click += (sender, e) =>
            {
                StartInfoPanel.Children.Clear();
                ToolBar.Visibility = Visibility.Visible;
                CreateMap(widthMap, heightMap, cellSize);
            };
            SizeMenu.AddHandler(RadioButton.CheckedEvent, new RoutedEventHandler(RadioButtonSizeChecked));
        }

        private void CreateMap(int width, int height, int cellSize)
        {
            var beginState = Game.CreateState(width, height);
            Game = new Game(beginState, new DefaultDialogService(), new JsonAsyncFileService());

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

                    //shape.MouseDown += (sender, e) => Game.CurrentState[x, y].ToggleState();
                    var trigger = new Microsoft.Xaml.Behaviors.EventTrigger("MouseDown");
                    var commandAction = new InvokeCommandAction()
                    {
                        Command = Game.ToggleCommand,
                        CommandParameter = Game.CurrentState[x, y],
                    };
                    trigger.Actions.Add(commandAction);
                    Interaction.GetTriggers(shape).Add(trigger);

                    var binding = new Binding
                    {
                        Source = Game.CurrentState[x, y],
                        Path = new PropertyPath("State")
                    };
                    shape.SetBinding(Shape.FillProperty, binding);
                }
            }

            StartGameButton.Command = Game.StartCommand;
            SaveMapButton.Command = Game.SaveCommand;
            LoadMapButton.Command = Game.OpenCommand;
        }

        private void RadioButtonSizeChecked(object sender, RoutedEventArgs e)
        {
            var pressed = (RadioButton)e.Source;
            var text = pressed.Content.ToString();
            var data = text.Split('x');
            widthMap = int.Parse(data[0]);
            heightMap = int.Parse(data[1]);
        }
    }
}
