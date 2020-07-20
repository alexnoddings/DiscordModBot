using System;
using Elvet.Plugins.RoleBack.Data;
using Microsoft.EntityFrameworkCore;

namespace Elvet.Data
{
    public class BotDbContext : DbContext
    {
        // DbSets are initialised as default! as EntityFramework will initialise them for us
        public DbSet<PreviousRoleDetails> PreviousRoleDetails { get; set; } = default!;
        public DbSet<PreviousRole> PreviousRoles { get; set; } = default!;

        /// <summary>
        /// Initialises a new instance of <see cref="DbContext"/>.
        /// </summary>
        /// <param name="options">The <see cref="DbContextOptions"/>.</param>
        public BotDbContext(DbContextOptions options) 
            : base(options)
        {
        }

        /// <summary>
        /// Configures the schema for the database model.
        /// </summary>
        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder = modelBuilder ?? throw new ArgumentNullException(nameof(modelBuilder));

            base.OnModelCreating(modelBuilder);

            // Applies entity configurations from plugins
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BotDbContext).Assembly);
        }
    }
}
