using System;
using System.Collections.Generic;

namespace Elvet.Core.Plugins.Builder.Internal
{
    internal class PluginInstanceInfo : IPluginInstanceInfo
    {
        public IElvetPlugin Instance { get; }
        public Type PluginType => Instance.GetType();
        public IEnumerable<(Type, Type)> TypeReaderTypes { get; }
        public IEnumerable<Type> ModuleTypes { get; }

        public PluginInstanceInfo(IElvetPlugin instance, IEnumerable<(Type, Type)> typeReaderTypes, IEnumerable<Type> moduleTypes)
        {
            Instance = instance;
            TypeReaderTypes = typeReaderTypes;
            ModuleTypes = moduleTypes;
        }
    }
}
