using System.Threading.Tasks;
using Discord.Commands;
using Elvet.Core.Commands;
using $safeprojectname$.Data;

namespace $safeprojectname$
{
    internal class $pluginname$Module : ElvetModuleBase
    {
        private readonly $pluginname$Config _config;
        private readonly $pluginname$DbContext _dbContext;

        public $pluginname$Module($pluginname$Config config, $pluginname$DbContext dbContext)
        {
            _config = config;
            _dbContext = dbContext;
        }

        [Command("$pluginname$")]
        public async Task $pluginname$()
        {
            await MarkSuccessful();
        }
    }
}
