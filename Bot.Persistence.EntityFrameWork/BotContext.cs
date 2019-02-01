using Bot.Persistence.Domain;
using Bot.Persistence.EntityFrameWork.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Bot.Persistence.EntityFrameWork
{

    public class BotContext : DbContext
    {

        /// <summary>Table containing all the server.</summary>
        public DbSet<Server> Servers { get; set; }

        /// <summary>Table containing all the users.</summary>
        public DbSet<User> Users { get; set; }

        /// <summary>Table containing all the requests.</summary>
        public DbSet<Request> Requests { get; set; }


        /// <inheritdoc/>
        /// <example>
        /// Migrations:
        /// Follow these steps in the Package manager console.
        /// 1. Move with `cd path` to the correct folder.
        /// 2. Use: `dotnet ef migrations add InitialCreate`.
        /// 3. Use: `dotnet ef database update`.
        /// Note: The connection string needs to be hardcoded to use a migration.
        /// </example>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(DatabaseConfig.DbConfig.ConnectionString);
        }


        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RequestsConfiguration());
            modelBuilder.ApplyConfiguration(new ServersConfiguration());
            modelBuilder.ApplyConfiguration(new UsersConfiguration());
        }
    }
}
