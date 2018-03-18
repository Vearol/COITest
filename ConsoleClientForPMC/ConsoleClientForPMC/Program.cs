using System;
using System.Collections.Generic;
using System.Linq;
using PMC.DataModels;
using PMC.DataModels.DataModels;
using PMC.DataModels.Points;

namespace ConsoleClientForPMC
{
    static class Program
    {
        private static Random random = new Random();

        private static void Shuffle<T>(this IList<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = random.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        static void Main(string[] args)
        {
            var simplePoints1D = new List<int> { 1,4,5,23,58,3,5,4,12,16,84,36,89,34,26,89,4,24,64,75 };

            var positionList = new List<Position<int, OneDimensionPoint<int>>>{Capacity = 10};
            for (var i = 0; i < 10; i++)
            {
                simplePoints1D.Shuffle();
                var points1D = simplePoints1D.Select(x => new OneDimensionPoint<int>(x)).ToList();

                var position1D = new Position<int, OneDimensionPoint<int>>(points1D);

                positionList.Add(position1D);
            }
            
            var matrix1D1 = new Matrix<int, OneDimensionPoint<int>>(positionList);
            var matrix1D2 = new Matrix<int, OneDimensionPoint<int>>(positionList);
            var matrix1D3 = new Matrix<int, OneDimensionPoint<int>>(positionList);
            var matrix1D4 = new Matrix<int, OneDimensionPoint<int>>(positionList);
            var matrix1D5 = new Matrix<int, OneDimensionPoint<int>>(positionList);

            var matrixList = new List<IMatrix<int>> {matrix1D1, matrix1D2, matrix1D3, matrix1D4, matrix1D5};

            var container = new Container<int>(matrixList);

            ContainerCollectionBuilder<int> builder = new IntContainers();
            var containerList = builder.Create();

            containerList.ContainerList = new List<Container<int>> {container};

            foreach (var insideContainer in containerList.ContainerList)
            {
                Console.WriteLine("Pathing container");
                foreach (var matrix in insideContainer.Matrices)
                {
                    Console.WriteLine("Pathing Matrix");

                    if (!(matrix is Matrix<int, OneDimensionPoint<int>> matrix1D))
                    {
                        Console.WriteLine("Couldn't cast. Break");
                        break;
                    }

                    foreach (var position in matrix1D.Positions)
                    {
                        Console.WriteLine("Pathing position");

                        foreach (var point in position.Points)
                        {
                            Console.WriteLine($"Point: X:{point.X}");
                        }
                    }
                }
            }

            Console.ReadLine();
        }
    }
}
