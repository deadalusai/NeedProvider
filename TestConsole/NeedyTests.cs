
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Need;

namespace NeedyTypeTest
{
    public static class NeedyTests
    {
        public static void Run()
        {
            Test instance = new Test();

            var serviceFactory = new ServiceFactory();

            const int ITER_COUNT = 100000; //100 thousand

            #region TypeCheck
            {
                var sw = Stopwatch.StartNew();
                for (int i = 0; i < ITER_COUNT; i++)
                {
                    if (instance is INeed<Adder>)
                        ((INeed<Adder>)instance).Accept((Adder)serviceFactory.GetService(typeof(Adder)));

                    if (instance is INeed<Multiplier>)
                        ((INeed<Multiplier>)instance).Accept((Multiplier)serviceFactory.GetService(typeof(Multiplier)));

                    if (instance is INeed<Subtractor>)
                        ((INeed<Subtractor>)instance).Accept((Subtractor)serviceFactory.GetService(typeof(Subtractor)));

                    if (instance is INeed<IServiceProvider>)
                        ((INeed<IServiceProvider>)instance).Accept((IServiceProvider)serviceFactory.GetService(typeof(IServiceProvider)));
                }
                sw.Stop();
                Console.WriteLine("Simple type check: {0}", sw.Elapsed);
            }
            #endregion

            #region NeedProvider
            {
                var assm = Assembly.GetAssembly(typeof(NeedyTests));

                IDictionary<Type, INeedProvider[]> needProviderCache =
                        NeedProviderFactory.BuildProviders(assm)
                                           .ToDictionary(k => k.EntityType, v => v.Providers);

                var sw = Stopwatch.StartNew();
                for (int i = 0; i < ITER_COUNT; i++)
                {
                    var providers = needProviderCache[instance.GetType()];

                    foreach (INeedProvider provider in providers)
                        provider.ProvideFor(instance, serviceFactory);
                }
                sw.Stop();

                Console.WriteLine("NeedProvider: {0}", sw.Elapsed);
            }
            #endregion
        }

        private class Test : INeed<Adder>,
                             INeed<Subtractor>,
                             INeed<Multiplier>,
                             INeed<IServiceProvider>
        {
            void INeed<Adder>.Accept(Adder service)
            {

            }

            void INeed<Subtractor>.Accept(Subtractor service)
            {

            }

            void INeed<Multiplier>.Accept(Multiplier service)
            {
                            }

            void INeed<IServiceProvider>.Accept(IServiceProvider service)
            {

            }

            public int Apply(int input)
            {
                throw new NotImplementedException();
            }
        }
    }

    public class Adder
    {
        public int Add(int first, int second)
        {
            return first + second;
        }
    }

    public class Subtractor
    {
        public int Subtract(int first, int second)
        {
            return first - second;
        }
    }

    public class Multiplier
    {
        public int Multiply(int first, int second)
        {
            return first * second;
        }
    }

    public class ServiceFactory : IServiceProvider
    {
        private Dictionary<Type, Func<object>> _serviceBuilderCache;

        public ServiceFactory()
        {
            _serviceBuilderCache = new Dictionary<Type, Func<object>>();

            BuildServices();
        }

        private void BuildServices()
        {
            _serviceBuilderCache[typeof(Adder)]            = () => new Adder();
            _serviceBuilderCache[typeof(Subtractor)]       = () => new Subtractor();
            _serviceBuilderCache[typeof(Multiplier)]       = () => new Multiplier();
            _serviceBuilderCache[typeof(IServiceProvider)] = () => this;
        }

        public object GetService(Type serviceType)
        {
            Func<object> builder;
            if (_serviceBuilderCache.TryGetValue(serviceType, out builder))
                return builder();

            throw new InvalidOperationException("Cannot provide service for " + serviceType);
        }
    }
}

