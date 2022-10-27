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

* Get a reference properties like a horse

```csharp
DbContext.Entry().Reference().Load();
```

```csharp
DbContext.Entry(samurai).Reference(s => s.Horse).Load();
```

### Example

Before running the code on our database we just get these samurais 

![image](https://user-images.githubusercontent.com/53051438/198177556-0765b883-2e14-468d-be7e-3cb7aaed4111.png)

running this code we add egonzalezt San horse

![image](https://user-images.githubusercontent.com/53051438/198178257-3917e510-78ef-42e6-8907-4b02288df52f.png)

```csharp
_context.Set<Horse>().Add(new Horse { SamuraiId = 1, Name = "Mr. Ed" });
_context.SaveChanges();
_context.ChangeTracker.Clear();
```

then we clean the context and get again just the first samurai (egonzalezt San) but there is a problem where is the horse? at this moment Quick Watch shows us that `Horse = null`

![image](https://user-images.githubusercontent.com/53051438/198177849-21c6916e-d1c9-4537-95af-0eb2ed846f4d.png)

```csharp
var samurai = _context.Samurais.Find(1);
```

the way to get the quotes and the horse is by using explicit load

```csharp
_context.Entry(samurai).Collection(s => s.Quotes).Load();
_context.Entry(samurai).Reference(s => s.Horse).Load();
```
and now egonzalezt San get their horse reference

![image](https://user-images.githubusercontent.com/53051438/198178187-fe83598f-1f57-4d63-9915-4a417fb448cc.png)

#### Final Code

```csharp
private static void ExplicitLoad()
{
    //Create a horse
    _context.Set<Horse>().Add(new Horse { SamuraiId = 1, Name   = "Mr. Horse" });
    _context.SaveChanges();
    _context.ChangeTracker.Clear();

    //Explicit Loading
    var samurai = _context.Samurais.Find(1);
    _context.Entry(samurai).Collection(s => s.Quotes).Load();
    _context.Entry(samurai).Reference(s => s.Horse).Load();
}
```

### Keep in mind

You can only load from a single object Explicit Loading cannot perform this operation with multiple objects.

You can filter loaded data using the query method

```csharp
var savedQuotes = context.Entry(samurai).
    .Collection(b => b.Quotes)
    .Query()
    .Where(q => q.Quote.Contains("saved")
    .ToList();
```

## Lazy Loading

Lazy Loading is easy to accidentally abuse and create performance problems or get unexpected results

By default, Lazy Loading is disabled 

![image](https://user-images.githubusercontent.com/53051438/198180356-f21ef236-2d8f-4b2e-a70a-15338c55c007.png)

![image](https://user-images.githubusercontent.com/53051438/198180407-bf30f6f1-d297-4c30-98ce-7bea8aa262c0.png)
