# USING

This doc provides comprehensive examples for using MockAsyncEnumerable in your projects.

> **Root namespace:** `RzR.Extensions.EntityMock` (with sub-namespaces `.Abstractions`, `.Extensions`, `.Faults`, `.Helpers`).
> All factory / builder / extension methods return `IMockAsyncEnumerable<T>`, which combines
> `IAsyncEnumerable<T>` and `IQueryable<T>`.

## Table of Contents

- [Installation](#installation)
- [Basic Usage](#basic-usage)
- [Extension Methods](#extension-methods)
- [Factory Pattern](#factory-pattern)
- [Builder Pattern](#builder-pattern)
- [Fault Injection](#fault-injection)
- [Advanced Scenarios](#advanced-scenarios)
- [Best Practices](#best-practices)
- [Migration Guide](#migration-guide)

---

## Installation

Install via NuGet Package Manager:

```bash
Install-Package MockAsyncEnumerable
```

Or via .NET CLI:

```bash
dotnet add package MockAsyncEnumerable
```

---

## Basic Usage

### Using EnumerableInvoker (deprecated)

> !!! `EnumerableInvoker` is marked `[Obsolete]` and will be removed in the next major version.
> Prefer `AsyncEnumerableFactory`, `ToMockAsyncEnumerable()`, or `AsyncEnumerableBuilder<T>`.

```csharp
using RzR.Extensions.EntityMock;
using Microsoft.EntityFrameworkCore;

var users = new List<User>
{
    new User { Id = 1, Name = "Alice", Age = 30 },
    new User { Id = 2, Name = "Bob", Age = 25 },
    new User { Id = 3, Name = "Charlie", Age = 35 }
};

#pragma warning disable CS0618
var asyncEnumerable = EnumerableInvoker.Invoke(users);
#pragma warning restore CS0618

// Use with async LINQ operations
var result = await asyncEnumerable
    .Where(u => u.Age > 25)
    .ToListAsync();
```

---

## Extension Methods

### Convert IEnumerable<T> to Async Enumerable

The approach using extension methods:

```csharp
using RzR.Extensions.EntityMock.Extensions;
using Microsoft.EntityFrameworkCore;

var users = new List<User>
{
    new User { Id = 1, Name = "Alice" },
    new User { Id = 2, Name = "Bob" }
};

var asyncUsers = users.ToMockAsyncEnumerable();
var result = await asyncUsers.FirstOrDefaultAsync(u => u.Name == "Alice");
```

### Convert IQueryable<T> to Async Enumerable

```csharp
using RzR.Extensions.EntityMock.Extensions;

var query = users.AsQueryable().Where(u => u.IsActive);
var asyncQuery = query.ToMockAsyncEnumerable();

var activeUsers = await asyncQuery.ToListAsync();
```

### Convert Arrays to Async Enumerable

```csharp
using RzR.Extensions.EntityMock.Extensions;

var userArray = new[]
{
    new User { Id = 1, Name = "Alice" },
    new User { Id = 2, Name = "Bob" }
};

var asyncUsers = userArray.ToMockAsyncEnumerable();
var count = await asyncUsers.CountAsync();
```

---

## Factory Pattern

The factory provides convenient methods for creating async enumerables:

### Create Empty Async Enumerable

```csharp
using RzR.Extensions.EntityMock;

var empty = AsyncEnumerableFactory.Empty<User>();
var count = await empty.CountAsync(); // Returns 0
```

### Create Single Item Async Enumerable

```csharp
using RzR.Extensions.EntityMock;

var singleUser = AsyncEnumerableFactory.Single(new User { Id = 1, Name = "Alice" });
var user = await singleUser.FirstAsync(); // Returns the single user
```

### Create from Multiple Items

```csharp
using RzR.Extensions.EntityMock;

var users = AsyncEnumerableFactory.Create(
    new User { Id = 1, Name = "Alice" },
    new User { Id = 2, Name = "Bob" },
    new User { Id = 3, Name = "Charlie" }
);

var list = await users.ToListAsync();
```

---

## Builder Pattern

For complex scenarios, use the builder pattern for fluent construction:

### Basic Builder Usage

```csharp
using RzR.Extensions.EntityMock;

var builder = AsyncEnumerableFactory.Builder<User>();

builder.Add(new User { Id = 1, Name = "Alice" });
builder.Add(new User { Id = 2, Name = "Bob" });

var users = builder.Build();
var result = await users.ToListAsync();
```

### Fluent Builder Chain

```csharp
using RzR.Extensions.EntityMock;

var users = AsyncEnumerableFactory.Builder<User>()
    .Add(new User { Id = 1, Name = "Alice" })
    .Add(new User { Id = 2, Name = "Bob" })
    .Build();

var count = await users.CountAsync();
```

### Builder with AddRange

```csharp
using RzR.Extensions.EntityMock;

var initialUsers = new List<User>
{
    new User { Id = 1, Name = "Alice" },
    new User { Id = 2, Name = "Bob" }
};

var moreUsers = new[]
{
    new User { Id = 3, Name = "Charlie" },
    new User { Id = 4, Name = "Diana" }
};

var allUsers = AsyncEnumerableFactory.Builder<User>()
    .AddRange(initialUsers)
    .AddRange(moreUsers)
    .Add(new User { Id = 5, Name = "Eve" })
    .Build();

var total = await allUsers.CountAsync(); // Returns 5
```

### Reusable Builder

```csharp
var builder = new AsyncEnumerableBuilder<User>();

// Build first collection
builder.Add(new User { Id = 1, Name = "Alice" });
var collection1 = builder.Build();

// Clear (also resets fault settings) and build another collection
builder.Clear();
builder.Add(new User { Id = 2, Name = "Bob" });
var collection2 = builder.Build();
```

> Note: `Build()` snapshots the items at call time. Adding more items afterwards
> does **not** affect previously-built sequences.

---

## Fault Injection

The builder can inject realistic failure modes — useful for testing retry,
timeout, partial-stream and cancellation behavior in production code without
changing it.

### Per-item latency

```csharp
using RzR.Extensions.EntityMock;
using Microsoft.EntityFrameworkCore;

var slow = AsyncEnumerableFactory.Builder<User>()
    .AddRange(users)
    .WithDelay(TimeSpan.FromMilliseconds(50))
    .Build();

var list = await slow.ToListAsync(); // ~50ms per MoveNextAsync call
```

### Throw at a specific index

```csharp
var seq = AsyncEnumerableFactory.Builder<User>()
    .AddRange(users)
    .ThrowAfter(3, new DbUpdateException("forced"))
    .Build();

// Iteration yields the first 3 elements then throws DbUpdateException.
```

With an index-aware factory:

```csharp
var seq = AsyncEnumerableFactory.Builder<int>()
    .AddRange(new[] { 10, 20, 30 })
    .ThrowAfter(2, idx => new InvalidOperationException($"failed at {idx}"))
    .Build();
```

### Time-based cancellation

```csharp
var seq = AsyncEnumerableFactory.Builder<User>()
    .AddRange(users)
    .WithDelay(TimeSpan.FromMilliseconds(20))
    .CancelAfter(TimeSpan.FromMilliseconds(100))
    .Build();

try
{
    await foreach (var u in seq) { /* ... */ }
}
catch (OperationCanceledException)
{
    // Internal token expired after 100ms.
}
```

The internal `CancelAfter` token is **linked** with any caller-supplied
cancellation token, so either source aborts iteration:

```csharp
using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));

await foreach (var u in seq.WithCancellation(cts.Token))
{
    // Either the caller token (1s) or the builder's CancelAfter wins,
    // whichever fires first.
}
```

### Combining faults

```csharp
var slow = AsyncEnumerableFactory.Builder<User>()
    .AddRange(users)
    .WithDelay(TimeSpan.FromMilliseconds(50)) // latency
    .ThrowAfter(3, _ => new DbUpdateException("forced")) // failure
    .CancelAfter(TimeSpan.FromSeconds(1)) // timeout
    .Build();
```

---

## Advanced Scenarios

### Pagination

```csharp
using RzR.Extensions.EntityMock.Extensions;

var allUsers = GetAllUsers(); // Returns List<User>

int pageNumber = 2;
int pageSize = 10;

var pagedResult = await allUsers
    .ToMockAsyncEnumerable()
    .OrderBy(u => u.Name)
    .Skip((pageNumber - 1) * pageSize)
    .Take(pageSize)
    .ToListAsync();
```

### Complex LINQ Queries

```csharp
using RzR.Extensions.EntityMock.Extensions;

var users = GetUsers();

var result = await users
    .ToMockAsyncEnumerable()
    .Where(u => u.IsActive)
    .Where(u => u.Department == "IT")
    .OrderByDescending(u => u.CreatedDate)
    .Select(u => new 
    { 
        u.Name, 
        u.Email,
        DaysActive = (DateTime.Now - u.CreatedDate).Days
    })
    .Take(50)
    .ToListAsync();
```

### Grouping Operations

```csharp
using RzR.Extensions.EntityMock.Extensions;

var users = GetUsers().ToMockAsyncEnumerable();

var groupedByDepartment = await users
    .GroupBy(u => u.Department)
    .ToListAsync();

foreach (var group in groupedByDepartment)
{
    Console.WriteLine($"{group.Key}: {group.Count()} users");
}
```

### Aggregation

```csharp
using RzR.Extensions.EntityMock.Extensions;

var orders = GetOrders().ToMockAsyncEnumerable();

var totalRevenue = await orders
    .Where(o => o.Status == "Completed")
    .SumAsync(o => o.Amount);

var averageOrderValue = await orders.AverageAsync(o => o.Amount);

var maxOrder = await orders.MaxAsync(o => o.Amount);
```

### Projections

```csharp
using RzR.Extensions.EntityMock.Extensions;

var users = GetUsers().ToMockAsyncEnumerable();

var userDtos = await users
    .Select(u => new UserDto
    {
        FullName = $"{u.FirstName} {u.LastName}",
        Email = u.Email,
        IsActive = u.IsActive
    })
    .ToListAsync();
```

---

## Best Practices

### 1. Use Extension Methods for Readability

**Good:**
```csharp
var result = await users.ToMockAsyncEnumerable().ToListAsync();
```

**Less Readable:**
```csharp
var result = await EnumerableInvoker.Invoke(users).ToListAsync();
```

### 2. Dispose Resources Properly

```csharp
await using (var enumerator = asyncEnumerable.GetAsyncEnumerator())
{
    while (await enumerator.MoveNextAsync())
    {
        var current = enumerator.Current;
        // Process current item
    }
}
```

### 3. Use Appropriate Factory Methods

```csharp
// For empty collections
var empty = AsyncEnumerableFactory.Empty<User>();

// For single items
var single = AsyncEnumerableFactory.Single(user);

// For known items
var multiple = AsyncEnumerableFactory.Create(user1, user2, user3);

// For dynamic building
var dynamic = AsyncEnumerableFactory.Builder<User>()
    .Add(user1)
    .AddRange(moreUsers)
    .Build();
```

---


### Query Exceptions

```csharp
using RzR.Extensions.EntityMock.Extensions;

var users = GetUsers().ToMockAsyncEnumerable();

try
{
    // FirstAsync throws if no elements found
    var user = await users.FirstAsync(u => u.Id == 999);
}
catch (InvalidOperationException)
{
    // No matching user found
}

// Use FirstOrDefaultAsync to avoid exception
var userOrNull = await users.FirstOrDefaultAsync(u => u.Id == 999);
```

---

## Migration Guide

### From v1.x to v2.0

**Old Way (v1.x):**
```csharp
var asyncEnum = EnumerableInvoker.Invoke(users);
```

**New Way (v2.0 - Recommended):**
```csharp
using RzR.Extensions.EntityMock.Extensions;

var asyncEnum = users.ToMockAsyncEnumerable();
```

**New Features in v2.0:**
```csharp
// Factory methods
var empty = AsyncEnumerableFactory.Empty<User>();
var single  AsyncEnumerableFactory.Single(user);
var multiple = AsyncEnumerableFactory.Create(user1, user2);

// Builder pattern
var built = AsyncEnumerableFactory.Builder<User>()
    .Add(user1)
    .AddRange(users)
    .Build();

// Fault injection (latency, throw-at-index, time-based cancellation)
var faulty = AsyncEnumerableFactory.Builder<User>()
    .AddRange(users)
    .WithDelay(TimeSpan.FromMilliseconds(50))
    .ThrowAfter(3, new DbUpdateException("forced"))
    .CancelAfter(TimeSpan.FromSeconds(1))
    .Build();
```

### Namespace migration (v3.x)

The root namespace was renamed from `MockAsyncEnumerable` to
`RzR.Extensions.EntityMock`. Update your `using` directives:

| Old | New |
|-----|-----|
| `using MockAsyncEnumerable;` | `using RzR.Extensions.EntityMock;` |
| `using MockAsyncEnumerable.Extensions;` | `using RzR.Extensions.EntityMock.Extensions;` |

Public factory / builder / extension methods now return `IMockAsyncEnumerable<T>`
(from `RzR.Extensions.EntityMock.Abstractions`) instead of the concrete
`AsyncEnumerable<T>`. Code that stores the result in `var` is unaffected.

#### If you used the explicit type

If your code declared the variable explicitly as `AsyncEnumerable<T>`, the
upgrade is a **source-breaking** change and the declaration must be updated.
Pick whichever option fits your call site:

**1. Switch to the abstraction (recommended)**

```csharp
// Before (v1.x)
AsyncEnumerable<User> users = list.ToMockAsyncEnumerable();

// After (v2.x)
using RzR.Extensions.EntityMock.Abstractions;

IMockAsyncEnumerable<User> users = list.ToMockAsyncEnumerable();
```

**2. Use one of the standard interfaces**

`IMockAsyncEnumerable<T>` derives from both `IAsyncEnumerable<T>` and
`IQueryable<T>`, so either works directly without a cast:

```csharp
IAsyncEnumerable<User> usersAsync = list.ToMockAsyncEnumerable();
IQueryable<User> usersQuery = list.ToMockAsyncEnumerable();
```

**3. Cast back to the concrete type (not recommended)**

The concrete `AsyncEnumerable<T>` still lives in
`RzR.Extensions.EntityMock.Helpers`, so an explicit cast keeps old code
compiling:

```csharp
using RzR.Extensions.EntityMock.Helpers;

AsyncEnumerable<User> users = (AsyncEnumerable<User>)list.ToMockAsyncEnumerable();
```

> !!!️ Casting couples your code to the implementation type and will break again
> if the internal type ever changes. Prefer option 1 or 2.

**Method signatures, fields, and properties** that exposed `AsyncEnumerable<T>`
should be updated the same way -> replace the type with `IMockAsyncEnumerable<T>`
(or one of its base interfaces).
