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

## Disconnected scenario

Now what happen on a disconnected scenario?

```csharp
var samurai = _context.Samurais.Find(samuraiId);
samurai.Quotes.Add(new Quote
{
    Text = "Now that I saved you, will you feed me"
});

using(var newContext = new SamuraiContext())
{
    newContext.Samurais.Update(samurai);
    newContext.SaveChanges();
}
```

EF CORE know that a samurai is already added because has an ID and determines that the quote is new because doesn't have and ID and assume that the child FK value is the parent's key but there is a problem with this strategy

![image](https://user-images.githubusercontent.com/53051438/197861910-6c205457-87e7-4e61-bab2-5cb440f82649.png)

It's updating the samurai and thats logic because we are inserting the new quote. but this update could be a performance problemm, for that reason there is a new way to add a new quote without updating the samurai.

DbContext methods

* Add
* Update
* Remove
* **Attach**

Using the attach method, connects the object and sets its state to unmodified also EF CORE detect the missing key and foreign key and fix that up.

```csharp
var samurai = _context.Samurais.Find(samuraiId);
samurai.Quotes.Add(new Quote
{
    Text = "Now that I saved you, will you feed me"
});

using(var newContext = new SamuraiContext())
{
    newContext.Samurais.Attach(samurai);
    newContext.SaveChanges();
}
```

And now there is no update 

![image](https://user-images.githubusercontent.com/53051438/197863191-841cfcbe-922c-4fce-9336-8c358514024c.png)

![image](https://user-images.githubusercontent.com/53051438/197863273-93db8f4c-0c15-4023-b6b9-0280a19651db.png)

But there more another way that is much simpler than the last options is just using the foreign keys defined on the child object making more easier to perform the insert quote operation

```csharp
public void InsertQuoteToSamurai(int samuraiId)
{
    var quote = new Quote { Text ="Yeah more easier!!", SamuraiId = samuraiId}
    using var newContext = new SamuraiContext();
    newContext.Quotes.Add(quote);
    newContext.SaveChanges();
}
```

using this strategy makes the code more short and easier because we are just adding a new quote without getting the samurai just its ID.

![image](https://user-images.githubusercontent.com/53051438/197864575-62cf9407-e640-4a24-b8ee-9261ebe73a26.png)
