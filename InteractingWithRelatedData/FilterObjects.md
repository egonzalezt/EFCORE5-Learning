# Related Data to Filter Objects

Now imagine that you wanna get all the samurais that have quotes that contain the word "saved" but you don't wanna get the quotes just the samurais. 

```csharp
var samurais = _context.Samurais
        .Where(s => s.Quotes.Any(q => q.Text.Contains("saved"))).ToList();
```