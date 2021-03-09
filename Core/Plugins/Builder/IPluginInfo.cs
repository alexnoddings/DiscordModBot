using System;
using System.Collections.Generic;
using Discord.Commands;

namespace Elvet.Core.Plugins.Builder
{
    /// <summary>
    /// Information describing a <see cref="IElvetPlugin"/>.
    /// </summary>
    public interface IPluginInfo
    {
        /// <summary>
        /// The <see cref="Type"/> of the <see cref="IElvetPlugin"/>. This type must be assignable to <see cref="IElvetPlugin"/>.
        /// </summary>
        public Type PluginType { get; }

        /// <summary>
        /// The <see cref="Type"/>s of <see cref="TypeReader"/>s registered by this <see cref="IElvetPlugin"/>.
        /// Each pair is the value type it reads and the reader type. The reader type must be assignable to <see cref="TypeReader"/>.
        /// </summary>
        public IEnumerable<(Type, Type)> TypeReaderTypes { get; }

        /// <summary>
        /// The <see cref="Type"/>s of <see cref="ModuleBase{T}"/>s registered by this <see cref="IElvetPlugin"/>.
        /// This type must be assignable to <see cref="ModuleBase{T}"/>.
        /// </summary>
        public IEnumerable<Type> ModuleTypes { get; }
    }
}
