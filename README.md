NeedProvider
============

### Description

A set of assemblies for providing interface-based dependency injection/object hydration.

### How it works

Dependancy requirements are defned by implementing a series of INeed<TService> interfaces.
Dependancies are accepted through those same interfaces.

### Usage

This is intended for use in situations where constructor injection and other traditional avenues are 
not viable, for instance when injecting dependancies into entities returned by an ORM such as 
Lightspeed, Entity Framework or NHibernate.

### Examples

A class which accepts a service of type IService

```csharp
interface IService {
	object Serve(object input);
}

class TestClass : INeed<IService> { 
	public void Accept(IService service) { 
		service.Serve(this);
	}
}
```

A simple static factory class is provided for scanning an assembly for types implementing INeed. 
The factory's output can be easily cached in a dictionary. This would usually be done at application start.

```csharp
void Setup() {
	Assembly assm = Assembly.GetAssembly(typeof(Main));
	
	IEnumerable<NeedProviderSet> providerSets = NeedProviderFactory.BuildProviders(assm);
}
```

Each NeedProviderSet maps a Type to a set of NeedProviders which can satisy its INeed requirements.

```csharp
void Runtime() {
	TestClass test = RetrieveTestClassFromORM();
	
	IServiceProvider serviceProvider = GetServiceProvider();
	
	NeedProviderSet providerSet = providerSets.First(p => p.EntityType == typeof(TestClass));
	
	foreach (INeedProvider provider in providerSet.Providers)
		provider.ProvideFor(test, serviceProvider);
}
```

Each provider retrieves the appropriate service via the System.IServiceProvider instance, and applies it to
the object through its implementation of the INeed interface method Accept(TService service).