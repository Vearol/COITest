using System.Collections.Generic;
using System.Linq;

namespace PMC.DataModels.DataModels
{
    public class Matrix<T> : IDimension
    {
        private readonly Position<T>[] _positions;

        public Matrix(Position<T>[] positions)
        {
            _positions = new Position<T>[positions.Length];
            for (var i = 0; i < positions.Length; i++)
            {
                _positions[i] = positions[i];
            }
        }

        public Position<T>[] Positions
        {
            get { return _positions; }
        }

        public Dimension Dimension
        {
            get { return _positions.First().Dimension; }
        }

        public short PositionCount
        {
            get { return (short) _positions.Length; }
        }
        
        public IEnumerable<int> DataPointCounts
        {
            get { return _positions.Select(x => x.Points.Length); }
        }

        public bool Is3D
        {
            get { return Dimension == Dimension.D3; }
        }
    }
}
