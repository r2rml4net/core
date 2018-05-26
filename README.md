# r2rml4net [![Build status][build-badge]][build-link] [![NuGet version][nuget-badge]][nuget-link]

Welcome to r2rml4net, a project which brings [R2RML](http://www.w3.org/TR/r2rml/) to the .NET world.

## Getting

```
nuget install r2rml4net
```

Or clone this repository.

## Features

As of version 0.5 the r2rml4net library:

* Is nearly fully compatible with the [R2RML specifications](http://www.w3.org/TR/r2rml/),
* Can load existing R2RML mapping graphs from any RDF serialization,
* Can creating R2RML graphs programmatically using a fluent API,
* Can generate R2RML graphs from relational databases (aka [Direct Mapping](http://www.w3.org/TR/rdb-direct-mapping/)),
* Can convert data from relational databases to structured data,
* Has been tested and supports Microsoft SQL Server.
* Supports .NET Framework 4.5.2 nad .NET Standard 2.0

## Important stuff to be done

* Support for other database managements systems
* Enhance the fluent API to support modifying mapping (current is append-only)

[nuget-badge]: https://badge.fury.io/nu/nuget.svg
[nuget-link]: https://badge.fury.io/nu/nuget
[build-badge]: https://ci.appveyor.com/api/projects/status/8y8hj6jd6d0urw6p/branch/master?svg=true
[build-link]: https://ci.appveyor.com/project/tpluscode/r2rml4net/branch/master
