# Loading Related Data

Eager Loading is great for querying graphs, but sometimes you have an object in memory let's suppose a Samurai but you need to get its related data like the horse or maybe the quotes, you can execute another query and let the change tracker take care of getting that data.

but there are two other ways to perform this task

* Explicit Loading
* Lazy Loading

these options perform an operation called loading 

## Explicit Loading

This way as the name says is done explicitly through the context using the `DbContext.Entry` method you pass the object that you already have in memory and you have two options:

* Get a collection property like quotes

```csharp
DbContext.Entry().Collection().Load();
```

```csharp
DbContext.Entry(samurai).Collection(s => s.Quotes).Load();
```

* Get a reference properties like horse

```csharp
DbContext.Entry().Reference().Load();
```

```csharp
DbContext.Entry(samurai).Reference(s => s.Horse).Load();
```

### Keep in mind

You can only load from a single object Explicit Loading cannot perform this operation with multiple object.

You can filter loaded data using query method

```csharp
var savedQuotes = context.Entry(samurai).
    .Collection(b => b.Quotes)
    .Query()
    .Where(q => q.Quote.Contains("saved")
    .ToList();
```
