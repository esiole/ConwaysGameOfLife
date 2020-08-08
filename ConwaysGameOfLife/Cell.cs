using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ConwaysGameOfLife
{
    public class Cell : INotifyPropertyChanged
    {
        public static SolidColorBrush Dead = Brushes.White;
        public static SolidColorBrush Alive = Brushes.Black;
        private SolidColorBrush state;

        public bool IsAlive { get => state == Alive; }

        public SolidColorBrush State
        {
            get => state;
            set
            {
                state = value;
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
