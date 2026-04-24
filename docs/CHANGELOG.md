### **v3.0.0.3872** [[RzR](mailto:108324929+I-RzR-I@users.noreply.github.com)] 24-04-2026
**Breaking changes**
* [DEV] - (RzR) -> **Namespace** - Root renamed `MockAsyncEnumerable` -> `RzR.Extensions.EntityMock` (sub-namespaces: `.Abstractions`, `.Extensions`, `.Faults`, `.Helpers`). Update all `using` directives;
* [DEV] - (RzR) -> **Return type** - Factory / builder / `ToMockAsyncEnumerable()` now return `IMockAsyncEnumerable<T>` instead of the concrete `AsyncEnumerable<T>`. Code using `var` is unaffected; explicit declarations must switch to the interface (or cast — see migration guide);

**New features**
* [DEV] - (RzR) -> **Abstraction** - New `IMockAsyncEnumerable<T> : IAsyncEnumerable<T>, IQueryable<T>` in `RzR.Extensions.EntityMock.Abstractions`;
* [DEV] - (RzR) -> **Fault Injection** - New builder fluent API: `WithDelay(TimeSpan)`, `ThrowAfter(int, Exception)`, `ThrowAfter(int, Func<int, Exception>)`, `CancelAfter(TimeSpan)` (linked with caller's cancellation token);
* [DEV] - (RzR) -> **Builder** - `Build()` snapshots items at call time; `Clear()` also resets fault settings;

**Deprecations**
* [DEV] - (RzR) -> `EnumerableInvoker` marked `[Obsolete]`. Use `AsyncEnumerableFactory`, `ToMockAsyncEnumerable()`, or `AsyncEnumerableBuilder<T>`;

### **v2.0.0.6582** [[RzR](mailto:108324929+I-RzR-I@users.noreply.github.com)] 13-03-2026
* **Factory Pattern** - New `AsyncEnumerableFactory` class with fluent creation methods: `Empty<T>()`, `Single<T>(item)`, `Create<T>(params T[])`, `Builder<T>()`;
* **Builder Pattern** - New `AsyncEnumerableBuilder<T>` for fluent construction: `Add(item)`, `AddRange(items)`, `Build()`, `Clear()`;
* **Extension Methods** - New `AsyncEnumerableExtensions` class with methods: `ToMockAsyncEnumerable()`;
* **Improvements**: Null Guard Protection, Better Async Patterns, Cancellation Token Support, Improved Disposal, Error Handling, XML Documentation

### **v1.0.3.6469**
-> Add support for (netstandard2.0, net8.0) and minimum requirement for supported frameworks;

### **v1.1.0.0**
-> Add support for (net9.0) and minimum requirement for supported frameworks; <br/>
