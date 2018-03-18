using System;
using System.Collections.Generic;
using System.Linq;
using PMC.DataModels.Points;

namespace PMC.DataModels.DataModels
{
    public interface IMatrix<T>
    {
        short PositionCount { get; }
        Type DimensionType { get; }
        List<int> DataPointCounts { get; }
        bool Is3D { get; }
    }

    public class Matrix<T, TD> : IMatrix<T> where TD : Point<T>
    {
        private List<Position<T, TD>> _positions;

        public Matrix(List<Position<T, TD>> positions)
        {
            _positions = positions;
        }

        public List<Position<T, TD>> Positions
        {
            get { return _positions; }
        }

        public short PositionCount
        {
            get { return (short) _positions.Count; }
        }

        public Type DimensionType
        {
            get { return typeof(TD); }
        }

        public List<int> DataPointCounts
        {
            get { return _positions.Select(x => x.Points.Count).ToList(); }
        }

        public bool Is3D
        {
            get { return typeof(TD).ToString().Contains("Three"); }
        }
    }
}
