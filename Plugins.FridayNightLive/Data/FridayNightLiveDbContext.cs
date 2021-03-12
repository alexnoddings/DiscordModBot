using Microsoft.EntityFrameworkCore;

namespace Elvet.FridayNightLive.Data
{
    internal class FridayNightLiveDbContext : DbContext
    {
        public DbSet<GuildConfig> GuildConfigs { get; set; } = default!;

        public DbSet<Session> Sessions { get; set; } = default!;

        public DbSet<SessionHost> Hosts { get; set; } = default!;

        public DbSet<SessionWinner> Winners { get; set; } = default!;

        public FridayNightLiveDbContext(DbContextOptions<FridayNightLiveDbContext> options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FridayNightLiveDbContext).Assembly);
        }
    }
}
