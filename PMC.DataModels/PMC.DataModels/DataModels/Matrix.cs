using System.Collections.Generic;
using System.Linq;
using PMC.DataModels.TestHelper;

namespace PMC.DataModels.DataModels
{
    /// <summary>
    /// Matrix contains array of Position, which should be the same type as the Matrix itself
    /// </summary>
    /// <typeparam name="T">Type of Matrix and Positions</typeparam>
    public class Matrix<T> : IDimension
    {
        private readonly Position<T>[] _positions;

        /// <param name="positions">Array of Position</param>
        public Matrix(Position<T>[] positions)
        {
            Validate(positions);

            _positions = new Position<T>[positions.Length];
            for (var i = 0; i < positions.Length; i++)
            {
                _positions[i] = positions[i];
            }
        }

        /// <returns>Array of Position in the current Matrix</returns>
        public Position<T>[] Positions
        {
            get { return _positions; }
        }

        /// <returns>Returns Position with given index</returns>
        public Position<T> this[int i]
        {
            get { return _positions[i]; }
        }

        /// <returns>Dimension of the current Matrix</returns>
        public Dimension Dimension
        {
            get { return _positions.First().Dimension; }
        }

        /// <returns>Number of Positions in the current Matrix</returns>
        public short PositionCount
        {
            get { return (short) _positions.Length; }
        }

        /// <returns>Array of number of Points in each Position</returns>
        public IEnumerable<int> DataPointCounts
        {
            get { return _positions.Select(x => x.Points.Length); }
        }

        /// <returns>Checks if current Matrix is from third dimension</returns>
        public bool Is3D
        {
            get { return Dimension == Dimension.D3; }
        }

        private void Validate(Position<T>[] positions)
        {
            var dimension = positions[0].Dimension;
            foreach (var position in positions)
            {
                if (position.Dimension != dimension)
                    throw new CustomException("Not all points in position have the same dimension", (byte)CustomErrorCode.IncorrectTypeOfSomePositions);
            }
        }
    }
}
