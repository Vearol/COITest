namespace PMC.DataModels.Points
{
    public class TwoDimensionPoint<T> : Point<T>
    {
        public T X { get { return _x; } }
        public T Y { get { return _y; } }

        public TwoDimensionPoint(T x, T y) : base(x, y)
        {}
    }
}
