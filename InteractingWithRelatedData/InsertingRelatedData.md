# Inserting Related Data

before making this operation first we need to have a relationship between two tables or objects, Samurai and Quotes is a one-to-many relationship so now we can make this operation.

```csharp
private static void InsertNewSamuraiWithManyQuotes()
{
    var samurai = new Samurai
    {
        Name = "Kyūzō",
        Quotes = new List<Quote> {
            new Quote {Text = "Watch out for my sharp sword!"},
            new Quote {Text="I told you to watch out for the sharp sword! Oh well!" }
        }
    };
    _context.Samurais.Add(samurai);
    _context.SaveChanges();
}
```

For this relationship first, we need to create a samurai and add to its properties their quotes as a list. 

It's very similar to adding a new quote or list of quotes to an existing samurai

```csharp
private static void AddQuoteToExistingSamuraiWhileTracked()
{
    var samurai = _context.Samurais.FirstOrDefault();
    samurai.Quotes.Add(new Quote
    {
        Text = "I bet you're happy that I've saved you!"
    });
    _context.SaveChanges();
}
```