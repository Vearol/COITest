namespace ConsoleClientForPMC.DatabaseStorage.Models.PointModels
{
    public class DecimalPointModel
    {
        public int Id { get; set; }
        public byte Dimension { get; set; }
        public decimal X { get; set; }
        public decimal Y { get; set; }
        public decimal Z { get; set; }

        public DecimalPointModel(int id, byte dimension, decimal x, decimal y, decimal z)
        {
            Id = id;
            Dimension = dimension;
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString()
        {
            var coordinates = $"Point<Decimal> x: {X}";

            if (Dimension == 2)
                coordinates += $", y: {Y}";

            if (Dimension == 3)
                coordinates += $", y: {Y}, z: {Z}";

            return coordinates;
        }
    }
}
