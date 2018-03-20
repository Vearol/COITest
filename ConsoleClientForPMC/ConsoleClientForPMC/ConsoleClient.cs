using System;
using System.Collections.Generic;
using ConsoleClientForPMC.DatabaseStorage;
using ConsoleClientForPMC.DatabaseStorage.Services;
using ConsoleClientForPMC.DatabaseStorage.Services.PointService;
using Npgsql;
using PMC.DataModels.TestHelper;

namespace ConsoleClientForPMC
{
    static class ConsoleClient
    {
        private static Random random = new Random();
        private const int minNumber = 0;
        private const int maxNumber = 100;

        private const int pointArraySize = 500;
        private const int positionArraySize = 500;
        private const int matrixArraySize = 60;
        private const int containerArraySize = 1000;

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
            using (var newDbConnection = PMCConnection.Create())
            {
                newDbConnection.Open();
                PMCDatabaseInstantiator.Init(newDbConnection, "PMCDB");

                Console.WriteLine("Database init done.");

                //TestWriteToDB(newDbConnection);
                TestElementsFromDB(newDbConnection);

                newDbConnection.Close();
            }


            Console.ReadLine();
        }
        
        private static void TestWriteToDB(NpgsqlConnection connection)
        {
            var pointIds1D = new int[pointArraySize];
            var pointIds2D = new int[pointArraySize];
            var pointIds3D = new int[pointArraySize];

            Console.WriteLine("Creating Points");
            for (var i = 0; i < pointArraySize; i++)
            {
                pointIds1D[i] = IntPointService.Create(connection, RandomNumber());

                pointIds2D[i] = IntPointService.Create(connection, RandomNumber(), RandomNumber());

                pointIds3D[i] = IntPointService.Create(connection, RandomNumber(), RandomNumber(), RandomNumber());
            }

            var positionIds1D = new int[positionArraySize];
            var positionIds2D = new int[positionArraySize];
            var positionIds3D = new int[positionArraySize];

            Console.WriteLine("Creating Positions");
            for (var i = 0; i < positionArraySize; i++)
            {
                if (i != 0 && i % 100 == 0)
                    Console.WriteLine("Created {0} positions", i);

                var position1D = PositionService.CreateNewPosition(connection, (byte) DataType.Int);
                var position2D = PositionService.CreateNewPosition(connection, (byte) DataType.Int);
                var position3D = PositionService.CreateNewPosition(connection, (byte) DataType.Int);

                positionIds1D[i] = position1D;
                positionIds2D[i] = position2D;
                positionIds3D[i] = position3D;

                for (var j = 0; j < pointArraySize; j++)
                {
                    PositionService.AddPoint(connection, position1D, pointIds1D[j]);
                    PositionService.AddPoint(connection, position2D, pointIds2D[j]);
                    PositionService.AddPoint(connection, position3D, pointIds3D[j]);
                }
            }

            var matrixIds = new int[matrixArraySize];

            Console.WriteLine("Creating Matrices");
            for (var i = 0; i < matrixArraySize; i++)
            {
                if (i != 0 && i % 10 == 0)
                    Console.WriteLine("Created {0} matrices", i);

                var matrixId = MatrixService.CreateNewMatrix(connection, (byte) DataType.Int);
                matrixIds[i] = matrixId;

                for (var j = 0; j < positionArraySize; j++)
                {
                    switch (i / 20)
                    {
                        case 0:
                            MatrixService.AddPosition(connection, matrixId, positionIds1D[j]);
                            break;
                        case 1:
                            MatrixService.AddPosition(connection, matrixId, positionIds2D[j]);
                            break;
                        case 2:
                            MatrixService.AddPosition(connection, matrixId, positionIds3D[j]);
                            break;
                    }
                }
            }
            
            var containerIds = new int[containerArraySize];

            Console.WriteLine("Creating Containers");
            for (var i = 0; i < containerArraySize; i++)
            {
                if (i != 0 && i % 100 == 0)
                    Console.WriteLine("Created {0} containers", i);

                var containerId = ContainerService.CreateNewContainer(connection, (byte) DataType.Int);
                containerIds[i] = containerId;

                for (var j = 0; j < matrixArraySize; j++)
                {
                    ContainerService.AddMatrix(connection, containerId, matrixIds[j]);
                }
            }

            Console.WriteLine("Creating Container Collection.");
            var containerCollectionId = ContainerCollectionService.CreateNewContainerCollection(connection, (byte) DataType.Int);

            for (var i = 0; i < containerArraySize; i++)
            {
                if (i != 0 && i % 100 == 0)
                    Console.WriteLine("Added {0} containers to collection", i);

                ContainerCollectionService.AddContainer(connection, containerCollectionId, containerIds[i]);
            }

            Console.WriteLine("All items are created.");
        }

        private static int RandomNumber()
        {
            return random.Next(minNumber, maxNumber);
        }

        private static void TestElementsFromDB(NpgsqlConnection connection)
        {
            for (var i = 0; i < 10; i++)
            {
                var containerIndex = random.Next(0, containerArraySize);
                var matrixIndex = random.Next(0, matrixArraySize);
                var positionIndex = random.Next(0, positionArraySize);
                var pointIndex = random.Next(0, pointArraySize);

                var container = ContainerService.Find(connection, containerIndex);
                var matrix = MatrixService.Find(connection, matrixIndex);
                var position = PositionService.Find(connection, positionIndex);
                var point = IntPointService.Find(connection, pointIndex);

                if (container == null || matrix == null || position == null)
                    throw new CustomException("Can't find data in database.", (byte)CustomErrorCode.MissingDataInDatabase);

                Console.WriteLine(point.ToString());
            }
        }
    }
}

