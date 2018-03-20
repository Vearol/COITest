namespace ConsoleClientForPMC.DatabaseStorage.Models.PointModels
{
    public class IntPointModel
    {
        public int Id { get; set; }
        public byte Dimension { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public IntPointModel(int id, byte dimension, int x, int y, int z)
        {
            Id = id;
            Dimension = dimension;
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString()
        {
            var coordinates = $"Point<Int> x: {X}";

            if (Dimension == 2)
                coordinates += $", y: {Y}";

            if (Dimension == 3)
                coordinates += $", y: {Y}, z: {Z}";

            return coordinates;
        }
    }
}
