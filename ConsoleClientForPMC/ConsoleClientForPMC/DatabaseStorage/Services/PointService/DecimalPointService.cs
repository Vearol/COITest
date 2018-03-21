using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ConsoleClientForPMC.DatabaseStorage.Models;
using Npgsql;
using PMC.DataModels.TestHelper;

namespace ConsoleClientForPMC.DatabaseStorage.Services.PointService
{
    public static class BitConverterExt
    {
        public static byte[] GetBytes(decimal dec)
        {
            var bits = decimal.GetBits(dec);
            var bytes = new List<byte>();
            foreach (var i in bits)
            {
                bytes.AddRange(BitConverter.GetBytes(i));
            }

            return bytes.ToArray();
        }

        public static decimal ToDecimal(byte[] bytes)
        {
            if (bytes.Count() != 16)
                throw new Exception("A decimal must be created from exactly 16 bytes");

            var bits = new int[4];
            for (var i = 0; i <= 15; i += 4)
            {
                bits[i / 4] = BitConverter.ToInt32(bytes, i);
            }

            return new decimal(bits);
        }
    }

    public class DecimalPointService
    {
        public static PointModel<decimal> Find(NpgsqlConnection connection, int id)
        {
            using (var cmd = new NpgsqlCommand($"SELECT * FROM Points WHERE Id = '{id}' LIMIT 1;", connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    return reader.Read() ? ReadModel(reader) : null;
                }
            }
        }

        public static int Create(NpgsqlConnection connection, decimal x, decimal? y = null, decimal? z = null)
        {
            var dimension = 1;
            decimal yValue = 0;

            if (y.HasValue)
            {
                dimension++;
                yValue = y.Value;
            }

            decimal zValue = 0;

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
                dataTypeParameter.Value = (byte)DataType.Decimal;
                command.Parameters.Add(dataTypeParameter);

                var xParameter = command.CreateParameter();
                xParameter.Direction = ParameterDirection.Input;
                xParameter.DbType = DbType.Decimal;
                xParameter.ParameterName = ":x";
                xParameter.Value = BitConverterExt.GetBytes(x);
                command.Parameters.Add(xParameter);

                var yParameter = command.CreateParameter();
                yParameter.Direction = ParameterDirection.Input;
                yParameter.DbType = DbType.Decimal;
                yParameter.ParameterName = ":y";
                yParameter.Value = BitConverterExt.GetBytes(yValue);
                command.Parameters.Add(yParameter);

                var zParameter = command.CreateParameter();
                zParameter.Direction = ParameterDirection.Input;
                zParameter.DbType = DbType.Decimal;
                zParameter.ParameterName = ":z";
                zParameter.Value = BitConverterExt.GetBytes(zValue);
                command.Parameters.Add(zParameter);

                id = (int)command.ExecuteScalar();
            }

            return id;
        }

        private static PointModel<decimal> ReadModel(IDataRecord reader)
        {
            var id = reader.GetInt32(0);
            var dimension = reader.GetInt16(1);
            var dataType = reader.GetInt16(2);
            if (dataType != (byte)DataType.Double)
                throw new CustomException($"Wrong Data Type. Decimal expected, got {(DataType)dataType}.", (byte)CustomErrorCode.WrongDataType);
            
            var x = BitConverterExt.ToDecimal((byte[])reader.GetValue(3));
            var y = BitConverterExt.ToDecimal((byte[])reader.GetValue(4));
            var z = BitConverterExt.ToDecimal((byte[])reader.GetValue(5));

            var errorLogModel = new PointModel<decimal>(id, (byte)dimension, x, y, z);

            return errorLogModel;
        }
    }
}
