using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PMC.DataModels.DataModels;

namespace PMC.DataModels.Tests
{
    [TestClass]
    public class DataSetTest
    {
        private const int minNumber = 0;
        private const int maxNumber = 100;
        private static readonly Random randNum = new Random();

        [TestMethod]
        public void HighDataSetSizeTest()
        {
            const int pointArraySize = 500;

            var testPoints1D = new Point<int>[pointArraySize];
            var testPoints2D = new Point<int>[pointArraySize];
            var testPoints3D = new Point<int>[pointArraySize];

            for (var i = 0; i < pointArraySize; i++)
            {
                testPoints1D[i] = new Point<int>(RandomNumber());

                testPoints2D[i] = new Point<int>(RandomNumber(), RandomNumber());

                testPoints3D[i] = new Point<int>(RandomNumber(), RandomNumber(), RandomNumber());
            }

            const int positionArraySize = 500;

            var testPositions1D = new Position<int>[positionArraySize];
            var testPositions2D = new Position<int>[positionArraySize];
            var testPositions3D = new Position<int>[positionArraySize];

            for (var i = 0; i < positionArraySize; i++)
            {
                testPositions1D[i] = new Position<int>(testPoints1D, Dimension.D1);
                testPositions2D[i] = new Position<int>(testPoints2D, Dimension.D2);
                testPositions3D[i] = new Position<int>(testPoints3D, Dimension.D3);
            }

            const int matrixArraySize = 60;

            var testMatrices = new Matrix<int>[matrixArraySize];

            for (var i = 0; i < matrixArraySize; i++)
            {
                switch (i / 20)
                {
                    case 0:
                        testMatrices[i] = new Matrix<int>(testPositions1D);
                        break;
                    case 1:
                        testMatrices[i] = new Matrix<int>(testPositions2D);
                        break;
                    case 2:
                        testMatrices[i] = new Matrix<int>(testPositions3D);
                        break;
                }
            }

            const int containerArraySize = 1000;

            var testContainers = new Container<int>[containerArraySize];

            for (var i = 0; i < containerArraySize; i++)
            {
                testContainers[i] = new Container<int>(testMatrices);
            }

            ContainerCollectionBuilder<int> collectionsBuilder = new IntContainers();
            var containers = collectionsBuilder.Create();

            containers.ContainerArray = testContainers;

            for (var i = 0; i < 10; i++)
            {
                var containerIndex = randNum.Next(0, containerArraySize);
                var matrixIndex = randNum.Next(0, matrixArraySize);
                var positionIndex = randNum.Next(0, positionArraySize);
                var pointIndex = randNum.Next(0, pointArraySize);

                var point = containers[containerIndex][matrixIndex][positionIndex][pointIndex];
                
                Trace.WriteLine(point.ToString());
            }
        }

        private static int RandomNumber()
        {
            return randNum.Next(minNumber, maxNumber);
        }
    }
}
