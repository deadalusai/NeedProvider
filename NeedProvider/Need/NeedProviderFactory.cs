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
                       select new Tuple<Type, Type[]>(type, needDefs);

            return from s in sets
                   let providers = (from defType in s.Item2
                                    let needType = defType.GetGenericArguments().First()
                                    //--Invoke--
                                    //let methodInfo = s.Item1.GetInterfaceMap(defType).TargetMethods.First()
                                    //select new InvokeNeedProvider(needType, methodInfo))
                                    //--Delegate--
                                    //let methodInfo = s.Item1.GetInterfaceMap(defType).TargetMethods.First()
                                    //select new DelegateNeedProvider(s.Item1, needType, methodInfo))
                                    //--Dispatching--
                                    select new DispatchingNeedProvider(needType))
                   select new NeedProviderSet(s.Item1, providers.ToArray());
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