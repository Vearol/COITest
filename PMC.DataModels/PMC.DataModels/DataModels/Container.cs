using System.Collections.Generic;

namespace PMC.DataModels.DataModels
{
    public class Container<T>
    {
        private List<IMatrix<T>> _matrices;

        public Container(List<IMatrix<T>> matrices)
        {
            _matrices = matrices;
        }
        
        public List<IMatrix<T>> Matrices
        {
            get { return _matrices; }
        }
    }
}
