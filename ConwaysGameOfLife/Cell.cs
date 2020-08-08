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
        public static Visibility Dead = Visibility.Hidden;
        public static Visibility Alive = Visibility.Visible;
        private Visibility state;

        public bool IsAlive { get => state == Alive; }

        public Visibility State
        {
            get => state;
            set
            {
                state = value;
                OnPropertyChanged("State");
            }
        }

        public Cell(Visibility state = Visibility.Hidden)
        {
            State = state;
        }

        public void ToggleState()
        {
            State = IsAlive ? Dead : Alive;
            MessageBox.Show("dad");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
