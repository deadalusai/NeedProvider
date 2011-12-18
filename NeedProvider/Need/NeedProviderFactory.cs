
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using Need.Providers;
using NeedProvider.Need.Providers;

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
            var entityDefinitions = from type in scanAssembly.GetTypes()
                       let needDefs = type.GetInterfaces()
                                          .Where(i => i.IsGenericType)
                                          .Where(i => i.GetGenericTypeDefinition() == typeof(INeed<>))
                                          .ToArray()
                       where needDefs.Any()
                       select new { EntityType = type, NeedDefinitions = needDefs };

            return from entity in entityDefinitions
                   let providers = (from defType in entity.NeedDefinitions
                                    let needType = defType.GetGenericArguments().First()
                                    //--Invoke--
                                    //let methodInfo = s.Item1.GetInterfaceMap(defType).TargetMethods.First()
                                    //select new InvokeNeedProvider(needType, methodInfo))
                                    //--Delegate--
                                    //let methodInfo = s.Item1.GetInterfaceMap(defType).TargetMethods.First()
                                    //select new DelegateNeedProvider(s.EntityType, needType, methodInfo))
                                    //--Dispatching--
                                    //select new DispatchingNeedProvider(needType))
                                    //--Compiled Delegate--
                                    let methodInfo = entity.EntityType.GetInterfaceMap(defType).TargetMethods.First()
                                    select new CompiledDelegateNeedProvider(entity.EntityType, needType, methodInfo))
                   select new NeedProviderSet(entity.EntityType, providers.ToArray());
        }
    }

    public class NeedProviderSet
    {
        public NeedProviderSet(Type entityType, INeedProvider[] needProviders)
        {
            EntityType = entityType;
            Providers = needProviders;
        }

        public Type EntityType { get; private set; }
        
        public INeedProvider[] Providers { get; private set; }
    }
}