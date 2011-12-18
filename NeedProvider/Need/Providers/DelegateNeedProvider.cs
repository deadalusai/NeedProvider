
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using Need;

namespace Need.Providers
{
    public class DelegateNeedProvider : INeedProvider
    {
        private Type _entityType;
        private Type _needType;
        private MethodInfo _acceptMethodInfo;
        private Delegate _applyDelegate;
    
        public DelegateNeedProvider(Type entityType, Type needType, MethodInfo acceptMethodInfo)
        {
            _entityType = entityType;
            _needType = needType;
            _acceptMethodInfo = acceptMethodInfo;
            _applyDelegate = BuildApplyDelegate();
        }

        public void ProvideFor(object entity, System.IServiceProvider serviceProvider)
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
