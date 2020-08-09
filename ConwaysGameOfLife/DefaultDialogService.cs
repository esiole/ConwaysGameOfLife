using Microsoft.Win32;
using System.Windows;

namespace ConwaysGameOfLife
{
    public class DefaultDialogService : IDialogService
    {
        public string FileName { get; set; }
        public string Filter { get; set; }

        public bool OpenFileDialog()
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = Filter;
            if (dialog.ShowDialog() == true)
            {
                FileName = dialog.FileName;
                return true;
            }
            return false;
        }

        public bool SaveFileDialog()
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = Filter;
            if (dialog.ShowDialog() == true)
            {
                FileName = dialog.FileName;
                return true;
            }
            return false;
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
