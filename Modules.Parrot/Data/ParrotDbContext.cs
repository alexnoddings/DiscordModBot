using Microsoft.EntityFrameworkCore;

namespace Elvet.Parrot.Data
{
    internal class ParrotDbContext : DbContext
    {
        public DbSet<UserParrotMessage> UserMessages { get; set; } = default!;

        public ParrotDbContext(DbContextOptions<ParrotDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ParrotDbContext).Assembly);
        }
    }
}
