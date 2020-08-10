using System.Threading;
using System.Threading.Tasks;

namespace ConwaysGameOfLife
{
    public class Game
    {
        public int Width;
        public int Height;

        public Cell[,] CurrentState { get; private set; }
        public Cell[,] NextState { get; private set; }
        public bool IsStart { get; private set; }

        public Game()
        {

        }

        public Game(Cell[,] beginState)
        {
            Height = beginState.GetLength(0);
            Width = beginState.GetLength(1);
            CurrentState = beginState;
            NextState = Game.CreateState(Width, Height);
        }

        public void SetState(Cell[,] state)
        {
            Height = state.GetLength(0);
            Width = state.GetLength(1);
            CurrentState = state;
            NextState = Game.CreateState(Width, Height);
        }

        public void Test(Cell[,] map)
        {
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                    CurrentState[i, j].State = map[i, j].State;
        }

        public static Cell[,] CreateState(int width, int height)
        {
            var map = new Cell[height, width];
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    map[i, j] = new Cell();
            return map;
        }

        public async void Start()
        {
            IsStart = true;
            await Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(1000);
                    Iteration();
                }
            });
        }

        public void Iteration()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
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

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    CurrentState[i, j].State = NextState[i, j].State;
                    NextState[i, j] = new Cell();
                    //
                }
            }
        }

        public int GetCountAliveNeighbour(int i, int j)
        {
            int countAlive = 0;

            int iIncrement = i + 1;
            if (iIncrement >= Height) iIncrement = 0;
            int iDecrement = i - 1;
            if (iDecrement < 0) iDecrement = Height - 1;
            int jIncrement = j + 1;
            if (jIncrement >= Width) jIncrement = 0;
            int jDecrement = j - 1;
            if (jDecrement < 0) jDecrement = Width - 1;

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
