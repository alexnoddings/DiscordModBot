using $safeprojectname$.Data;

namespace $safeprojectname$
{
    /// <summary>
    /// The scoped service which handles $pluginname$ events.
    /// </summary>
    internal class $pluginname$Service
    {
        private readonly $pluginname$DbContext _dbContext;

        /// <summary>
        /// Initialises a new instance of the <see cref="$pluginname$Service" />.
        /// </summary>
        /// <param name="dbContext">The <see cref="$pluginname$DbContext" /> to get data from.</param>
        public $pluginname$Service($pluginname$DbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
