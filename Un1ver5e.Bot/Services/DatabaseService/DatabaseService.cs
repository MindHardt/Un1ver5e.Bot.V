using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Diagnostics;

namespace Un1ver5e.Bot.Services
{
    public class DatabaseService
    {
        public string Host { get; }
        public string Name { get; }
        private readonly string connectionString;

        public DatabaseService(IConfiguration config)
        {
            IConfigurationSection configSection = config.GetSection("database");

            Host = configSection["host"];
            Name = configSection["name"];
            string password = configSection["password"];
            string username = configSection["username"];

            //Npgsql connection string
            connectionString = $"Host={Host};Username={username};Password={password};Database={Name}";
        }

        /// <summary>
        /// Gets an opened <see cref="NpgsqlConnection"/> object that uses currect <see cref="connectionString"/>. This should be disposed.
        /// </summary>
        /// <returns></returns>
        public NpgsqlConnection GetOpenedConnection()
        {
            var con = new NpgsqlConnection(connectionString);
            con.Open();
            return con;
        }

        /// <summary>
        /// Gets approximate latency of a database query.
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
        /// Gets the size of the database, in bytes.
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
