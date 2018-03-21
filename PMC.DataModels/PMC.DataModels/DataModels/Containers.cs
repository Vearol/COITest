using System.Linq;
using PMC.DataModels.TestHelper;

namespace PMC.DataModels.DataModels
{
    /// <summary>
    /// Containers contains array of Container, which should be the same type as the Containers itself
    /// </summary>
    /// <typeparam name="T">Type of Containers</typeparam>
    public class Containers<T>
    {
        private Container<T>[] _containers;

        /// <summary>
        /// Empty constructor for factory method
        /// </summary>
        public Containers() {}

        /// <param name="containers">Array of Container</param>
        public Containers(Container<T>[] containers)
        {
            Validate(containers);

            _containers = containers;
        }

        /// <returns>Array of Container in current Containers</returns>
        public Container<T>[] ContainerArray
        {
            get { return _containers; }
            set
            {
                Validate(value);

                _containers = value;
            }
        }

        /// <returns>Returns Container with given index</returns>
        public Container<T> this[int i]
        {
            get { return _containers[i]; }
        }

        /// <summary>
        /// Some of validations acording to the rules
        /// </summary>
        /// /// <param name="containers">Array of Container that need check for valid data</param>
        public static void Validate(Container<T>[] containers)
        {
            var firstContainer = containers.FirstOrDefault();
            if (firstContainer == null) return;

            var containerMatrixCount = firstContainer.Matrices.Length;
            var containerMatrixPositionCount = 0;

            foreach (var container in containers)
            {
                if (container.Matrices.Length != containerMatrixCount)
                    throw new CustomException("Number of matrices in all containers is not the same.", (byte)CustomErrorCode.IncorrectNumberOfMarices);

                if (container.Matrices.Length != 0 && containerMatrixPositionCount == 0)
                    containerMatrixPositionCount = container.Matrices[0].PositionCount;
            
                foreach (var containerMatrix in container.Matrices)
                {
                    if (containerMatrix.PositionCount != containerMatrixPositionCount)
                        throw new CustomException("Number of position in all matrices in all containers is not the same.", (byte)CustomErrorCode.IncorrectNumberOfPositions);
                }
            }

            for (var i = 0; i < containerMatrixCount; i++)
            {
                var currentMatrixInFirstContainer = containers[0].Matrices[i];

                var currentMatrixIs3D = currentMatrixInFirstContainer.Is3D;

                for (var j = 1; j < containers.Length; j++)
                {
                    if (containers[j].Matrices[i].Dimension != currentMatrixInFirstContainer.Dimension)
                        throw new CustomException("Types of all matrices with the same index is not the same.", (byte)CustomErrorCode.IncorrectTypesOfMatrices);

                    if (currentMatrixIs3D && !currentMatrixInFirstContainer.DataPointCounts.SequenceEqual(containers[j].Matrices[i].DataPointCounts))
                    {
                        throw new CustomException("Number of data points at each 3D position across equivalent matrix indexes is not the same.", (byte)CustomErrorCode.IncorrectNumberOf3DPoints);
                    }
                }
            }
        }
    }
}
