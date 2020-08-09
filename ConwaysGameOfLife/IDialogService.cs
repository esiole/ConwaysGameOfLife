namespace ConwaysGameOfLife
{
    public interface IDialogService
    {
        void ShowMessage(string message);
        string FileName { get; set; }
        string Filter { get; set; }
        bool OpenFileDialog();
        bool SaveFileDialog();
    }
}
