using System.Data;
using ConsoleClientForPMC.DatabaseStorage.Models;
using Npgsql;

namespace ConsoleClientForPMC.DatabaseStorage.Services
{
    public class PositionService
    {
        public static PositionModel Find(NpgsqlConnection connection, int positionId)
        {
            using (var cmd = new NpgsqlCommand($"SELECT * FROM Positions WHERE Id = {positionId} LIMIT 1;", connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    return reader.Read() ? ReadTo(reader) : null;
                }
            }
        }

        public static int CreateNewPosition(NpgsqlConnection connection, byte dataType)
        {
            int id;

            using (var command = new NpgsqlCommand("insert into positions(datatype)" +
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

        public static void AddPoint(NpgsqlConnection connection, int positionId, int pointId)
        {
            using (var command = new NpgsqlCommand("insert into positionpoints(positionId, pointId)" +
                                                 " values(:positionId,:pointId)", connection))
            {
                var positionIdParameter = command.CreateParameter();
                positionIdParameter.Direction = ParameterDirection.Input;
                positionIdParameter.DbType = DbType.Int32;
                positionIdParameter.ParameterName = ":positionId";
                positionIdParameter.Value = positionId;
                command.Parameters.Add(positionIdParameter);

                var pointIdParameter = command.CreateParameter();
                pointIdParameter.Direction = ParameterDirection.Input;
                pointIdParameter.DbType = DbType.Int32;
                pointIdParameter.ParameterName = ":pointId";
                pointIdParameter.Value = pointId;
                command.Parameters.Add(pointIdParameter);
                
                command.ExecuteNonQuery();
            }
        }

        private static PositionModel ReadTo(IDataRecord reader)
        {
            var positionId = reader.GetInt32(0);
            var data = reader.GetInt16(1);

            var errorLogModel = new PositionModel(positionId, (byte)data);
            return errorLogModel;
        }
    }
}
