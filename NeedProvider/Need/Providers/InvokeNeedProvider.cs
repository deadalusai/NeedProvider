using System;
using System.Reflection;

using Need;

namespace Need.Providers
{
    public class InvokeNeedProvider : INeedProvider
    {
        private Type _needType;
        private MethodInfo _acceptMethodInfo;

        public InvokeNeedProvider(Type needType, MethodInfo acceptMethodInfo)
        {
            _needType = needType;
            _acceptMethodInfo = acceptMethodInfo;
        }

        public void ProvideFor(object entity, IServiceProvider serviceProvider)
        {
            _acceptMethodInfo.Invoke(entity, new[] { serviceProvider.GetService(_needType) });
        }
    }
}