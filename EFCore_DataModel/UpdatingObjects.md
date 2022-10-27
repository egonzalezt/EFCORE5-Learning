# Updating 

## Simple Objects
Making crud what do you need to update an object??

```csharp
var samurai = _context.Samurais.FirstOrDefault();
samurai.Name += "San";
_context.SaveChanges();
```

in this example, you get the first samurai store in your database and update it let's suppose that your first samurai has the name `egonzalezt` and you need to add the `San` on the name now our samurai name is `egonzaleztSan` but what occurs behind the query?

![update](https://user-images.githubusercontent.com/53051438/197415400-7ef44481-3b9f-4db1-a49c-f42b15806ac1.png)

First EF CORE makes a `SELECT` statement to get the samurai and then updates the samurai, but remember Entity Entry? EF CORE uses to track the object changes and perform the required SQL script updating just the required information, not all the objects just the required part is the name of the samurai making the query more simple and shorter. 

## With batch

So now a group of samurais needs to be renamed with the `San` at the end of its names.

```csharp
private static void RetrieveAndUpdateMultipleSamurais()
{
    var samurais = _context.Samurais.Skip(1).Take(4).ToList();
    samurais.ForEach(s => s.Name += "San");
    _context.SaveChanges();
}
```

go to [Wait](#wait) to get more information about Skip() and Take().

Now with the ForEach method, we loop through the 4 samurais that we take and update their names using a lambda function, Finally, save the changes into the database, and again with the Entity Entry it tracks the objects and knows which parameters need to be updated and with parameters are going to be skipped.

Also, save changes can make different operations like updating an object and inserting a new object you can do that without any worry about getting an error.

### Wait

What is Skip() and Take()??

these two methods are used to omit the first N rows and take the next M rows 

for example we know that the first samurai `egonzalezt` now has the name `egonzaleztSan` we don't need to update `egonzaleztSan` again for that reason we Skip(1) the first samurai and Take(4) samurais these methods are great for paging data. 