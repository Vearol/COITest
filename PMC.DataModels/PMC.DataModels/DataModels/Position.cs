using System.Collections.Generic;
using PMC.DataModels.Points;

namespace PMC.DataModels.DataModels
{
    public class Position<T, TD> where TD : Point<T>
    {
        private List<TD> _points;

        public Position(List<TD> points)
        {
            _points = points;
        }

        public List<TD> Points
        {
            get { return _points; }
        }
    }
}
