namespace PMC.DataModels.DataModels
{
    public class Position<T> : IDimension
    {
        private readonly Point<T>[] _points;

        public Position(Point<T>[] points, Dimension pointDim)
        {
            Dimension = pointDim;
            _points = new Point<T>[points.Length];

            for (var i = 0; i < _points.Length; i++)
            {
                _points[i] = new Point<T>(points[i]);
            }
        }

        public Position(Dimension pointDim)
        {
            Dimension = pointDim;
        }

        public Point<T>[] Points
        {
            get { return _points; }
        }

        public Point<T> this[int i]
        {
            get { return _points[i]; }
        }

        public Dimension Dimension { get; }
    }
}
