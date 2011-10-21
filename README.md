NeedProvider
============

Description
-----------
This is a set of projects for providing interface-based dependancy injection.

Dependancy requirements are defned by implementing a series of INeed<TService> interfaces.
Dependancies are accepted through those same interfaces.

This is intended for use in situations where constructor injection and other traditional avenues are 
not viable, for instance when injecting dependancies into entities returned by an ORM such as 
Lightspeed, Entity Framework and NHibernate.