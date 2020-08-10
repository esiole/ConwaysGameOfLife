using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace ConwaysGameOfLife
{
    public class Cell : INotifyPropertyChanged
    {
        public static SolidColorBrush Dead = Brushes.White;
        public static SolidColorBrush Alive = Brushes.Black;
        private SolidColorBrush state;
        private Color colorState;

        public bool IsAlive { get => colorState == Color.FromRgb(0, 0, 0); }

        public SolidColorBrush State
        {
            get => state;
            set
            {
                state = value;
                colorState = value.Color;
                OnPropertyChanged("State");
            }
        }

        public Cell(SolidColorBrush state = default)
        {
            if (state == null) State = Dead;
            else State = state;
        }

        public void ToggleState()
        {
            State = IsAlive ? Dead : Alive;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
