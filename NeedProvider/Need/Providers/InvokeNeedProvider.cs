
using System;
using System.Reflection;

namespace Need.Providers
{
    public class InvokeNeedProvider : INeedProvider
    {
        private readonly Type _needType;
        private readonly MethodInfo _acceptMethodInfo;

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