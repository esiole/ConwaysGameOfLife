using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConwaysGameOfLife
{
    public class JsonAsyncFileService : IFileServiceAsync
    {
        public async Task<Cell[,]> OpenAsync(string fileName)
        {
            Cell[,] map;
            string json;
            using (var stream = new StreamReader(fileName))
            {
                json = await stream.ReadToEndAsync();
            }
            map = JsonConvert.DeserializeObject<Cell[,]>(json);
            return map;
        }

        public async Task SaveAsync(string fileName, Cell[,] map)
        {
            throw new NotImplementedException();
        }
    }
}
