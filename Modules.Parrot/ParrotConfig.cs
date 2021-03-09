using System;
using Elvet.Core;
using Elvet.Core.Config.Exceptions;
using Elvet.Core.Plugins;

namespace Elvet.Parrot
{
    internal class ParrotConfig : IPluginConfig, IValidateAble<ParrotConfig>
    {
        public bool Enabled { get; set; } = true;

        public string ConnectionString { get; set; } = string.Empty;

        /// <summary>
        /// The maximum time into the future a message can be scheduled.
        /// </summary>
        public TimeSpan MaxTriggerTime { get; set; } = TimeSpan.FromDays(7);

        /// <summary>
        /// The maximum size in characters a message can be.
        /// </summary>
        public int MaxMessageSize { get; set; } = 512;

        /// <summary>
        /// The maximum number of reminders one user can have scheduled.
        /// </summary>
        public int MaxReminders { get; set; } = 8;

        public ParrotConfig Validate()
        {
            if (MaxTriggerTime < TimeSpan.Zero)
                MaxTriggerTime = TimeSpan.MaxValue;

            if (string.IsNullOrWhiteSpace(ConnectionString))
                throw new BadConfigurationException(nameof(ConnectionString), nameof(ParrotConfig), "cannot be null or whitespace.");

            if (MaxReminders < 1)
                throw new BadConfigurationException(nameof(MaxReminders), nameof(ParrotConfig), "must be >= 1.");

            return this;
        }
    }
}
