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
        public DbSet<MessageTag> Tags { get; set; } = null!;

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
