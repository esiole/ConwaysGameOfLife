using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ConwaysGameOfLife
{
    public class Game
    {
        public readonly int Width;
        public readonly int Height;

        public Cell[,] CurrentState { get; private set; }
        public Cell[,] NextState { get; private set; }
        public bool IsStart { get; private set; }

        private IFileServiceAsync asyncFileService;
        private IDialogService dialogService;

        private Command startCommand;
        public Command StartCommand
        {
            get
            {
                return startCommand ?? (startCommand = new Command(obj =>
                {
                    IsStart = true;
                    Start();
                }, 
                obj => !IsStart
                ));
            }
        }

        private Command saveCommand;
        public Command SaveCommand
        {
            get
            {
                return saveCommand ?? (saveCommand = new Command(obj =>
                {
                    WriteJsonAsync();
                },
                obj => !IsStart
                ));
            }
        }

        private Command openCommand;
        public Command OpenCommand
        {
            get
            {
                return openCommand ?? (openCommand = new Command(obj =>
                {
                    ReadJsonAsync();
                },
                obj => !IsStart
                ));
            }
        }

        public Game(Cell[,] beginState, IDialogService dialogService, IFileServiceAsync asyncFileService)
        {
            Height = beginState.GetLength(0);
            Width = beginState.GetLength(1);
            CurrentState = beginState;
            NextState = Game.CreateState(Width, Height);
            this.dialogService = dialogService;
            this.asyncFileService = asyncFileService;
        }

        public static Cell[,] CreateState(int width, int height)
        {
            var map = new Cell[height, width];
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    map[i, j] = new Cell();
            return map;
        }

        public async void Start()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(1000);
                    Iteration();
                }
            });
        }

        public void Iteration()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    var cell = CurrentState[i, j];
                    var countAliveNeighbour = GetCountAliveNeighbour(i, j);
                    if (cell.IsAlive)
                    {
                        if (countAliveNeighbour == 2 || countAliveNeighbour == 3)
                            NextState[i, j].State = Cell.Alive;
                    }
                    else
                    {
                        if (countAliveNeighbour == 3)
                            NextState[i, j].State = Cell.Alive;
                    }
                }
            }

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    CurrentState[i, j].State = NextState[i, j].State;
                    NextState[i, j] = new Cell();
                    //
                }
            }
        }

        public int GetCountAliveNeighbour(int i, int j)
        {
            int countAlive = 0;

            int iIncrement = i + 1;
            if (iIncrement >= Height) iIncrement = 0;
            int iDecrement = i - 1;
            if (iDecrement < 0) iDecrement = Height - 1;
            int jIncrement = j + 1;
            if (jIncrement >= Width) jIncrement = 0;
            int jDecrement = j - 1;
            if (jDecrement < 0) jDecrement = Width - 1;

            if (CurrentState[iDecrement, jDecrement].IsAlive) countAlive++;
            if (CurrentState[i, jDecrement].IsAlive) countAlive++;
            if (CurrentState[iIncrement, jDecrement].IsAlive) countAlive++;
            if (CurrentState[iDecrement, j].IsAlive) countAlive++;
            if (CurrentState[iIncrement, j].IsAlive) countAlive++;
            if (CurrentState[iDecrement, jIncrement].IsAlive) countAlive++;
            if (CurrentState[i, jIncrement].IsAlive) countAlive++;
            if (CurrentState[iIncrement, jIncrement].IsAlive) countAlive++;

            return countAlive;
        }

        private async void WriteJsonAsync()
        {
            var dialog = new SaveFileDialog()
            {
                Filter = "json files (*.json)|*.json",
            };
            dialog.ShowDialog();
            using (var stream = new StreamWriter(dialog.FileName))
            {
                var json = JsonConvert.SerializeObject(CurrentState);
                await stream.WriteLineAsync(json);
            }
        }

        private async void ReadJsonAsync()
        {
            //var dialog = new OpenFileDialog()
            //{
            //    Filter = "json files (*.json)|*.json",
            //};
            //dialog.ShowDialog();
            //using (var stream = new StreamReader(dialog.FileName))
            //{
            //    var json = await stream.ReadToEndAsync();
            //    var map = JsonConvert.DeserializeObject<Cell[,]>(json);
            //    for (int i = 0; i < Height; i++)
            //        for (int j = 0; j < Width; j++)
            //            CurrentState[i, j].State = map[i, j].State;
            //}
            try
            {
                if (dialogService.OpenFileDialog() == true)
                {
                    var map = await asyncFileService.OpenAsync(dialogService.FileName);
                    for (int i = 0; i < Height; i++)
                        for (int j = 0; j < Width; j++)
                            CurrentState[i, j].State = map[i, j].State;
                    dialogService.ShowMessage("Файл открыт");
                }
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage(ex.Message);
            }
        }
    }
}
