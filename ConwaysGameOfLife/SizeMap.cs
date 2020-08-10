namespace ConwaysGameOfLife
{
    public class SizeMap
    {
        public int WidthCellCount { get; private set; }
        public int HeightCellCount { get; private set; }
        public string WidthStr { get => $"Ширина карты: {WidthCellCount}"; }
        public string HeightStr { get => $"Высота карты: {HeightCellCount}"; }

        public SizeMap(int widthCellCount, int heightCellCount)
        {
            WidthCellCount = widthCellCount;
            HeightCellCount = heightCellCount;
        }
    }
}
