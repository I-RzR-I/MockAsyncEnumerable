> **Note** This repository is developed for .netstandard2.0+

[![NuGet Version](https://img.shields.io/nuget/v/MockAsyncEnumerable.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/MockAsyncEnumerable/)
[![Nuget Downloads](https://img.shields.io/nuget/dt/MockAsyncEnumerable.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/MockAsyncEnumerable)

A simple implementation for transforming synchronous collections into async enumerables, enabling seamless testing of EF Core queries, paginated grids, and LINQ operations, or any action or dynamic aggregated query using EF Core (`Microsoft.EntityFrameworkCore`) through Expressions (`System.Linq.Expressions`) with `IAsyncEnumerable<T>`.

This library was born from the necessity to implement paged grid results in projects where data was provided by stored procedures or in-memory collections, while still needing to support async EF Core APIs like `ToListAsync()`, `FirstOrDefaultAsync()`, etc.

> **Root namespace:** `RzR.Extensions.EntityMock` (with sub-namespaces `.Abstractions`, `.Extensions`, `.Faults`, `.Helpers`).

## Features
- Convert `IEnumerable<T>`, `IQueryable<T>`, and arrays to `IAsyncEnumerable<T>` / `IQueryable<T>`;
- Public APIs return the **`IMockAsyncEnumerable<T>`** abstraction (combines `IAsyncEnumerable<T>` + `IQueryable<T>`);
- **Builder pattern** for fluent async enumerable construction;
- **Factory methods** for quick creation (`Empty`, `Single`, `Create`);
- **Fault injection** on the builder: artificial latency, throw-at-index, and time-based cancellation — useful for testing retry, timeout and cancellation logic;
- Full support for LINQ async operations (`Where`, `Select`, `OrderBy`, etc.);
- Comprehensive null guards and error handling;
- Proper cancellation token support (linked tokens for combined caller / internal cancellation);
- Optimized disposal patterns (`IAsyncDisposable`);
- Multi-target: `netstandard2.0`, `netstandard2.1`, `net5.0`–`net9.0`.


## Installation

**In case you wish to use it in your project, u can install the package from <a href="https://www.nuget.org/packages/MockAsyncEnumerable" target="_blank">nuget.org</a>** or specify what version you want:

> `Install-Package MockAsyncEnumerable`

Or specify a version:

> `Install-Package MockAsyncEnumerable -Version x.x.x.x`

## Examples
### Using Extension Methods

```csharp
using RzR.Extensions.EntityMock.Extensions;
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
using RzR.Extensions.EntityMock;
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
using RzR.Extensions.EntityMock;
using Microsoft.EntityFrameworkCore;

// Build an async enumerable fluently
var users = AsyncEnumerableFactory.Builder<User>()
    .Add(new User { Id = 1, Name = "Alice" })
    .Add(new User { Id = 2, Name = "Bob" })
    .AddRange(moreUsers)
    .Build();

var result = await users.ToListAsync();
```

### Fault Injection (latency, exceptions, cancellation)

The builder can inject realistic failure modes so tests can exercise retry,
timeout and cancellation paths in production code without changing it:

```csharp
using RzR.Extensions.EntityMock;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

var slow = AsyncEnumerableFactory.Builder<User>()
    .AddRange(users)
    .WithDelay(TimeSpan.FromMilliseconds(50)) // per-item latency
    .ThrowAfter(3, _ => new DbUpdateException("forced")) // throw at index 3
    .CancelAfter(TimeSpan.FromSeconds(1)) // internal cancel
    .Build();

// Caller-supplied cancellation token is honored alongside the internal one.
await foreach (var u in slow.WithCancellation(ct)) { /* ... */ }
```

Available builder fault APIs:

| Method | Description |
|--------|-------------|
| `WithDelay(TimeSpan)` | Awaits the delay before every `MoveNextAsync` call |
| `ThrowAfter(int, Exception)` | Throws the supplied exception when the given index is reached |
| `ThrowAfter(int, Func<int, Exception>)` | Same, but the factory receives the triggering index |
| `CancelAfter(TimeSpan)` | Starts an internal `CancellationTokenSource` linked with the caller token |

### Using EnumerableInvoker (deprecated)

> !!! `EnumerableInvoker` is marked `[Obsolete]` and will be removed in the next major version.
> Prefer `AsyncEnumerableFactory`, the `ToMockAsyncEnumerable()` extension methods, or `AsyncEnumerableBuilder<T>`.

```csharp
using RzR.Extensions.EntityMock;
using Microsoft.EntityFrameworkCore;

var data = new List<User>
{
    new User { Id = 1, Name = "Alice" },
    new User { Id = 2, Name = "Bob" }
};

#pragma warning disable CS0618
var asyncEnumerable = EnumerableInvoker.Invoke(data);
#pragma warning restore CS0618

var result = await asyncEnumerable.FirstOrDefaultAsync(u => u.Name == "Alice");
```

## API Reference

> All factory / builder / extension methods now return `IMockAsyncEnumerable<T>`
> (defined in `RzR.Extensions.EntityMock.Abstractions`), which extends both
> `IAsyncEnumerable<T>` and `IQueryable<T>`.

### Abstraction (`RzR.Extensions.EntityMock.Abstractions`)

| Type | Description |
|--------|-------------|
| `IMockAsyncEnumerable<T>` | Public, stable surface — combines `IAsyncEnumerable<T>` + `IQueryable<T>` |

### Extension Methods (`RzR.Extensions.EntityMock.Extensions`)

| Method | Description |
|--------|-------------|
| `ToMockAsyncEnumerable<T>(this IEnumerable<T>)` | Converts any enumerable to async enumerable |
| `ToMockAsyncEnumerable<T>(this IQueryable<T>)` | Converts queryable to async enumerable |
| `ToMockAsyncEnumerable<T>(this T[])` | Converts array to async enumerable |

### Factory Methods (`AsyncEnumerableFactory`)

| Method | Description |
|--------|-------------|
| `Empty<T>()` | Creates an empty async enumerable |
| `Single<T>(T item)` | Creates async enumerable with single item |
| `Create<T>(params T[] items)` | Creates async enumerable from items |
| `Builder<T>()` | Returns a new builder instance |

### Builder Pattern (`AsyncEnumerableBuilder<T>`)

| Method | Description |
|--------|-------------|
| `Add(T item)` | Adds a single item (fluent) |
| `AddRange(IEnumerable<T> items)` | Adds multiple items (fluent) |
| `WithDelay(TimeSpan delay)` | Configures per-item async latency (fluent) |
| `ThrowAfter(int, Exception)` | Throws at the configured zero-based index (fluent) |
| `ThrowAfter(int, Func<int, Exception>)` | Same, with index-aware factory (fluent) |
| `CancelAfter(TimeSpan)` | Cancels iteration after the duration (fluent) |
| `Build()` | Snapshots items and creates the async enumerable |
| `Clear()` | Resets items **and** fault settings |

### Classic Approach (`EnumerableInvoker`) — **deprecated**

| Method | Description |
|--------|-------------|
| `Invoke<T>(IEnumerable<T>)` | `[Obsolete]` — use `ToMockAsyncEnumerable()` instead |


## Content
1. [USING](docs/usage.md)
2. [CHANGELOG](docs/CHANGELOG.md)
3. [BRANCH-GUIDE](docs/branch-guide.md)