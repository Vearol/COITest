namespace PMC.DataModels.Points
{
    public abstract class Point<T>
    {
        protected readonly T _x;
        protected readonly T _y;
        protected readonly T _z;

        protected Point(T x, T y, T z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        protected Point(T x)
        {
            _x = x;
        }

        protected Point(T x, T y) : this(x)
        {
            _y = y;
        }
    }
}
