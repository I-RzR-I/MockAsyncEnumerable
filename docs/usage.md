# USING

This doc provides comprehensive examples for using MockAsyncEnumerable in your projects.

## Table of Contents

- [Installation](#installation)
- [Basic Usage](#basic-usage)
- [Extension Methods](#extension-methods)
- [Factory Pattern](#factory-pattern)
- [Builder Pattern](#builder-pattern)
- [Advanced Scenarios](#advanced-scenarios)
- [Best Practices](#best-practices)

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

### Using EnumerableInvoker

The simplest way to convert a synchronous collection to async enumerable:

```csharp
using MockAsyncEnumerable;
using Microsoft.EntityFrameworkCore;

var users = new List<User>
{
    new User { Id = 1, Name = "Alice", Age = 30 },
    new User { Id = 2, Name = "Bob", Age = 25 },
    new User { Id = 3, Name = "Charlie", Age = 35 }
};

var asyncEnumerable = EnumerableInvoker.Invoke(users);

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
using MockAsyncEnumerable.Extensions;
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
using MockAsyncEnumerable.Extensions;

var query = users.AsQueryable().Where(u => u.IsActive);
var asyncQuery = query.ToMockAsyncEnumerable();

var activeUsers = await asyncQuery.ToListAsync();
```

### Convert Arrays to Async Enumerable

```csharp
using MockAsyncEnumerable.Extensions;

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
using MockAsyncEnumerable;

var empty = AsyncEnumerableFactory.Empty<User>();
var count = await empty.CountAsync(); // Returns 0
```

### Create Single Item Async Enumerable

```csharp
using MockAsyncEnumerable;

var singleUser = AsyncEnumerableFactory.Single(new User { Id = 1, Name = "Alice" });
var user = await singleUser.FirstAsync(); // Returns the single user
```

### Create from Multiple Items

```csharp
using MockAsyncEnumerable;

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
using MockAsyncEnumerable;

var builder = AsyncEnumerableFactory.Builder<User>();

builder.Add(new User { Id = 1, Name = "Alice" });
builder.Add(new User { Id = 2, Name = "Bob" });

var users = builder.Build();
var result = await users.ToListAsync();
```

### Fluent Builder Chain

```csharp
using MockAsyncEnumerable;

var users = AsyncEnumerableFactory.Builder<User>()
    .Add(new User { Id = 1, Name = "Alice" })
    .Add(new User { Id = 2, Name = "Bob" })
    .Build();

var count = await users.CountAsync();
```

### Builder with AddRange

```csharp
using MockAsyncEnumerable;

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

// Clear and build another collection
builder.Clear();
builder.Add(new User { Id = 2, Name = "Bob" });
var collection2 = builder.Build();
```

---

## Advanced Scenarios

### Pagination

```csharp
using MockAsyncEnumerable.Extensions;

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
using MockAsyncEnumerable.Extensions;

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
using MockAsyncEnumerable.Extensions;

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
using MockAsyncEnumerable.Extensions;

var orders = GetOrders().ToMockAsyncEnumerable();

var totalRevenue = await orders
    .Where(o => o.Status == "Completed")
    .SumAsync(o => o.Amount);

var averageOrderValue = await orders.AverageAsync(o => o.Amount);

var maxOrder = await orders.MaxAsync(o => o.Amount);
```

### Projections

```csharp
using MockAsyncEnumerable.Extensions;

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
using MockAsyncEnumerable.Extensions;

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
using MockAsyncEnumerable.Extensions;

var asyncEnum = users.ToMockAsyncEnumerable();
```

**New Features in v2.0:**
```csharp
// Factory methods
var empty = AsyncEnumerableFactory.Empty<User>();
var single = AsyncEnumerableFactory.Single(user);
var multiple = AsyncEnumerableFactory.Create(user1, user2);

// Builder pattern
var built = AsyncEnumerableFactory.Builder<User>()
    .Add(user1)
    .AddRange(users)
    .Build();
```
