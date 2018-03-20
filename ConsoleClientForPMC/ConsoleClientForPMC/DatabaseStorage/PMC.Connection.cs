using Npgsql;

namespace ConsoleClientForPMC.DatabaseStorage
{
    public static class PMCConnection
    {
        public static NpgsqlConnection Create()
        {
            return new NpgsqlConnection("Host=localhost;Port=5433;Database=PMCDB;Username=postgres;Password=post123petro;");
        }
    }
}
