using Microsoft.EntityFrameworkCore;

namespace Elvet.InCharacter.Data
{
    internal class InCharacterDbContext : DbContext
    {
        public DbSet<PlayerCharacter> Characters { get; set; } = default!;

        public InCharacterDbContext(DbContextOptions<InCharacterDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(InCharacterDbContext).Assembly);
        }
    }
}
