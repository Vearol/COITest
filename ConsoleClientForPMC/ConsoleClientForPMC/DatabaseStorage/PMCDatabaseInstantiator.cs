using System;
using Npgsql;

namespace ConsoleClientForPMC.DatabaseStorage
{
    public static class PMCDatabaseInstantiator
    {
        public static void Init(NpgsqlConnection connection, string dbName)
        {
            var sqlCreateDbQuery = $"CREATE DATABASE \"{dbName}\" " +
                                   "WITH OWNER = \"postgres\" " +
                                   "TEMPLATE = \"template0\" " +
                                   "ENCODING = 'UTF8' " +
                                   "CONNECTION LIMIT = -1;";

            if (!CheckDatabaseExists(connection, dbName))
            {
                Console.WriteLine("Database doesn't exist");

                using (var cmd = new NpgsqlCommand(sqlCreateDbQuery, connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }

            if (!CheckTableExists(connection, "points"))
                CreatePointsTable(connection);

            if (!CheckTableExists(connection, "positions"))
                CreatePositionsTable(connection);

            if (!CheckTableExists(connection, "positionpoints"))
                CreatePositionPointsTable(connection);

            if (!CheckTableExists(connection, "matrices"))
                CreateMatricesTable(connection);

            if (!CheckTableExists(connection, "matrixpositions"))
                CreateMatrixPositionsTable(connection);

            if (!CheckTableExists(connection, "containers"))
                CreateContainersTable(connection);

            if (!CheckTableExists(connection, "containermatrices"))
                CreateContainerMatricesTable(connection);

            if (!CheckTableExists(connection, "containercollections"))
                CreateContainerCollectionsTable(connection);

            if (!CheckTableExists(connection, "containercollectioncontainers"))
                CreateContainerCollectionContainersTable(connection);

        }

        private static void CreatePointsTable(NpgsqlConnection connection)
        {
            Console.WriteLine("Creating Points table");
            const string sqlCreateTableQuery = "CREATE TABLE Points (" +
                                               "Id SERIAL," +
                                               "Dimension SMALLINT," +
                                               "DataType SMALLINT," +
                                               "X BYTEA," +
                                               "Y BYTEA," +
                                               "Z BYTEA," +
                                               "PRIMARY KEY(Id) );";

            using (var cmd = new NpgsqlCommand(sqlCreateTableQuery, connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private static void CreatePositionsTable(NpgsqlConnection connection)
        {
            Console.WriteLine("Creating Positions table");
            const string sqlCreateTableQuery = "CREATE TABLE Positions (" +
                                               "Id SERIAL, " +
                                               "DataType SMALLINT, " +
                                               "PRIMARY KEY(Id));";

            using (var cmd = new NpgsqlCommand(sqlCreateTableQuery, connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private static void CreatePositionPointsTable(NpgsqlConnection connection)
        {
            Console.WriteLine("Creating PositionPoints table");
            const string sqlCreateTableQuery = "CREATE TABLE PositionPoints (" +
                                               "PositionId SERIAL, " +
                                               "PointId INT, " +
                                               "PRIMARY KEY(PositionId, PointId));";

            using (var cmd = new NpgsqlCommand(sqlCreateTableQuery, connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private static void CreateMatricesTable(NpgsqlConnection connection)
        {
            Console.WriteLine("Creating Matrices table");
            const string sqlCreateTableQuery = "CREATE TABLE Matrices (" +
                                               "Id SERIAL," +
                                               "DataType SMALLINT," +
                                               "PRIMARY KEY(Id));";

            using (var cmd = new NpgsqlCommand(sqlCreateTableQuery, connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private static void CreateMatrixPositionsTable(NpgsqlConnection connection)
        {
            Console.WriteLine("Creating MatrixPositions table");
            const string sqlCreateTableQuery = "CREATE TABLE MatrixPositions (" +
                                               "MatrixId SERIAL," +
                                               "PositionId INT," +
                                               "PRIMARY KEY(MatrixId, PositionId));";

            using (var cmd = new NpgsqlCommand(sqlCreateTableQuery, connection))
            {
                cmd.ExecuteNonQuery();
            }
        }
        private static void CreateContainersTable(NpgsqlConnection connection)
        {
            Console.WriteLine("Creating Containers table");
            const string sqlCreateTableQuery = "CREATE TABLE Containers (" +
                                               "Id SERIAL," +
                                               "DataType SMALLINT," +
                                               "PRIMARY KEY(Id));";

            using (var cmd = new NpgsqlCommand(sqlCreateTableQuery, connection))
            {
                cmd.ExecuteNonQuery();
            }
        }
        private static void CreateContainerMatricesTable(NpgsqlConnection connection)
        {
            Console.WriteLine("Creating ContainerMatrices table");
            const string sqlCreateTableQuery = "CREATE TABLE ContainerMatrices (" +
                                               "ContainerId SERIAL," +
                                               "MatrixId INT," +
                                               "PRIMARY KEY(ContainerId, MatrixId));";

            using (var cmd = new NpgsqlCommand(sqlCreateTableQuery, connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private static void CreateContainerCollectionsTable(NpgsqlConnection connection)
        {
            Console.WriteLine("Creating ContainerCollections table");
            const string sqlCreateTableQuery = "CREATE TABLE ContainerCollections (" +
                                               "Id SERIAL," +
                                               "DataType SMALLINT," +
                                               "PRIMARY KEY(Id));";

            using (var cmd = new NpgsqlCommand(sqlCreateTableQuery, connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private static void CreateContainerCollectionContainersTable(NpgsqlConnection connection)
        {
            Console.WriteLine("Creating ContainerCollectionContainers table");
            const string sqlCreateTableQuery = "CREATE TABLE ContainerCollectionContainers (" +
                                               "ContainerCollectionId SERIAL," +
                                               "ContainerId INT," +
                                               "PRIMARY KEY(ContainerCollectionId, ContainerId));";

            using (var cmd = new NpgsqlCommand(sqlCreateTableQuery, connection))
            {
                cmd.ExecuteNonQuery();
            }
        }


        private static bool CheckDatabaseExists(NpgsqlConnection connection, string dbName)
        {
            bool result;
            try
            {
                using (var cmd = new NpgsqlCommand($"SELECT datdba FROM pg_database where datname ='{dbName}';", connection))
                {
                    var resultObj = cmd.ExecuteScalar();
                    var databaseId = 0;
                    if (resultObj != null)
                    {
                        int.TryParse(resultObj.ToString(), out databaseId);
                    }

                    result = (databaseId > 0);
                }
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }

        private static bool CheckTableExists(NpgsqlConnection connection, string name)
        {
            var exists = false;
            var sqlPointsTableExistsQuery = $"SELECT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_schema = 'public' AND table_name = '{name}');";

            using (var cmd = new NpgsqlCommand(sqlPointsTableExistsQuery, connection))
            {
                var resultObj = cmd.ExecuteScalar();
                if (resultObj != null)
                {
                    bool.TryParse(resultObj.ToString(), out exists);
                }
            }
            return exists;
        }
    }
}
