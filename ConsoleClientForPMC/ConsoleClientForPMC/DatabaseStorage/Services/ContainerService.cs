using System.Data;
using ConsoleClientForPMC.DatabaseStorage.Models;
using Npgsql;

namespace ConsoleClientForPMC.DatabaseStorage.Services
{
    public static class ContainerService
    {
        public static ContainerModel Find(NpgsqlConnection connection, int containerId )
        {
            using (var cmd = new NpgsqlCommand($"SELECT * FROM Containers WHERE Id = '{containerId}' LIMIT 1;", connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    return reader.Read() ? ReadTo(reader) : null;
                }
            }
        }

        public static int CreateNewContainer(NpgsqlConnection connection, byte dataType)
        {
            int id;

            using (var command = new NpgsqlCommand("insert into containers(datatype)" +
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

        public static void AddMatrix(NpgsqlConnection connection, int containerId, int matrixId)
        {
            using (var command = new NpgsqlCommand("insert into containermatrices(containerId, matrixId)" +
                                                 " values(:containerId, :matrixId);", connection))
            {
                var containerIdParameter = command.CreateParameter();
                containerIdParameter.Direction = ParameterDirection.Input;
                containerIdParameter.DbType = DbType.Int32;
                containerIdParameter.ParameterName = ":containerId";
                containerIdParameter.Value = containerId;
                command.Parameters.Add(containerIdParameter);

                var matrixIdParameter = command.CreateParameter();
                matrixIdParameter.Direction = ParameterDirection.Input;
                matrixIdParameter.DbType = DbType.Int32;
                matrixIdParameter.ParameterName = ":matrixId";
                matrixIdParameter.Value = matrixId;
                command.Parameters.Add(matrixIdParameter);

                command.ExecuteNonQuery();
            }
        }

        private static ContainerModel ReadTo(IDataRecord reader)
        {
            var id = reader.GetInt32(0);
            var data = reader.GetInt16(1);

            var errorLogModel = new ContainerModel(id, (byte)data);
            return errorLogModel;
        }
    }
}
