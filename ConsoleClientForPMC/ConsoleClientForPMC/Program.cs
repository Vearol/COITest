using System;
using System.Collections.Generic;
using System.Linq;
using PMC.DataModels;
using PMC.DataModels.DataModels;

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
            var simplePoints1D = new int[] { 1,4,5,23,58,3,5,4,12,16,84,36,89,34,26,89,4,24,64,75 };

            var positionList = new List<Position<int>>{Capacity = 10};
            var positionList2D = new List<Position<int>> { Capacity = 10 };
            for (var i = 0; i < 10; i++)
            {
                simplePoints1D.Shuffle();
                var points1D = simplePoints1D.Select(x => new Point<int>(x)).ToArray();
                var points2D = simplePoints1D.Select(x => new Point<int>(x, x)).ToArray();

                var position1D = new Position<int>(points1D, Dimension.D1);
                var position2D = new Position<int>(points2D, Dimension.D2);

                positionList.Add(position1D);
                positionList2D.Add(position2D);
            }

            var positionArray = positionList.ToArray();
            var positionArray2D = positionList2D.ToArray();

            var matrix1D1 = new Matrix<int>(positionArray);
            var matrix1D2 = new Matrix<int>(positionArray);
            var matrix1D3 = new Matrix<int>(positionArray2D);
            var matrix1D4 = new Matrix<int>(positionArray2D);

            var matrixList = new Matrix<int>[] {matrix1D1, matrix1D2, matrix1D3, matrix1D4};

            var container = new Container<int>(matrixList);

            ContainerCollectionBuilder<int> builder = new IntContainers();
            var containerList = builder.Create();

            containerList.ContainerList = new Container<int>[] {container};

            foreach (var insideContainer in containerList.ContainerList)
            {
                Console.WriteLine("Pathing container");
                foreach (var matrix in insideContainer.Matrices)
                {
                    Console.WriteLine("Pathing Matrix");

                    foreach (var position in matrix.Positions)
                    {
                        Console.WriteLine("Pathing position");

                        foreach (var point in position.Points)
                        {
                            Console.WriteLine($"Point: {point.ToString()}");
                        }
                    }
                }
            }

            Console.ReadLine();
        }
    }
}
