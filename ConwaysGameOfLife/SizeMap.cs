namespace ConwaysGameOfLife
{
    public class SizeMap
    {
        public int WidthCellCount { get; private set; }
        public int HeightCellCount { get; private set; }
        public string WidthStr { get => GetStrSize(WidthCellCount); }
        public string HeightStr { get => GetStrSize(HeightCellCount); }

        public SizeMap(int widthCellCount, int heightCellCount)
        {
            WidthCellCount = widthCellCount;
            HeightCellCount = heightCellCount;
        }

        private string GetStrSize(int size)
        {
            return $"= {size} клеток";
        }
    }
}
