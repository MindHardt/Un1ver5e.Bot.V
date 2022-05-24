using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Diagnostics;
using Un1ver5e.Bot.Services.Database;

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
            string maxPool = configSection["max_pool_size"];

            //Npgsql connection string
            connectionString = $"Host={Host};Username={username};Password={password};Database={Name};Maximum Pool Size={maxPool}";
        }

        /// <summary>
        /// Gets a <see cref="NpgsqlConnection"/> object that uses currect <see cref="connectionString"/>. This should be disposed.
        /// </summary>
        /// <returns></returns>
        public NpgsqlConnection GetConnection() => new NpgsqlConnection(connectionString);

        /// <summary>
        /// Gets approximate latency of a database query.
        /// </summary>
        /// <returns></returns>
        public async ValueTask<TimeSpan> GetPing()
        {
            Stopwatch sw = new();

            using (var conn = GetConnection())
            {
                await conn.OpenAsync();
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
            using var conn = GetConnection();
            await conn.OpenAsync();
            NpgsqlCommand command = new()
            {
                Connection = conn,
                CommandText = $"select pg_database_size('{Name}')"
            };

            return (long)(await command.ExecuteScalarAsync())!;
        }

        public async ValueTask Pull(IDatabaseEntity entity)
        {
            await entity.Pull(this);
        }

        public async ValueTask Push(IDatabaseEntity entity)
        {
            await entity.Push(this);
        }

    }
}
