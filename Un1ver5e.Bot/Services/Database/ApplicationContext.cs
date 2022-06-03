using Microsoft.EntityFrameworkCore;
using Un1ver5e.Bot.Services.Database.Entities;

namespace Un1ver5e.Bot.Services.Database
{

    public class ApplicationContext : DbContext
    {
        public DbSet<PomodoroData> PomodoroData { get; set; } = null!;

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
