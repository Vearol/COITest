namespace PMC.DataModels.Points
{
    public class ThreeDimensionPoint<T> : Point<T>
    {
        public T X { get { return _x; } }
        public T Y { get { return _y; } }
        public T Z { get { return _z; } }

        public ThreeDimensionPoint(T x, T y, T z) : base(x, y, z)
        {}
    }
}
