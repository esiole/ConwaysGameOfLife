using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace ConwaysGameOfLife
{
    public class JsonAsyncFileService : IFileServiceAsync
    {
        public string DialogFilter { get => "json files (*.json)|*.json"; }

        public async Task<Cell[,]> OpenAsync(string fileName)
        {
            string json;
            using (var stream = new StreamReader(fileName))
            {
                json = await stream.ReadToEndAsync();
            }
            return JsonConvert.DeserializeObject<Cell[,]>(json);
        }

        public async Task SaveAsync(string fileName, Cell[,] map)
        {
            var json = JsonConvert.SerializeObject(map);
            using (var stream = new StreamWriter(fileName))
            {
                await stream.WriteLineAsync(json);
            }
        }
    }
}
