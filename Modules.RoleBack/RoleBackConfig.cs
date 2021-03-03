using System;
using Elvet.Core;
using Elvet.Core.Config.Exceptions;

namespace Elvet.RoleBack
{
    internal class RoleBackConfig : IValidateAble<RoleBackConfig>
    {
        public string ConnectionString { get; set; } = string.Empty;

        public TimeSpan RolesValidFor { get; set; } = TimeSpan.FromSeconds(-1);

        public RoleBackConfig Validate()
        {
            if (string.IsNullOrWhiteSpace(ConnectionString))
                throw new BadConfigurationException("Invalid connection string for RoleBack. Cannot be null or whitespace.");

            return this;
        }
    }
}
