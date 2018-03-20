using System;
using Npgsql;

namespace ConsoleClientForPMC.DatabaseStorage
{
    public static class PMCDatabaseInstantiator
    {
        public static void Init(NpgsqlConnection connection, string dbName)
        {
            connection.Open();
            using (connection)
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
                        connection.Close();
                    }
                }
            }

            using (var newDbConnection = PMCConnection.Create())
            {
                newDbConnection.Open();
                if (!CheckTableExists(newDbConnection, "points"))
                    CreatePointsTable(newDbConnection);
                
                if (!CheckTableExists(newDbConnection, "positions"))
                    CreatePositionsTable(newDbConnection);

                if (!CheckTableExists(newDbConnection, "matrices"))
                    CreateMatricesTable(newDbConnection);

                if (!CheckTableExists(newDbConnection, "containers"))
                    CreateContainersTable(newDbConnection);

                if (!CheckTableExists(newDbConnection, "containercollections"))
                    CreateContainerCollectionsTable(newDbConnection);

                newDbConnection.Close();
            }
        }

        private static void CreatePointsTable(NpgsqlConnection connection)
        {
            Console.WriteLine("Creating Points table");
            const string sqlCreateTableQuery = "CREATE TABLE Points (" +
                                               "Id SERIAL," +
                                               "Dimension SMALLINT," +
                                               "X INT," +
                                               "Y INT," +
                                               "Z INT," +
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
                                               "PointId INT, " +
                                               "PRIMARY KEY(Id, PointId));";

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
                                               "PositionId INT," +
                                               "PRIMARY KEY(Id, PositionId));";

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
                                               "MatrixId INT," +
                                               "PRIMARY KEY(Id, MatrixId));";

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
                                               "ContainerId INT," +
                                               "PRIMARY KEY(Id, ContainerId));";

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
