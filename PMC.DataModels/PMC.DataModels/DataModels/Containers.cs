﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace PMC.DataModels.DataModels
{
    public class Containers<T>
    {
        private List<Container<T>> _containers;

        public Containers() {}

        public Containers(List<Container<T>> containers)
        {
            _containers = containers;
        }

        public List<Container<T>> ContainerList
        {
            get { return _containers; }
            set
            {
                Validate(value);

                _containers = value;
            }
        }

        private void Validate(List<Container<T>> containers)
        {
            if (containers.Count == 0) return;

            var containerMatrixCount = containers[0].Matrices.Count;
            var containerMatrixPositionCount = 0;

            foreach (var container in containers)
            {
                if (container.Matrices.Count != containerMatrixCount)
                    throw new Exception("Number of matrices in all containers is not the same.");

                if (container.Matrices.Count != 0 && containerMatrixPositionCount == 0)
                    containerMatrixPositionCount = container.Matrices[0].PositionCount;
            
                foreach (var containerMatrix in container.Matrices)
                {
                    if (containerMatrix.PositionCount != containerMatrixPositionCount)
                        throw new Exception("Number of position in all matrices in all containers is not the same.");
                }
            }

            for (var i = 0; i < containerMatrixCount; i++)
            {
                var currentMatrixInFirstContainer = containers[0].Matrices[i];

                var currentMatrixIs3D = currentMatrixInFirstContainer.Is3D;

                for (var j = 1; j < containers.Count; j++)
                {
                    if (containers[j].Matrices[i].DimensionType != currentMatrixInFirstContainer.DimensionType)
                        throw new Exception("Types of all matrices with the same index is not the same.");

                    if (currentMatrixIs3D && !currentMatrixInFirstContainer.DataPointCounts.SequenceEqual(containers[j].Matrices[i].DataPointCounts))
                    {
                        throw new Exception("Number of data points at each 3D position across equivalent matrix indexes is not the same.");
                    }
                }
            }
        }
    }
}