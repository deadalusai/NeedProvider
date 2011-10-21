using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Need
{
    public interface INeed<TService>
    {
          void Accept(TService service);
    }
}