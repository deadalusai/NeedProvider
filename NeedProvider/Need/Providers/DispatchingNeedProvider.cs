
using System;

namespace Need.Providers
{
    public class DispatchingNeedProvider : INeedProvider
    {
        private readonly Type _needType;
        private readonly Dispatcher _dispatcher;

        public DispatchingNeedProvider(Type needType)
        {
            _needType = needType;
            _dispatcher = BuildDispatcher(needType);
        }

        public void ProvideFor(object entity, IServiceProvider serviceProvider)
        {
            _dispatcher.Dispatch(entity, serviceProvider.GetService(_needType));
        }

        private static Dispatcher BuildDispatcher(Type needType)
        {
            Type dispatcherType = typeof(Dispatcher<>).MakeGenericType(needType);

            return (Dispatcher)Activator.CreateInstance(dispatcherType);
        }

        private abstract class Dispatcher
        {
            public abstract void Dispatch(object entity, object service);
        }

        private sealed class Dispatcher<TNeed> : Dispatcher
        {
            public override void Dispatch(object entity, object service)
            {
                ((INeed<TNeed>)entity).Accept((TNeed)service);
            }
        }
    }
}