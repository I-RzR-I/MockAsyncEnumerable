### **v2.0.0.6582** [[RzR](mailto:108324929+I-RzR-I@users.noreply.github.com)] 13-03-2026
* **Factory Pattern** - New `AsyncEnumerableFactory` class with fluent creation methods: `Empty<T>()`, `Single<T>(item)`, `Create<T>(params T[])`, `Builder<T>()`;
* **Builder Pattern** - New `AsyncEnumerableBuilder<T>` for fluent construction: `Add(item)`, `AddRange(items)`, `Build()`, `Clear()`;
* **Extension Methods** - New `AsyncEnumerableExtensions` class with methods: `ToMockAsyncEnumerable()`;
* **Improvements**: Null Guard Protection, Better Async Patterns, Cancellation Token Support, Improved Disposal, Error Handling, XML Documentation

### **v1.0.3.6469**
-> Add support for (netstandard2.0, net8.0) and minimum requirement for supported frameworks;

### **v1.1.0.0**
-> Add support for (net9.0) and minimum requirement for supported frameworks; <br/>
