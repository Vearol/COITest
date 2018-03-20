namespace ConsoleClientForPMC.DatabaseStorage.Models.PointModels
{
    public class DecimalPointModel
    {
        public int Id { get; set; }
        public byte Dimension { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public DecimalPointModel(int id, byte dimension, int x, int y, int z)
        {
            Id = id;
            Dimension = dimension;
            X = x;
            Y = y;
            Z = z;
        }
    }
}
