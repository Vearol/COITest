using System;
using ConsoleClientForPMC.DatabaseStorage;
using ConsoleClientForPMC.DatabaseStorage.Services;
using ConsoleClientForPMC.DatabaseStorage.Services.PointService;
using Npgsql;
using PMC.DataModels;
using PMC.DataModels.DataModels;
using PMC.DataModels.TestHelper;

namespace ConsoleClientForPMC
{
    static class ConsoleClient
    {
        private static Random random = new Random();
        private const int minNumber = 0;
        private const int maxNumber = 100;

        private const int pointArraySize = 500;
        private const int positionArraySize = 170;
        private const int matrixArraySize = 60;
        private const int containerArraySize = 1000;

        private const int validCollectionContainerId = 1;

        static void Main(string[] args)
        {
            using (var connection = PMCConnection.Create())
            {
                connection.Open();
                PMCDatabaseInstantiator.Init(connection, "PMCDB");

                Console.WriteLine("Database init done.");

                if (ContainerService.Count(connection) < containerArraySize ||
                    MatrixService.Count(connection) < matrixArraySize ||
                    PositionService.Count(connection) < positionArraySize ||
                    PointService.Count(connection) < pointArraySize)
                {
                    WriteTestDataToDB(connection);
                }

                TestElementsFromDB(connection);

                var containers = LoadDataFromDbToModel(connection);

                TestElementsFromPMCModel(containers);

                Console.WriteLine("End");
                connection.Close();
            }


            Console.ReadLine();
        }

        private static Containers<int> LoadDataFromDbToModel(NpgsqlConnection connection)
        {
            Console.WriteLine("Loading data to Containers PMC Model");

            var containerCollection = ContainerCollectionService.Find(connection, validCollectionContainerId);
            var dataType = containerCollection.DataType;

            var containerCollectionData = ContainerCollectionService.Query(connection, containerCollection.Id);

            var pmsContainers = new Container<int>[containerCollectionData.Count];
            
            for (var i = 0; i < containerCollectionData.Count; i++)
            {
                if (i % 100 == 0 && i != 0)
                    Console.WriteLine("Loaded {0} containers", i);

                var containerModel = ContainerService.Find(connection, containerCollectionData[i].ContainerId);

                if (containerModel.DataType != dataType)
                    throw new CustomException("Containers has incorrect data type", (byte)CustomErrorCode.IncorrectDataTypeInContainers);

                var containerMatrices = ContainerService.Query(connection, containerModel.Id);

                var pmsMatrices = new Matrix<int>[containerMatrices.Count];
                
                for (var j = 0; j < containerMatrices.Count; j++)
                {
                    if (j % 10 == 0 && j != 0)
                        Console.WriteLine("Loaded {0} matrices", j);
                    
                    var matrixModel = MatrixService.Find(connection, containerMatrices[j].MatrixId);
                    if (matrixModel.DataType != dataType)
                        throw new CustomException("Containers has incorrect data type", (byte)CustomErrorCode.IncorrectDataTypeInContainers);

                    var matrixPositions = MatrixService.Query(connection, matrixModel.Id);

                    var pmsPositions = new Position<int>[matrixPositions.Count];
                    
                    for (var k = 0; k < matrixPositions.Count; k++)
                    {
                        var positionModel = PositionService.Find(connection, matrixPositions[k].PositionId);
                        if (positionModel.DataType != dataType)
                            throw new CustomException("Containers has incorrect data type", (byte)CustomErrorCode.IncorrectDataTypeInContainers);

                        var positionPoints = PositionService.Query(connection, positionModel.Id);

                        var pmsPoints = new Point<int>[positionPoints.Count];

                        var dimension = Dimension.D1;
                        for (var s = 0; s < positionPoints.Count; s++)
                        {
                            var pointModel = IntPointService.Find(connection, positionPoints[s].PointId);
                            if (pointModel == null)
                                continue;
                            var pointModelDimension = (Dimension)pointModel.Dimension;
                            if (dimension != pointModelDimension)
                                dimension = pointModelDimension;

                            pmsPoints[s] = new Point<int>(pointModel.GetPointArray());
                        }

                        pmsPositions[k] = new Position<int>(pmsPoints, dimension);
                    }

                    pmsMatrices[j] = new Matrix<int>(pmsPositions);
                }

                pmsContainers[i] = new Container<int>(pmsMatrices);
            }

            ContainerCollectionBuilder<int> builder = new IntContainers();
            var containers = builder.Create();

            containers.ContainerArray = pmsContainers;
            return containers;
        }

        private static void WriteTestDataToDB(NpgsqlConnection connection)
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
            Console.WriteLine("Testing random data from database");

            for (var i = 0; i < 10; i++)
            {
                var containerIndex = random.Next(1, containerArraySize);
                var matrixIndex = random.Next(1, matrixArraySize);
                var positionIndex = random.Next(1, positionArraySize);
                var pointIndex = random.Next(1, pointArraySize);

                var container = ContainerService.Find(connection, containerIndex);
                var matrix = MatrixService.Find(connection, matrixIndex);
                var position = PositionService.Find(connection, positionIndex);
                var point = IntPointService.Find(connection, pointIndex);

                if (container == null || matrix == null || position == null)
                    throw new CustomException("Can't find data in database.", (byte)CustomErrorCode.MissingDataInDatabase);

                Console.WriteLine(point.ToString());
            }
        }

        private static void TestElementsFromPMCModel<T>(Containers<T> containers)
        {
            for (var i = 0; i < 10; i++)
            {
                var containerIndex = random.Next(0, containerArraySize);
                var matrixIndex = random.Next(0, matrixArraySize);
                var positionIndex = random.Next(0, positionArraySize);
                var pointIndex = random.Next(0, pointArraySize);

                var point = containers[containerIndex][matrixIndex][positionIndex][pointIndex];

                Console.WriteLine(point.ToString());
            }
        }
    }
}

