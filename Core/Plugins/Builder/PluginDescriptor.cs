using System;
using System.Collections.Generic;
using System.Linq;
using Discord.Commands;

namespace Elvet.Core.Plugins.Builder
{
    /// <summary>
    /// Describes an <see cref="IElvetPlugin"/> with it's type, type readers, and module types.
    /// </summary>
    public class PluginDescriptor : IPluginInfo
    {
        private readonly List<Type> _moduleTypes = new();
        private readonly Dictionary<Type, Type> _typeReaderTypes = new();

        public Type PluginType { get; }
        public IEnumerable<(Type, Type)> TypeReaderTypes => _typeReaderTypes.Select(kv => (kv.Key, kv.Value));
        public IEnumerable<Type> ModuleTypes => _moduleTypes.AsEnumerable();

        /// <summary>
        /// Initialises a new instance of <see cref="PluginDescriptor"/>.
        /// </summary>
        /// <param name="pluginType">The <see cref="Type"/> of the <see cref="IElvetPlugin"/>.</param>
        public PluginDescriptor(Type pluginType)
        {
            if (pluginType is null)
                throw new ArgumentNullException(nameof(pluginType));

            if (!pluginType.IsAssignableTo(typeof(IElvetPlugin)))
                throw new BadTypeException("Bad plugin type.", nameof(pluginType), typeof(IElvetPlugin), pluginType);

            PluginType = pluginType;
        }

        /// <summary>
        /// Will try to add the <paramref name="moduleType"/> to the <see cref="ModuleTypes"/>.
        /// Will not re-add the module if it has already been added.
        /// </summary>
        /// <param name="moduleType">The <see cref="Type"/> to add.</param>
        /// <returns><see langword="true"/> if the module was added, otherwise <see langword="false"/>.</returns>
        public bool TryAddModule(Type moduleType)
        {
            if (_moduleTypes.Contains(moduleType))
                return false;

            _moduleTypes.Add(moduleType);
            return true;
        }

        /// <summary>
        /// Will try to add the <paramref name="typeReaderType"/> which reads <paramref name="valueType"/>s to the <see cref="TypeReaderTypes"/>.
        /// Will not re-add the type reader if it has already been added.
        /// </summary>
        /// <param name="valueType">The <see cref="Type"/> to which is read.</param>
        /// <param name="typeReaderType">The <see cref="Type"/> which is a <see cref="TypeReader"/>.</param>
        /// <returns><see langword="true"/> if the module was added, otherwise <see langword="false"/>.</returns>
        public bool TryAddTypeReader(Type valueType, Type typeReaderType)
        {
            if (_typeReaderTypes.ContainsKey(valueType))
                return false;

            _typeReaderTypes.Add(valueType, typeReaderType);
            return true;
        }
    }
}
