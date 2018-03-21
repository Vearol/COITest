using Npgsql;

namespace ConsoleClientForPMC.DatabaseStorage.Services.PointService
{
    public static class PointService
    {
        public static long Count(NpgsqlConnection connection)
        {
            using (var cmd = new NpgsqlCommand("SELECT COUNT(Id) FROM Points", connection))
            {
                return (long)cmd.ExecuteScalar();
            }
        }
    }
}
