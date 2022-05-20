using Npgsql;
using System.Diagnostics;

namespace Un1ver5e.Bot.Database
{
    public class DatabaseController
    {
        public string Host { get; }
        public string Name { get; }
        private readonly string connstr;

        public DatabaseController(string dbHost, string dbUsername, string dbPassword, string dbName)
        {
            Host = dbHost;
            Name = dbName;
            connstr = $"Host={dbHost};Username={dbUsername};Password={dbPassword};Database={dbName}";
        }

        public NpgsqlConnection GetOpenedConnection()
        {
            var con = new NpgsqlConnection(connstr);
            con.Open();
            return con;
        }

        /// <summary>
        /// Gets approximate ping of a DB query.
        /// </summary>
        /// <returns></returns>
        public async ValueTask<TimeSpan> GetPing()
        {
            Stopwatch sw = new();

            using (var conn = GetOpenedConnection())
            {
                NpgsqlCommand command = new()
                {
                    Connection = conn,
                    CommandText = "Select 1"
                };

                sw.Start();
                _ = await command.ExecuteNonQueryAsync();
                sw.Stop();
            }

            return sw.Elapsed;
        }
        /// <summary>
        /// Gets the size of the DB.
        /// </summary>
        /// <returns></returns>
        public async ValueTask<long> GetSize()
        {
            using var conn = GetOpenedConnection();
            NpgsqlCommand command = new()
            {
                Connection = conn,
                CommandText = $"select pg_database_size('{Name}')"
            };

            return (long)(await command.ExecuteScalarAsync())!;
        }

    }
}
