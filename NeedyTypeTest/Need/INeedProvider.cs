using System;

namespace Need
{
    public interface INeedProvider
    {
        void ProvideFor(object entity, IServiceProvider serviceProvider);
    }
}

