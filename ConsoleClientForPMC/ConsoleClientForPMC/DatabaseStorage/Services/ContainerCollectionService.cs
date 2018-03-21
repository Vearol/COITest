using System.Collections.Generic;
using System.Data;
using ConsoleClientForPMC.DatabaseStorage.Models;
using Npgsql;

namespace ConsoleClientForPMC.DatabaseStorage.Services
{
    public class ContainerCollectionService
    {
        public static ContainerCollectionModel Find(NpgsqlConnection connection, int containerCollectionId)
        {
            using (var cmd = new NpgsqlCommand($"SELECT * FROM ContainerCollections WHERE Id = '{containerCollectionId}' LIMIT 1;", connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    return reader.Read() ? ReadModel(reader) : null;
                }
            }
        }

        public static int CreateNewContainerCollection(NpgsqlConnection connection, byte dataType)
        {
            int id;

            using (var command = new NpgsqlCommand("insert into containercollections(datatype)" +
                                                   " values(:datatype) returning id;", connection))
            {
                var dataTypeParameter = command.CreateParameter();
                dataTypeParameter.Direction = ParameterDirection.Input;
                dataTypeParameter.DbType = DbType.Byte;
                dataTypeParameter.ParameterName = ":dataType";
                dataTypeParameter.Value = dataType;
                command.Parameters.Add(dataTypeParameter);

                id = (int)command.ExecuteScalar();
            }

            return id;
        }

        public static List<ContainerCollectionContainerModel> Query(NpgsqlConnection connection, int containerCollectionId)
        {
            using (var cmd = new NpgsqlCommand($"SELECT * FROM ContainerCollectionContainers WHERE containerCollectionId = {containerCollectionId};", connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    var results = new List<ContainerCollectionContainerModel>();
                    while (reader.Read())
                    {
                        results.Add(ReadContainerCollectionContainer(reader));
                    }
                    return results;
                }
            }
        }

        public static void AddContainer(NpgsqlConnection connection, int containercollectionId, int containerId)
        {
            using (var command = new NpgsqlCommand("insert into ContainerCollectionContainers(containercollectionId, containerId)" +
                                                 " values(:containercollectionId, :containerId);", connection))
            {
                var containercollectionIdParameter = command.CreateParameter();
                containercollectionIdParameter.Direction = ParameterDirection.Input;
                containercollectionIdParameter.DbType = DbType.Int32;
                containercollectionIdParameter.ParameterName = ":containercollectionId";
                containercollectionIdParameter.Value = containercollectionId;
                command.Parameters.Add(containercollectionIdParameter);

                var containerIdParameter = command.CreateParameter();
                containerIdParameter.Direction = ParameterDirection.Input;
                containerIdParameter.DbType = DbType.Int32;
                containerIdParameter.ParameterName = ":containerId";
                containerIdParameter.Value = containerId;
                command.Parameters.Add(containerIdParameter);

                command.ExecuteNonQuery();
            }
        }

        private static ContainerCollectionModel ReadModel(IDataRecord reader)
        {
            var id = reader.GetInt32(0);
            var data = reader.GetInt16(1);

            var errorLogModel = new ContainerCollectionModel(id, (byte)data);
            return errorLogModel;
        }

        private static ContainerCollectionContainerModel ReadContainerCollectionContainer(IDataRecord reader)
        {
            var containerCollectionId = reader.GetInt32(0);
            var containerId = reader.GetInt32(1);

            var errorLogModel = new ContainerCollectionContainerModel(containerCollectionId, containerId);
            return errorLogModel;
        }
    }
}
