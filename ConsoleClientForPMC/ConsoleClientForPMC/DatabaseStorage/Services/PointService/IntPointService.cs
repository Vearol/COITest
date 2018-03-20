using System;
using System.Data;
using ConsoleClientForPMC.DatabaseStorage.Models.PointModels;
using Npgsql;

namespace ConsoleClientForPMC.DatabaseStorage.Services.PointService
{
    public class IntPointService
    {
        public static IntPointModel Find(NpgsqlConnection connection, int id)
        {
            using (var cmd = new NpgsqlCommand($"SELECT * FROM ErrorLogs WHERE Id = '{id}' LIMIT 1;", connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    return reader.Read() ? ReadTo(reader) : null;
                }
            }
        }

        public static void Create(NpgsqlConnection connection, byte dimension, int x, int? y = null, int? z = null)
        {
            using (var command = new NpgsqlCommand("insert into points(dimension,x,y,z)" +
                                                 " values(:dimension,:x,:y,:z)", connection))
            {
                var dimensionParameter = command.CreateParameter();
                dimensionParameter.Direction = ParameterDirection.Input;
                dimensionParameter.DbType = DbType.Int16;
                dimensionParameter.ParameterName = ":dimension";
                dimensionParameter.Value = dimension;
                command.Parameters.Add(dimensionParameter);

                var xParameter = command.CreateParameter();
                xParameter.Direction = ParameterDirection.Input;
                xParameter.DbType = DbType.Int32;
                xParameter.ParameterName = ":x";
                xParameter.Value = x;
                command.Parameters.Add(xParameter);

                if (y.HasValue)
                {
                    var yParameter = command.CreateParameter();
                    yParameter.Direction = ParameterDirection.Input;
                    yParameter.DbType = DbType.Int32;
                    yParameter.ParameterName = ":y";
                    yParameter.Value = x;
                    command.Parameters.Add(yParameter);
                }

                if (z.HasValue)
                {
                    var zParameter = command.CreateParameter();
                    zParameter.Direction = ParameterDirection.Input;
                    zParameter.DbType = DbType.Int32;
                    zParameter.ParameterName = ":z";
                    zParameter.Value = x;
                    command.Parameters.Add(zParameter);

                    command.ExecuteNonQuery();
                }
            }
        }

        private static IntPointModel ReadTo(IDataRecord reader)
        {
            var id = reader.GetInt32(0);
            var dimension = reader.GetByte(1);
            var x = reader.GetInt32(2);
            var y = reader.GetInt32(3);
            var z = reader.GetInt32(4);

            var errorLogModel = new IntPointModel(id, dimension, x, y, z);
            return errorLogModel;
        }
    }
}
