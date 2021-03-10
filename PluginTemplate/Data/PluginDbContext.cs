using Microsoft.EntityFrameworkCore;

namespace $safeprojectname$.Data
{
    internal class $pluginname$DbContext : DbContext
    {
        public $pluginname$DbContext(DbContextOptions<$pluginname$DbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof($pluginname$DbContext).Assembly);
        }
    }
}
