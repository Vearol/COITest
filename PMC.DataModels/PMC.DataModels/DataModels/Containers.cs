using System;
using System.Collections.Generic;
using System.Linq;
using PMC.DataModels.TestHelper;

namespace PMC.DataModels.DataModels
{
    public class Containers<T>
    {
        private Container<T>[] _containers;

        public Containers() {}

        public Containers(Container<T>[] containers)
        {
            _containers = containers;
        }

        public Container<T>[] ContainerList
        {
            get { return _containers; }
            set
            {
                Validate(value);

                _containers = value;
            }
        }

        public static void Validate(Container<T>[] containers)
        {
            if (containers.Length == 0) return;

            var containerMatrixCount = containers[0].Matrices.Length;
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
