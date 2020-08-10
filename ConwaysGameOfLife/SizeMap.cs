namespace ConwaysGameOfLife
{
    public class SizeMap
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public string WidthStr { get => $"Ширина карты: {Width}"; }
        public string HeightStr { get => $"Высота карты: {Height}"; }

        public SizeMap(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
