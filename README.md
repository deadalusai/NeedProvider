NeedProvider
============

### Description

A set of assemblies for providing interface-based dependancy injection/object hydration.

### How it works

Dependancy requirements are defned by implementing a series of INeed<TService> interfaces.
Dependancies are accepted through those same interfaces.

### Usage

This is intended for use in situations where constructor injection and other traditional avenues are 
not viable, for instance when injecting dependancies into entities returned by an ORM such as 
Lightspeed, Entity Framework and NHibernate.

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