using System;
using Elvet.Core;
using Elvet.Core.Plugins.Config;

namespace Elvet.RoleBack
{
    internal class RoleBackConfig : IPluginConfig, IConnectionStringConfig, IValidateAble<RoleBackConfig>
    {
        public bool Enabled { get; set; } = true;

        public string? ConnectionString { get; set; }

        /// <summary>
        /// How long roles are valid for after disconnection. If a user joins after this time, their roles won't be reinstated.
        /// </summary>
        public TimeSpan RolesValidFor { get; set; } = TimeSpan.FromDays(3);

        public RoleBackConfig Validate()
        {
            if (RolesValidFor < TimeSpan.Zero)
                RolesValidFor = TimeSpan.MaxValue;

            return this;
        }
    }
}
