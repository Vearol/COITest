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

        private const int pointArraySize = 500;
        private const int positionArraySize = 500;
        private const int matrixArraySize = 60;
        private const int containerArraySize = 1000;

        [TestMethod]
        public void HighDataSetSizeTest()
        {
            GenerateTestPoints(out var testPoints1D, out var testPoints2D, out var testPoints3D);

            GenerateTestPositions(testPoints1D, testPoints2D, testPoints3D, out var testPositions1D, out var testPositions2D, out var testPositions3D);

            var testMatrices = GenerateTestMatrices(testPositions1D, testPositions2D, testPositions3D);

            var testContainers = GenerateTestContainers(testMatrices);

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

        private static Container<int>[] GenerateTestContainers(Matrix<int>[] testMatrices)
        {
            var testContainers = new Container<int>[containerArraySize];

            for (var i = 0; i < containerArraySize; i++)
            {
                testContainers[i] = new Container<int>(testMatrices);
            }

            return testContainers;
        }

        private static Matrix<int>[] GenerateTestMatrices(Position<int>[] testPositions1D, Position<int>[] testPositions2D, Position<int>[] testPositions3D)
        {
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

            return testMatrices;
        }

        private static void GenerateTestPositions(Point<int>[] testPoints1D, Point<int>[] testPoints2D, Point<int>[] testPoints3D, out Position<int>[] testPositions1D, out Position<int>[] testPositions2D, out Position<int>[] testPositions3D)
        {
            testPositions1D = new Position<int>[positionArraySize];
            testPositions2D = new Position<int>[positionArraySize];
            testPositions3D = new Position<int>[positionArraySize];
            for (var i = 0; i < positionArraySize; i++)
            {
                testPositions1D[i] = new Position<int>(testPoints1D, Dimension.D1);
                testPositions2D[i] = new Position<int>(testPoints2D, Dimension.D2);
                testPositions3D[i] = new Position<int>(testPoints3D, Dimension.D3);
            }
        }

        private static void GenerateTestPoints(out Point<int>[] testPoints1D, out Point<int>[] testPoints2D, out Point<int>[] testPoints3D)
        {
            testPoints1D = new Point<int>[pointArraySize];
            testPoints2D = new Point<int>[pointArraySize];
            testPoints3D = new Point<int>[pointArraySize];
            for (var i = 0; i < pointArraySize; i++)
            {
                testPoints1D[i] = new Point<int>(RandomNumber());

                testPoints2D[i] = new Point<int>(RandomNumber(), RandomNumber());

                testPoints3D[i] = new Point<int>(RandomNumber(), RandomNumber(), RandomNumber());
            }
        }

        private static int RandomNumber()
        {
            return randNum.Next(minNumber, maxNumber);
        }
    }
}
