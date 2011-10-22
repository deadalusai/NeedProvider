using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using Need;

namespace Need.Providers
{
    public class DispatchingNeedProvider : INeedProvider
    {
        private Type _entityType;
        private Type _needType;
        private MethodInfo _acceptMethodInfo;
        private Dispatcher _dispatcher;

        public DispatchingNeedProvider(Type entityType, Type needType, MethodInfo acceptMethodInfo)
        {
            _entityType = entityType;
            _needType = needType;
            _acceptMethodInfo = acceptMethodInfo;
            _dispatcher = BuildDispatcher();
        }

        public void ProvideFor(object entity, IServiceProvider serviceProvider)
        {
            _dispatcher.Accept(entity, serviceProvider.GetService(_needType));
        }

        private Dispatcher BuildDispatcher()
        {
            var typedDispatcher = typeof(Dispatcher<>).MakeGenericType(_needType);

            return (Dispatcher)Activator.CreateInstance(typedDispatcher);
        }

        private abstract class Dispatcher
        {
            public abstract void Accept(object entity, object service);
        }

        private sealed class Dispatcher<TNeed> : Dispatcher
        {
            public override void Accept(object entity, object service)
            {
                ((INeed<TNeed>)entity).Accept((TNeed)service);
            }
        }
    }
}