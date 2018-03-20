namespace ConsoleClientForPMC.DatabaseStorage.Models.PointModels
{
    public class DoublePointModel
    {
        public int Id { get; set; }
        public byte Dimension { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public DoublePointModel(int id, byte dimension, double x, double y, double z)
        {
            Id = id;
            Dimension = dimension;
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString()
        {
            var coordinates = $"Point<Double> x: {X}";

            if (Dimension == 2)
                coordinates += $", y: {Y}";

            if (Dimension == 3)
                coordinates += $", y: {Y}, z: {Z}";

            return coordinates;
        }
    }
}
