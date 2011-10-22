using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using Need.Providers;

namespace Need
{
    public static class NeedProviderFactory
    {
        /// <summary>
        /// Note: the providerType is just a hack to control the output of this factory
        /// </summary>
        public static IEnumerable<NeedProviderSet> BuildProviders(Assembly scanAssembly)
        {
            //Find the types which implement INeed<>
            var sets = from type in scanAssembly.GetTypes()
                       let needDefs = type.GetInterfaces()
                                          .Where(i => i.IsGenericType)
                                          .Where(i => i.GetGenericTypeDefinition() == typeof(INeed<>))
                                          .ToArray()
                       where needDefs.Any()
                       select new { EntityType = type, NeedDefinitionTypes = needDefs };

            return from s in sets
                   let providers = (from defType in s.NeedDefinitionTypes
                                    let needType = defType.GetGenericArguments().First()
                                    select new DispatchingNeedProvider(needType))
                   select new NeedProviderSet(s.EntityType, providers.ToArray());
        }
    }

    public struct NeedProviderSet
    {
        public readonly Type EntityType;
        public readonly INeedProvider[] Providers;

        public NeedProviderSet(Type entityType, INeedProvider[] needProviders)
            : this()
        {
            EntityType = entityType;
            Providers = needProviders;
        }
    }
}