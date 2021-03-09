using Microsoft.EntityFrameworkCore;

namespace Elvet.RoleBack.Data
{
    internal class RoleBackDbContext : DbContext
    {
        public DbSet<UserGuildRoles> UserGuildRoles { get; set; } = default!;

        public RoleBackDbContext(DbContextOptions<RoleBackDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RoleBackDbContext).Assembly);
        }
    }
}
