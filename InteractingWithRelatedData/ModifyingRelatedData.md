# Modifying Related Data

This operation can be made in two situations:

## Connected scenarios

DbContext is **aware** of all changes made to objects that is it tracking 

This example uses eager loading to get the second samurai with their quotes, after that if modify the first quote and save changes

```csharp
var samurai = _context.Samurais.Include(s => s.Quotes)
                      .FirstOrDefault(s => s.Id == 2);
samurai.Quotes[0].Text += "Did you hear that?";
_context.SaveChanges();
```

![image](https://user-images.githubusercontent.com/53051438/198298446-95f3bfbd-7b73-41dc-a71b-7bda2ce38b1d.png)

and as you see on this query it just modify the quote, not the entire samurai or the other quotes related to that samurai

## Disconnected scenarios

DbContext **has no **clue** about the history of objects before they are attached.

Here the data or entities are being tracked after the changes have been made and that is a big problem, especially with performance.

In this example get again the second samurai and modify the quote and that's all then use a new context and update that quote

```csharp
var samurai = _context.Samurais.Include(s => s.Quotes)
                      .FirstOrDefault(s => s.Id == 2);
var quote = samurai.Quotes[0];
quote.Text += "Did you hear that?";

using var newContext = new EFCoreContext();
newContext.Quotes.Update(quote);
newContext.SaveChanges();
```

![image](https://user-images.githubusercontent.com/53051438/198299288-080911ae-ece9-4220-8150-c44c4d75e7f1.png)

But this query is massive just for saving a new samurai

this query updates all the quotes and it just needs to update just the first quote, the reason is that the quote is still attached to one samurai and this samurai has a set of quotes and that's why all the quotes are being updated.

Using `Attach()` doesn't work because the entire object is going to be marked as untrack so again we create the same query, this problem could be solved using attach just by adding a new quote not modifying it.

### Possible Solution

we need to separate that quote from the group of quotes and the samurai instead of using update or save methods is required to use the DbSet `Entry` method that gives you more control. this method focuses specifically on the entry that you pass in, the quote in this case ignoring anything else that might be attached to it.

```csharp
var samurai = _context.Samurais.Include(s => s.Quotes)
                      .FirstOrDefault(s => s.Id == 2);
var quote = samurai.Quotes[0];
quote.Text += "Did you hear that?";

using var newContext = new EFCoreContext();
newContext.Entry(quote).State = EntityState.Modified;
newContext.SaveChanges();
```

using `Entry().State` we are modifying the state of that quote telling EF CORE that this object is modified and needs to be updated or saved into the database to make it possible that, you need to use one of the Enums of EntityState `EntityState.Modified` now EF CORE tracks just this quote and not the set of quotes and the samurai.

![image](https://user-images.githubusercontent.com/53051438/198301809-81e110ba-6d71-41ba-bd3f-5b96691c0999.png)

And now the query is updating just the required quote.
