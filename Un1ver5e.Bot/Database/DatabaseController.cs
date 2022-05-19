using Npgsql;
using System.Diagnostics;

namespace Un1ver5e.Bot.Database
{
    public class DatabaseController
    {
        private string connstr;

        public DatabaseController(string connstr)
        {
            this.connstr = connstr;
        }
        private NpgsqlConnection GetOpenedConnection()
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

    }
}
