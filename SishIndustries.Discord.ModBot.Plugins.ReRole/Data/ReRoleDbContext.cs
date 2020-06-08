using Microsoft.EntityFrameworkCore;

namespace SishIndustries.Discord.ModBot.Plugins.ReRole.Data
{
    public class ReRoleDbContext : DbContext
    {
        // Automatically set by Entity Framework
        public DbSet<PreviousRoleDetails> PreviousRoleDetails { get; set; } = default!;
        public DbSet<PreviousRole> PreviousRoles { get; set; } = default!;

        public ReRoleDbContext(DbContextOptions<ReRoleDbContext> options) 
            : base(options)
        {
        }

        // Ignore possible null reference returns as navigation expressions aren't executed
        #pragma warning disable CS8603
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<PreviousRoleDetails>().HasKey(prd => prd.Id);
            builder.Entity<PreviousRole>().HasKey(pr => pr.Id);

            builder
                .Entity<PreviousRoleDetails>()
                .HasMany<PreviousRole>(prd => prd.PreviousRoles)
                .WithOne(pr => pr.Details)
                .HasForeignKey(pr => pr.DetailsId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
        #pragma warning restore CS8603
    }
}
