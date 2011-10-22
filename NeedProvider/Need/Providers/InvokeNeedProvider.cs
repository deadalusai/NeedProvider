using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using Need;

namespace Need.Providers
{
    public class InvokeNeedProvider : INeedProvider
    {
        private Type _entityType;
        private Type _needType;
        private MethodInfo _acceptMethodInfo;

        public InvokeNeedProvider(Type entityType, Type needType, MethodInfo acceptMethodInfo)
        {
            _entityType = entityType;
            _needType = needType;
            _acceptMethodInfo = acceptMethodInfo;
        }

        public void ProvideFor(object entity, IServiceProvider serviceProvider)
        {
            _acceptMethodInfo.Invoke(entity, new[] { serviceProvider.GetService(_needType) });
        }
    }
}