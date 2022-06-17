using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Un1ver5e.Bot.Services.Database.Entities;

namespace Un1ver5e.Bot.Services.Database
{

    public class ApplicationContext : DbContext
    {
        public DbSet<PomodoroData> PomodoroData { get; set; } = null!;
        public DbSet<TicTacToeData> TicTacToeData { get; set; } = null!;
        public DbSet<RpcData> RpcData { get; set; } = null!;
        public DbSet<Tag> Tags { get; set; } = null!;

        /// <summary>
        /// Attempts to get <see cref="Entities.PomodoroData"/> of the user with the specified ID. If it is not present in the table it creates a new <see cref="Entities.PomodoroData"/> with the specified ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PomodoroData GetPomodoro(ulong id)
        {
            return PomodoroData.Where(data => data.Id == id).FirstOrDefault() ?? new PomodoroData() { Id = id };
        }

        /// <summary>
        /// Attempts to get <see cref="Entities.TicTacToeData"/> of the user with the specified ID. If it is not present in the table it creates a new <see cref="Entities.TicTacToeData"/> with the specified ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TicTacToeData GetTicTacToe(ulong id)
        {
            return TicTacToeData.Where(data => data.Id == id).FirstOrDefault() ?? new TicTacToeData() { Id = id };
        }

        /// <summary>
        /// Attempts to get <see cref="Entities.RpcData"/> of the user with the specified ID. If it is not present in the table it creates a new <see cref="Entities.RpcData"/> with the specified ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RpcData GetRpc(ulong id)
        {
            return RpcData.Where(data => data.Id == id).FirstOrDefault() ?? new RpcData() { Id = id };
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
    }
}
