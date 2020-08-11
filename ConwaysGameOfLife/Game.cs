using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ConwaysGameOfLife
{
    public class Game
    {
        public int WidthMap;
        public int HeightMap;
        private int countStabilityZone = 0;

        public Cell[,] CurrentState { get; private set; }
        public Cell[,] NextState { get; private set; }
        public bool IsStart { get; set; }

        public Game() { }

        public void SetMap(Cell[,] map)
        {
            HeightMap = map.GetLength(0);
            WidthMap = map.GetLength(1);
            CurrentState = map;
            NextState = Game.CreateMap(WidthMap, HeightMap);
        }

        public void ChangeStateOnTheMap(Cell[,] map)
        {
            if (HeightMap != map.GetLength(0) || WidthMap != map.GetLength(1))
                throw new ArgumentException("Размерности переданного массива не совпадают с размерностями текущего состояния.");
            for (int i = 0; i < HeightMap; i++)
                for (int j = 0; j < WidthMap; j++)
                    CurrentState[i, j].State = map[i, j].State;
        }

        public void UpdateState(int widthStart, int widthEnd)
        {
            bool isStability = true;
            for (int i = 0; i < HeightMap; i++)
            {
                for (int j = widthStart; j < widthEnd; j++)
                {
                    if (isStability && CurrentState[i, j].State != NextState[i, j].State)
                            isStability = false;
                    CurrentState[i, j].State = NextState[i, j].State;
                    NextState[i, j] = new Cell();
                }
            }
            if (isStability) countStabilityZone++;
        }

        public static Cell[,] CreateMap(int width, int height)
        {
            var map = new Cell[height, width];
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    map[i, j] = new Cell();
            return map;
        }

        public static Cell[,] CreateRandomMap(int width, int height)
        {
            var random = new Random();
            var map = new Cell[height, width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (random.Next(2) == 0) map[i, j] = new Cell();
                    else  map[i, j] = new Cell(Cell.Alive);
                }
            }
            return map;
        }
        
        public async void StartAsync()
        {
            IsStart = true;
            await Task.Run(() =>
            {
                while (IsStart)
                {
                    Thread.Sleep(100);
                    int max = 12;
                    Parallel.For(0, max, (i) => Iteration(i * WidthMap / max, (i + 1) * WidthMap / max));
                    Parallel.For(0, max, (i) => UpdateState(i * WidthMap / max, (i + 1) * WidthMap / max));
                    if (countStabilityZone == max)
                    {
                        IsStart = false;
                        MessageBox.Show("Игра закончена!");
                    }    
                    countStabilityZone = 0;
                }
            });
        }
        
        public void Iteration(int widthStart, int widthEnd)
        {
            for (int i = 0; i < HeightMap; i++)
            {
                for (int j = widthStart; j < widthEnd; j++)
                {
                    var cell = CurrentState[i, j];
                    var countAliveNeighbour = GetCountAliveNeighbour(i, j);
                    if (cell.IsAlive)
                    {
                        if (countAliveNeighbour == 2 || countAliveNeighbour == 3)
                            NextState[i, j].State = Cell.Alive;
                    }
                    else
                    {
                        if (countAliveNeighbour == 3)
                            NextState[i, j].State = Cell.Alive;
                    }
                }
            }
        }

        public int GetCountAliveNeighbour(int i, int j)
        {
            int countAlive = 0;

            int iIncrement = i + 1;
            if (iIncrement >= HeightMap) iIncrement = 0;
            int iDecrement = i - 1;
            if (iDecrement < 0) iDecrement = HeightMap - 1;
            int jIncrement = j + 1;
            if (jIncrement >= WidthMap) jIncrement = 0;
            int jDecrement = j - 1;
            if (jDecrement < 0) jDecrement = WidthMap - 1;

            if (CurrentState[iDecrement, jDecrement].IsAlive) countAlive++;
            if (CurrentState[i, jDecrement].IsAlive) countAlive++;
            if (CurrentState[iIncrement, jDecrement].IsAlive) countAlive++;
            if (CurrentState[iDecrement, j].IsAlive) countAlive++;
            if (CurrentState[iIncrement, j].IsAlive) countAlive++;
            if (CurrentState[iDecrement, jIncrement].IsAlive) countAlive++;
            if (CurrentState[i, jIncrement].IsAlive) countAlive++;
            if (CurrentState[iIncrement, jIncrement].IsAlive) countAlive++;

            return countAlive;
        }
    }
}
