using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Need;
using System.Reflection;
using System.Linq.Expressions;

namespace NeedProvider.Need.Providers
{
    public class CompiledDelegateNeedProvider : INeedProvider
    {
        private Type _entityType;
        private Type _needType;
        private MethodInfo _acceptMethodInfo;
        private Action<object, object> _applyAction;

        public CompiledDelegateNeedProvider(Type entityType, Type needType, MethodInfo acceptMethodInfo)
        {
            _entityType = entityType;
            _needType = needType;
            _acceptMethodInfo = acceptMethodInfo;
            _applyAction = BuildAction();
        }

        public void ProvideFor(object entity, System.IServiceProvider serviceProvider)
        {
            _applyAction(entity, serviceProvider.GetService(_needType));
        }
    
        private Action<object, object> BuildAction()
        {
            var entityParameter = Expression.Parameter(typeof(object), "entity");
            var entityCast = Expression.TypeAs(entityParameter, _entityType);

            var serviceParameter = Expression.Parameter(typeof(object), "service");
            var serviceCast = Expression.TypeAs(serviceParameter, _needType);

            var callExpr = Expression.Call(entityCast, _acceptMethodInfo, serviceCast);

            var lambdaExpr = Expression.Lambda<Action<object, object>>(callExpr, entityParameter, serviceParameter);

            return lambdaExpr.Compile();
        }
    }
}
