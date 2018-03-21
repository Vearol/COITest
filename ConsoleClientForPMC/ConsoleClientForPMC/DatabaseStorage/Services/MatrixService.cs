using System.Collections.Generic;
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
                    return reader.Read() ? ReadModel(reader) : null;
                }
            }
        }

        public static long Count(NpgsqlConnection connection)
        {
            using (var cmd = new NpgsqlCommand("SELECT COUNT(Id) FROM Matrices", connection))
            {
                return (long)cmd.ExecuteScalar();
            }
        }

        public static List<MatrixPositionModel> Query(NpgsqlConnection connection, int matrixId)
        {
            using (var cmd = new NpgsqlCommand($"SELECT * FROM MatrixPositions WHERE matrixId = {matrixId};", connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    var results = new List<MatrixPositionModel>();
                    while (reader.Read())
                    {
                        results.Add(ReadMatrixPosition(reader));
                    }
                    return results;
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

        private static MatrixModel ReadModel(IDataRecord reader)
        {
            var id = reader.GetInt32(0);
            var data = reader.GetInt16(1);

            var errorLogModel = new MatrixModel(id, (byte)data);
            return errorLogModel;
        }

        private static MatrixPositionModel ReadMatrixPosition(IDataRecord reader)
        {
            var matrixId = reader.GetInt32(0);
            var positionId = reader.GetInt32(1);

            var errorLogModel = new MatrixPositionModel(matrixId, positionId);
            return errorLogModel;
        }
    }
}
