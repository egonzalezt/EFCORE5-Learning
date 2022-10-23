# Updating Simple Objects

Making crud what do you need to update an object??

```csharp
var samurai = _context.Samurais.FirstOrDefault();
samurai.Name += "San";
_context.SaveChanges();
```

in this example, you get the first samurai store in your database and update it let's suppose that your first samurai has the name `egonzalezt` and you need to add the `San` on the name now our samurai name is `egonzaleztSan` but what occurs behind the query?

