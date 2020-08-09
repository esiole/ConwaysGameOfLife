using System.Threading.Tasks;

namespace ConwaysGameOfLife
{
    public interface IFileServiceAsync
    {
        string DialogFilter { get; }
        Task<Cell[,]> OpenAsync(string fileName);
        Task SaveAsync(string fileName, Cell[,] map);
    }
}
