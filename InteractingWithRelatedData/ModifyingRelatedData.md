# Modifying Related Data

This operation can be made in two situations:

## Connected scenarios

DbContext is **aware** of all changes made to objects that is it tracking 

This example uses eager loading to get the second samurai with their quotes, after that if modify the first quote and save changes

```csharp
var samurai = _context.Samurais.Include(s => s.Quotes)
                      .FirstOrDefault(s => s.Id == 2);
samurai.Quotes[0].Text = "Did you hear that?";
_context.SaveChanges();
```

NOTE VIDEO 0:49 

and as you see on this query it just modify the quote, not the entire samurai or the other quotes related with that samurai

## Disconnected scenarios

DbContext **has no clue** about history of objects before they are attached.

Here the data or entities are being track after the changes has been made and thats is a big problem, specially with performance.

