namespace PMC.DataModels.Points
{
    public class OneDimensionPoint<T> : Point<T>
    {
        public T X { get { return _x; } }

        public OneDimensionPoint(T x) : base(x) { }
    }
}
