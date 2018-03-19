namespace PMC.DataModels.DataModels
{
    public class Container<T>
    {
        private readonly Matrix<T>[] _matrices;

        public Container(Matrix<T>[] matrices)
        {
            _matrices = matrices;
        }
        
        public Matrix<T>[] Matrices
        {
            get { return _matrices; }
        }

        public Matrix<T> this[int i]
        {
            get { return _matrices[i]; }
        }
    }
}
