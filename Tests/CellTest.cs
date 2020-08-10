using System.Windows.Media;
using ConwaysGameOfLife;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class CellTest
    {
        [TestMethod]
        public void IsAlive()
        {
            var cell = new Cell();
            Assert.IsFalse(cell.IsAlive);
            cell = new Cell(Cell.Alive);
            Assert.IsTrue(cell.IsAlive);
            cell.State = Cell.Dead;
            Assert.IsFalse(cell.IsAlive);
            cell.State = Cell.Alive;
            Assert.IsTrue(cell.IsAlive);
            cell.ToggleState();
            Assert.IsFalse(cell.IsAlive);
            cell.ToggleState();
            Assert.IsTrue(cell.IsAlive);
            cell = new Cell(new SolidColorBrush(Color.FromRgb(255, 255, 255)));
            Assert.IsFalse(cell.IsAlive);
            cell = new Cell(new SolidColorBrush(Color.FromRgb(255, 0, 0)));
            Assert.IsFalse(cell.IsAlive);
            cell = new Cell(new SolidColorBrush(Color.FromRgb(0, 0, 0)));
            Assert.IsTrue(cell.IsAlive);
        }
    }
}
