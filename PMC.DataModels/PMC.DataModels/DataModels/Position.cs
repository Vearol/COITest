using PMC.DataModels.TestHelper;

namespace PMC.DataModels.DataModels
{
    /// <summary>
    /// Position contains array of Point, which should be the same type as the Position itself
    /// </summary>
    /// <typeparam name="T">Type of Position and Point</typeparam>
    public class Position<T> : IDimension
    {
        private readonly Point<T>[] _points;
        
        /// <param name="points">Array of Point</param>
        /// <param name="pointDim">Dimension of Point. Should be in constructor, because Position may contain no points.</param>
        public Position(Point<T>[] points, Dimension pointDim)
        {
            Dimension = pointDim;

            Validate(points);
            _points = new Point<T>[points.Length];

            for (var i = 0; i < _points.Length; i++)
            {
                _points[i] = new Point<T>(points[i]);
            }
        }

        /// <param name="pointDim">Dimension of Point. Should be in constructor, because Position may contain no points.</param>
        public Position(Dimension pointDim)
        {
            Dimension = pointDim;
        }

        /// <returns>Array of Point in the current Position</returns>
        public Point<T>[] Points
        {
            get { return _points; }
        }

        /// <returns>Returns Point with given index</returns>
        public Point<T> this[int i]
        {
            get { return _points[i]; }
        }

        /// <returns>Dimension of the current Position</returns>
        public Dimension Dimension { get; }

        private void Validate(Point<T>[] points)
        {
            foreach (var point in points)
            {
                if (point.Dimension != Dimension)
                    throw new CustomException("Not all points in position have the same dimension", (byte)CustomErrorCode.IncorrectTypeOfSomePoints);
            }
        }
    }
}
