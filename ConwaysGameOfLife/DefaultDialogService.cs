using Microsoft.Win32;
using System.Windows;

namespace ConwaysGameOfLife
{
    public class DefaultDialogService : IDialogService
    {
        public string FileName { get; set; }

        public bool OpenFileDialog()
        {
            var dialog = new OpenFileDialog();
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
