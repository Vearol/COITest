namespace ConsoleClientForPMC.DatabaseStorage.Models
{
    public class PointModel<T>
    {
        public int Id { get; set; }
        public byte Dimension { get; set; }
        public T X { get; set; }
        public T Y { get; set; }
        public T Z { get; set; }

        public PointModel(int id, byte dimension, T x, T y, T z)
        {
            Id = id;
            Dimension = dimension;
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString()
        {
            var coordinates = $"Point<{typeof(T)}> x: {X}";

            if (Dimension == 2)
                coordinates += $", y: {Y}";

            if (Dimension == 3)
                coordinates += $", y: {Y}, z: {Z}";

            return coordinates;
        }

        public T[] GetPointArray()
        {
            switch (Dimension)
            {
                case 2:
                    return new T[] { X, Y };
                case 3:
                    return new T[] { X, Y, Z };
                default:
                    return new T[] { X };
            }
        }
    }
}
