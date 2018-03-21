using System;
using System.Data;
using ConsoleClientForPMC.DatabaseStorage.Models;
using Npgsql;
using PMC.DataModels.TestHelper;

namespace ConsoleClientForPMC.DatabaseStorage.Services.PointService
{
    public static class IntPointService
    {
        public static PointModel<int> Find(NpgsqlConnection connection, int id)
        {
            using (var cmd = new NpgsqlCommand($"SELECT * FROM Points WHERE Id = '{id}' LIMIT 1;", connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    return reader.Read() ? ReadModel(reader) : null;
                }
            }
        }

        public static int Create(NpgsqlConnection connection, int x, int? y = null, int? z = null)
        {
            var dimension = 1;
            var yValue = 0;

            if (y.HasValue)
            {
                dimension++;
                yValue = y.Value;
            }

            var zValue = 0;

            if (z.HasValue)
            {
                dimension++;
                zValue = z.Value;
            }

            int id;
            
            using (var command = new NpgsqlCommand("insert into points(dimension,datatype,x,y,z)" +
                                                 " values(:dimension,:datatype,:x,:y,:z) returning id;", connection))
            {
                var dimensionParameter = command.CreateParameter();
                dimensionParameter.Direction = ParameterDirection.Input;
                dimensionParameter.DbType = DbType.Int16;
                dimensionParameter.ParameterName = ":dimension";
                dimensionParameter.Value = dimension;
                command.Parameters.Add(dimensionParameter);

                var dataTypeParameter = command.CreateParameter();
                dataTypeParameter.Direction = ParameterDirection.Input;
                dataTypeParameter.DbType = DbType.Byte;
                dataTypeParameter.ParameterName = ":datatype";
                dataTypeParameter.Value = (byte)DataType.Int;
                command.Parameters.Add(dataTypeParameter);

                var xParameter = command.CreateParameter();
                xParameter.Direction = ParameterDirection.Input;
                xParameter.DbType = DbType.Binary;
                xParameter.ParameterName = ":x";
                xParameter.Value = BitConverter.GetBytes(x);
                command.Parameters.Add(xParameter);

                var yParameter = command.CreateParameter();
                yParameter.Direction = ParameterDirection.Input;
                yParameter.DbType = DbType.Binary;
                yParameter.ParameterName = ":y";
                yParameter.Value = BitConverter.GetBytes(yValue);
                command.Parameters.Add(yParameter);

                var zParameter = command.CreateParameter();
                zParameter.Direction = ParameterDirection.Input;
                zParameter.DbType = DbType.Binary;
                zParameter.ParameterName = ":z";
                zParameter.Value = BitConverter.GetBytes(zValue);
                command.Parameters.Add(zParameter);

                id = (int)command.ExecuteScalar();
            }

            return id;
        }
        
        private static PointModel<int> ReadModel(IDataRecord reader)
        {
            var id = reader.GetInt32(0);
            var dimension = reader.GetInt16(1);
            var dataType = reader.GetInt16(2);
            if (dataType != (byte) DataType.Int)
                throw new CustomException($"Wrong Data Type. Int expected, got {(DataType)dataType}.", (byte)CustomErrorCode.WrongDataType);

            var x = BitConverter.ToInt32((byte[]) reader.GetValue(3), 0);
            var y = BitConverter.ToInt32((byte[]) reader.GetValue(4), 0);
            var z = BitConverter.ToInt32((byte[]) reader.GetValue(5), 0);

            var errorLogModel = new PointModel<int>(id, (byte)dimension, x, y, z);
            return errorLogModel;
        }
    }
}
