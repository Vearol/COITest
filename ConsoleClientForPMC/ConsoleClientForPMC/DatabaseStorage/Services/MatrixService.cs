using System.Data;
using ConsoleClientForPMC.DatabaseStorage.Models;
using Npgsql;

namespace ConsoleClientForPMC.DatabaseStorage.Services
{
    public class MatrixService
    {
        public static MatrixModel Find(NpgsqlConnection connection, int matrixId)
        {
            using (var cmd = new NpgsqlCommand($"SELECT * FROM Matrices WHERE Id = '{matrixId}' LIMIT 1;", connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    return reader.Read() ? ReadTo(reader) : null;
                }
            }
        }

        public static int CreateNewMatrix(NpgsqlConnection connection, byte dataType)
        {
            int id;

            using (var command = new NpgsqlCommand("insert into matrices(datatype)" +
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

        public static void AddPosition(NpgsqlConnection connection, int matrixId, int positionId)
        {
            using (var command = new NpgsqlCommand("insert into matrixpositions(matrixId, positionId)" +
                                                 " values(:matrixId, :positionId);", connection))
            {
                var matrixIdParameter = command.CreateParameter();
                matrixIdParameter.Direction = ParameterDirection.Input;
                matrixIdParameter.DbType = DbType.Int32;
                matrixIdParameter.ParameterName = ":matrixId";
                matrixIdParameter.Value = matrixId;
                command.Parameters.Add(matrixIdParameter);

                var positionIdParameter = command.CreateParameter();
                positionIdParameter.Direction = ParameterDirection.Input;
                positionIdParameter.DbType = DbType.Int32;
                positionIdParameter.ParameterName = ":positionId";
                positionIdParameter.Value = positionId;
                command.Parameters.Add(positionIdParameter);

                command.ExecuteNonQuery();
            }
        }

        private static MatrixModel ReadTo(IDataRecord reader)
        {
            var id = reader.GetInt32(0);
            var data = reader.GetInt16(1);

            var errorLogModel = new MatrixModel(id, (byte)data);
            return errorLogModel;
        }
    }
}
