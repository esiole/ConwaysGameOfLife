using System.IO;
using System.Threading.Tasks;
using ConwaysGameOfLife;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class OpenSaveTest
    {
        private async Task Service(string fileName, IFileServiceAsync service)
        {
            var map = Game.CreateRandomMap(15, 10);
            await service.SaveAsync(fileName, map);
            var readMap = await service.OpenAsync(fileName);
            File.Delete(fileName);
            for (int i = 0; i < map.GetLength(0); i++)
                for (int j = 0; j < map.GetLength(1); j++)
                    Assert.AreEqual(map[i, j].IsAlive, readMap[i, j].IsAlive);
        }

        [TestMethod]
        public async Task JsonService()
        {
            await Service("test.json", new JsonAsyncFileService());
        }
    }
}
