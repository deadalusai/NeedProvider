
using System;
using System.Reflection;

namespace Need.Providers
{
    public class DelegateNeedProvider : INeedProvider
    {
        private readonly Type _entityType;
        private readonly Type _needType;
        private readonly MethodInfo _acceptMethodInfo;
        private readonly Delegate _applyDelegate;
    
        public DelegateNeedProvider(Type entityType, Type needType, MethodInfo acceptMethodInfo)
        {
            _entityType = entityType;
            _needType = needType;
            _acceptMethodInfo = acceptMethodInfo;
            _applyDelegate = BuildApplyDelegate();
        }

        public void ProvideFor(object entity, IServiceProvider serviceProvider)
        {
            _applyDelegate.DynamicInvoke(entity, serviceProvider.GetService(_needType));
        }
    
        private Delegate BuildApplyDelegate()
        {
            Type applyActionType = typeof(Action<,>).MakeGenericType(_entityType, _needType);
    
            return Delegate.CreateDelegate(applyActionType, _acceptMethodInfo);
        }
    }
}
