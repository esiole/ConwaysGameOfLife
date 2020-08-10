using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ConwaysGameOfLife
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        public readonly Game Game;
        private SizeMap selectedSize;
        public readonly int CellSize = 30;
        private readonly IFileServiceAsync asyncFileService;
        private readonly IDialogService dialogService;
        private Command startCommand;
        private Command saveCommand;
        private Command openCommand;
        private Command toggleCommand;
        private Command randomCommand;

        public ObservableCollection<SizeMap> Sizes { get; private set; }
        public SizeMap SelectedSize
        {
            get => selectedSize;
            set
            {
                selectedSize = value;
                OnPropertyChanged("SelectedSize");
            }
        }
        public Command StartCommand
        {
            get
            {
                return startCommand ?? (startCommand = new Command(obj =>
                {
                    Game.StartAsync();
                },
                obj => !Game.IsStart
                ));
            }
        }
        public Command SaveCommand
        {
            get
            {
                return saveCommand ?? (saveCommand = new Command(obj =>
                {
                    WriteJsonAsync();
                },
                obj => !Game.IsStart
                ));
            }
        }
        public Command OpenCommand
        {
            get
            {
                return openCommand ?? (openCommand = new Command(obj =>
                {
                    ReadJsonAsync();
                },
                obj => !Game.IsStart
                ));
            }
        }
        public Command ToggleCommand
        {
            get
            {
                return toggleCommand ?? (toggleCommand = new Command(obj =>
                {
                    var cell = (Cell)obj;
                    cell.ToggleState();
                },
                obj => !Game.IsStart
                ));
            }
        }
        public Command RandomCommand
        {
            get
            {
                return randomCommand ?? (randomCommand = new Command(obj =>
                {
                    Game.ChangeStateOnTheMap(Game.CreateRandomMap(Game.WidthMap, Game.HeightMap));
                },
                obj => !Game.IsStart
                ));
            }
        }

        public ApplicationViewModel(IDialogService dialogService, IFileServiceAsync asyncFileService)
        {
            Game = new Game();
            this.dialogService = dialogService;
            this.asyncFileService = asyncFileService;
            this.dialogService.Filter = asyncFileService.DialogFilter;
            Sizes = new ObservableCollection<SizeMap>
            {
                new SizeMap(10, 10), new SizeMap(20, 30), new SizeMap(30, 20),
            };
            SelectedSize = Sizes[0];
        }

        private async void WriteJsonAsync()
        {
            try
            {
                if (dialogService.SaveFileDialog() == true)
                {
                    await asyncFileService.SaveAsync(dialogService.FileName, Game.CurrentState);
                }
            }
            catch (Exception e)
            {
                dialogService.ShowMessage(e.Message);
            }
        }

        private async void ReadJsonAsync()
        {
            try
            {
                if (dialogService.OpenFileDialog() == true)
                {
                    Game.ChangeStateOnTheMap(await asyncFileService.OpenAsync(dialogService.FileName));
                }
            }
            catch (Exception e)
            {
                dialogService.ShowMessage(e.Message);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
