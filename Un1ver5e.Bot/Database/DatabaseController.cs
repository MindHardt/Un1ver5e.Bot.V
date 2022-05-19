using Npgsql;

namespace Un1ver5e.Bot.Database
{
    public class DatabaseController
    {
        private str
        private NpgsqlConnection GetOpenedConnection()
        {
            var con = new NpgsqlConnection(connstr);
            con.Open();
            return con;
        }



    }
}
