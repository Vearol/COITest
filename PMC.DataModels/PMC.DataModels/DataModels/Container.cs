namespace PMC.DataModels.DataModels
{
    /// <summary>
    /// Container contains array of Matrix, which should be the same type as the Container itself
    /// </summary>
    /// <typeparam name="T">Type of Container and Matrices</typeparam>
    public class Container<T>
    {
        private readonly Matrix<T>[] _matrices;

        /// <param name="matrices">Array of Matrix</param>
        public Container(Matrix<T>[] matrices)
        {
            _matrices = matrices;
        }

        /// <returns>Array of Matrix in the current Container</returns>
        public Matrix<T>[] Matrices
        {
            get { return _matrices; }
        }

        /// <returns>Returns Matrix with given index</returns>
        public Matrix<T> this[int i]
        {
            get { return _matrices[i]; }
        }
    }
}
