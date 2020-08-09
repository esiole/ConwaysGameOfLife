using System;
using ConwaysGameOfLife;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class GameTest
    {
        private void TestCountAliveNeighbour(Cell[,] state, int i, int j, int expectedResult)
        {
            var game = new Game(state, new DefaultDialogService(), new JsonAsyncFileService());
            Assert.AreEqual(expectedResult, game.GetCountAliveNeighbour(i, j));
        }

        private void TestIteration(Cell[,] beginState, Cell[,] endState)
        {
            var game = new Game(beginState, new DefaultDialogService(), new JsonAsyncFileService());
            game.Iteration();
            for (int i = 0; i < beginState.GetLength(0); i++)
                for (int j = 0; j < beginState.GetLength(1); j++)
                    Assert.AreEqual(game.CurrentState[i, j].State, endState[i, j].State);
        }
        
        [TestMethod]
        public void SizeMap()
        {
            var state = new[,]
            {
                { new Cell(), new Cell(), new Cell(), new Cell(), },
                { new Cell(), new Cell(), new Cell(), new Cell(), },
            };
            var game = new Game(state, new DefaultDialogService(), new JsonAsyncFileService());
            Assert.AreEqual(2, game.Height);
            Assert.AreEqual(4, game.Width);
        }

        [TestMethod]
        public void AllAliveNeighbour()
        {
            var state = Game.CreateState(3, 3);
            foreach (var e in state)
                e.State = Cell.Alive;
            TestCountAliveNeighbour(state, 2, 2, 8);
        }

        [TestMethod]
        public void AllDeadNeighbour()
        {
            TestCountAliveNeighbour(Game.CreateState(3, 3), 2, 2, 0);
        }

        [TestMethod]
        public void HorizontalScroll()
        {
            var state = Game.CreateState(3, 3);
            state[0, 2].State = Cell.Alive;
            TestCountAliveNeighbour(state, 0, 0, 1);
            state = Game.CreateState(3, 4);
            state[2, 0].State = Cell.Alive;
            TestCountAliveNeighbour(state, 2, 2, 1);
        }

        [TestMethod]
        public void VerticalScroll()
        {
            var state = Game.CreateState(4, 3);
            state[2, 0].State = Cell.Alive;
            TestCountAliveNeighbour(state, 0, 0, 1);
            state = Game.CreateState(3, 3);
            state[0, 2].State = Cell.Alive;
            TestCountAliveNeighbour(state, 2, 2, 1);
        }

        [TestMethod]
        public void DiagonalScroll()
        {
            var state = Game.CreateState(3, 3);
            state[2, 2].State = Cell.Alive;
            TestCountAliveNeighbour(state, 0, 0, 1);
            state = Game.CreateState(3, 3);
            state[0, 0].State = Cell.Alive;
            state[0, 1].State = Cell.Alive;
            TestCountAliveNeighbour(state, 2, 2, 2);
            state = Game.CreateState(3, 3);
            state[2, 0].State = Cell.Alive;
            state[2, 2].State = Cell.Alive;
            TestCountAliveNeighbour(state, 0, 1, 2);
        }

        [TestMethod]
        public void Cycle3Group()
        {
            Func<Cell[,]> getState1 = () =>
            {
                var map = Game.CreateState(5, 5);
                map[1, 2].State = Cell.Alive;
                map[2, 2].State = Cell.Alive;
                map[3, 2].State = Cell.Alive;
                return map;
            };
            Func<Cell[,]> getState2 = () =>
            {
                var map = Game.CreateState(5, 5);
                map[2, 1].State = Cell.Alive;
                map[2, 2].State = Cell.Alive;
                map[2, 3].State = Cell.Alive;
                return map;
            };
            TestIteration(getState1(), getState2());
            TestIteration(getState2(), getState1());
        }

        [TestMethod]
        public void Stability3Group()
        {
            Func<Cell[,]> getState1 = () =>
            {
                var map = Game.CreateState(4, 4);
                map[1, 1].State = Cell.Alive;
                map[1, 2].State = Cell.Alive;
                map[2, 1].State = Cell.Alive;
                return map;
            };
            Func<Cell[,]> getState2 = () =>
            {
                var map = getState1();
                map[2, 2].State = Cell.Alive;
                return map;
            };
            TestIteration(getState1(), getState2());
            TestIteration(getState2(), getState2());
        }

        private Cell[,] PointState()
        {
            var map = Game.CreateState(5, 5);
            map[2, 2].State = Cell.Alive;
            return map;
        }

        private Cell[,] SecondPointHorizontalState()
        {
            var map = Game.CreateState(5, 5);
            map[2, 2].State = Cell.Alive;
            map[2, 3].State = Cell.Alive;
            return map;
        }

        private Cell[,] SecondPointVerticalState()
        {
            var map = Game.CreateState(5, 5);
            map[2, 2].State = Cell.Alive;
            map[3, 2].State = Cell.Alive;
            return map;
        }

        /// <summary>
        /// 00000
        /// 00100
        /// 00010
        /// 00100
        /// 00000
        /// </summary>
        [TestMethod]
        public void Wiki3Group1()
        {
            Func<Cell[,]> getState1 = () =>
            {
                var map = Game.CreateState(5, 5);
                map[1, 2].State = Cell.Alive;
                map[3, 2].State = Cell.Alive;
                map[2, 3].State = Cell.Alive;
                return map;
            };
            TestIteration(getState1(), SecondPointHorizontalState());
        }

        /// <summary>
        /// 00000
        /// 00100
        /// 00100
        /// 00010
        /// 00000
        /// </summary>
        [TestMethod]
        public void Wiki3Group2()
        {
            Func<Cell[,]> getState1 = () =>
            {
                var map = Game.CreateState(5, 5);
                map[1, 2].State = Cell.Alive;
                map[2, 2].State = Cell.Alive;
                map[3, 3].State = Cell.Alive;
                return map;
            };
            TestIteration(getState1(), SecondPointHorizontalState());
        }

        /// <summary>
        /// 00000
        /// 00010
        /// 00100
        /// 01000
        /// 00000
        /// </summary>
        [TestMethod]
        public void Wiki3Group3()
        {
            Func<Cell[,]> getState1 = () =>
            {
                var map = Game.CreateState(5, 5);
                map[1, 3].State = Cell.Alive;
                map[2, 2].State = Cell.Alive;
                map[3, 1].State = Cell.Alive;
                return map;
            };
            TestIteration(getState1(), PointState());
        }

        /// <summary>
        /// 00000
        /// 01000
        /// 00010
        /// 00100
        /// 00000
        /// </summary>
        [TestMethod]
        public void Wiki3Group4()
        {
            Func<Cell[,]> getState1 = () =>
            {
                var map = Game.CreateState(5, 5);
                map[1, 1].State = Cell.Alive;
                map[2, 3].State = Cell.Alive;
                map[3, 2].State = Cell.Alive;
                return map;
            };
            TestIteration(getState1(), PointState());
        }

        /// <summary>
        /// 00000
        /// 01010
        /// 00000
        /// 00100
        /// 00000
        /// </summary>
        [TestMethod]
        public void Wiki3Group5()
        {
            Func<Cell[,]> getState1 = () =>
            {
                var map = Game.CreateState(5, 5);
                map[1, 1].State = Cell.Alive;
                map[1, 3].State = Cell.Alive;
                map[3, 2].State = Cell.Alive;
                return map;
            };
            TestIteration(getState1(), PointState());
        }

        /// <summary>
        /// 00000
        /// 01010
        /// 00000
        /// 00010
        /// 00000
        /// </summary>
        [TestMethod]
        public void Wiki3Group6()
        {
            Func<Cell[,]> getState1 = () =>
            {
                var map = Game.CreateState(5, 5);
                map[1, 1].State = Cell.Alive;
                map[1, 3].State = Cell.Alive;
                map[3, 3].State = Cell.Alive;
                return map;
            };
            TestIteration(getState1(), PointState());
        }

        /// <summary>
        /// 00000
        /// 00000
        /// 01010
        /// 00010
        /// 00000
        /// </summary>
        [TestMethod]
        public void Wiki3Group7()
        {
            Func<Cell[,]> getState1 = () =>
            {
                var map = Game.CreateState(5, 5);
                map[2, 1].State = Cell.Alive;
                map[2, 3].State = Cell.Alive;
                map[3, 3].State = Cell.Alive;
                return map;
            };
            TestIteration(getState1(), SecondPointVerticalState());
        }

        [TestMethod]
        public void FromPointToNull()
        {
            TestIteration(PointState(), Game.CreateState(5, 5));
        }

        /// <summary>
        /// 000||000
        /// 011||010
        /// 000||010
        /// </summary>
        [TestMethod]
        public void Group2ToNull()
        {
            TestIteration(SecondPointHorizontalState(), Game.CreateState(5, 5));
            TestIteration(SecondPointVerticalState(), Game.CreateState(5, 5));
        }
    }
}
