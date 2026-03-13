> **Note** This repository is developed for .netstandard2.0+

[![NuGet Version](https://img.shields.io/nuget/v/MockAsyncEnumerable.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/MockAsyncEnumerable/)
[![Nuget Downloads](https://img.shields.io/nuget/dt/MockAsyncEnumerable.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/MockAsyncEnumerable)

A simple implementation for transforming synchronous collections into async enumerables, enabling seamless testing of EF Core queries, paginated grids, and LINQ operations, or any action or dynamic aggregated query using EF Core (`Microsoft.EntityFrameworkCore`) through Expressions (`System.Linq.Expressions`) with IAsyncEnumerable<T>.

This library was born from the necessity to implement paged grid results in projects where data was provided by stored procedures or in-memory collections, while still needing to support async EF Core APIs like ToListAsync(), FirstOrDefaultAsync(), etc.

## Features
- Convert `IEnumerable<T>`, `IQueryable<T>`, and arrays to `IAsyncEnumerable<T>`;
- **Builder pattern** for fluent async enumerable construction;
- **Factory methods** for quick creation (Empty, Single, Create);
- Full support for LINQ async operations (Where, Select, OrderBy, etc.);
- Comprehensive null guards and error handling;
- Proper cancellation token support;
- Optimized disposal patterns (IAsyncDisposable);
- Extensive test coverage;
- Compatible with EF Core testing scenarios;


## Installation

**In case you wish to use it in your project, u can install the package from <a href="https://www.nuget.org/packages/MockAsyncEnumerable" target="_blank">nuget.org</a>** or specify what version you want:

> `Install-Package MockAsyncEnumerable`

Or specify a version:

> `Install-Package MockAsyncEnumerable -Version x.x.x.x`

## Examples
### Using Extension Methods

```csharp
using MockAsyncEnumerable.Extensions;
using Microsoft.EntityFrameworkCore;

// Convert a list to async enumerable
var users = new List<User>
{
    new User { Id = 1, Name = "Alice" },
    new User { Id = 2, Name = "Bob" }
};

var asyncUsers = users.ToMockAsyncEnumerable();
var result = await asyncUsers.Where(u => u.Id > 1).ToListAsync();
```

### Using Factory Methods

```csharp
using MockAsyncEnumerable;
using Microsoft.EntityFrameworkCore;

// Create empty async enumerable
var empty = AsyncEnumerableFactory.Empty<User>();

// Create from single item
var single = AsyncEnumerableFactory.Single(new User { Id = 1, Name = "Alice" });

// Create from multiple items
var multiple = AsyncEnumerableFactory.Create(
    new User { Id = 1, Name = "Alice" },
    new User { Id = 2, Name = "Bob" }
);

var users = await multiple.ToListAsync();
```

### Using Builder Pattern

```csharp
using MockAsyncEnumerable;
using Microsoft.EntityFrameworkCore;

// Build an async enumerable fluently
var users = AsyncEnumerableFactory.Builder<User>()
    .Add(new User { Id = 1, Name = "Alice" })
    .Add(new User { Id = 2, Name = "Bob" })
    .AddRange(moreUsers)
    .Build();

var result = await users.ToListAsync();
```

### Using EnumerableInvoker

```csharp
using MockAsyncEnumerable;
using Microsoft.EntityFrameworkCore;

var data = new List<User>
{
    new User { Id = 1, Name = "Alice" },
    new User { Id = 2, Name = "Bob" }
};

var asyncEnumerable = EnumerableInvoker.Invoke(data);
var result = await asyncEnumerable.FirstOrDefaultAsync(u => u.Name == "Alice");
```

## API Reference

### Extension Methods (MockAsyncEnumerable.Extensions)

| Method | Description |
|--------|-------------|
| `ToMockAsyncEnumerable<T>(this IEnumerable<T>)` | Converts any enumerable to async enumerable |
| `ToMockAsyncEnumerable<T>(this IQueryable<T>)` | Converts queryable to async enumerable |
| `ToMockAsyncEnumerable<T>(this T[])` | Converts array to async enumerable |

### Factory Methods (AsyncEnumerableFactory)

| Method | Description |
|--------|-------------|
| `Empty<T>()` | Creates an empty async enumerable |
| `Single<T>(T item)` | Creates async enumerable with single item |
| `Create<T>(params T[] items)` | Creates async enumerable from items |
| `Builder<T>()` | Returns a new builder instance |

### Builder Pattern (AsyncEnumerableBuilder<T>)

| Method | Description |
|--------|-------------|
| `Add(T item)` | Adds a single item (fluent) |
| `AddRange(IEnumerable<T> items)` | Adds multiple items (fluent) |
| `Build()` | Creates the async enumerable |
| `Clear()` | Resets the builder |

### Classic Approach (EnumerableInvoker)

| Method | Description |
|--------|-------------|
| `Invoke<T>(IEnumerable<T>)` | Converts enumerable to async enumerable |


## Content
1. [USING](docs/usage.md)
1. [CHANGELOG](docs/CHANGELOG.md)
1. [BRANCH-GUIDE](docs/branch-guide.md)