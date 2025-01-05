namespace ColorProblem
{
    public enum Color
    {
        Red,
        Green,
        Blue,
        Yellow
    }

    public class Region
    {
        public string Name { get; set; } // Ідентифікатор регіону
        public List<Region> Neighbours { get; set; } = new List<Region>();
        public Color? Color { get; set; } // Колір може бути відсутнім, поки не розфарбовано

        public Region(string name)
        {
            Name = name;
        }

        public void AddNeighbours(List<Region> neighbours)
        {
            Neighbours.AddRange(neighbours);
        }
    }
}
