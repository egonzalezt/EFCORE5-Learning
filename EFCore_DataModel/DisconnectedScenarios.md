# Disconnected Scenarios

If you have an application with has access to the database on the same local network or is a monolithic application where the database lives on the application is a Connected scenario but when your app needs to get and send data over the public network or cloud databases is called disconnected scenarios.

Example: Web application DbContext on the server can't track what's happening in a user web browser.

"In disconnected scenarios, it's up to you to inform the context about object state"

## Persisting data in Disconnected scenarios

Let's simulate a disconnected scenario

```csharp
private static void QueryAndUpdateBattles_Disconnected()
{
    List<Battle> disconnectedBattles;
    using (var context1 = new EfcoreContext())
    {
        disconnectedBattles = _context.Battles.ToList();
    } //context1 is disposed
    disconnectedBattles.ForEach(b =>
    {
        b.StartDate = new DateTime(1570, 01, 01);
        b.EndDate = new DateTime(1570, 12, 1);
    });
    using (var context2 = new EfcoreContext())
    {
        context2.UpdateRange(disconnectedBattles);
        context2.SaveChanges();
    }
}
```

with the keyword `using` the context is being disposed making EF CORE lose track of its objects when the newly updated objects are being saved EF CORE doesn't know with parameters changed for that reason it starts tracking these objects but when an update operation is performed EF CORE just update all its parameters.

[Good course for Enterprise applications](https://app.pluralsight.com/library/courses/entity-framework-enterprise-update/table-of-contents)

## [No Track Queries and DbContext](https://learn.microsoft.com/en-us/ef/core/querying/tracking)

Using Disconnected scenarios is not a good idea to track queries and context because is expensive because this context is disposed of when the operation is done for that reason EF CORE allows you to disable the tracking.

Ways to disable tracking

* On the queries

    ```csharp
    var samurai = _context.Samurais.AsNoTracking.FirstOrDefault()
    ```

* On DbContext

    All queries on SamuraiContextNoTracking will default to no tracking using DbSet.AsTracking() for special queries to be tracked

    ```csharp
    public class SamuraiContextNoTracking : SamuraiContext
    {
        public SamuraiContextNoTracking()
        {
            base.ChangeTracker.QueryTrackingBehavior =
             QueryTrackingBehavior.NoTracking;
        }
    
    }
    ```

    Note: using the SamuraiContextNoTracking class will keep all the mappings, logging and other configuration made on the original DbContext.

### Example

```csharp
class Program
{
    private static SamuraiContext _context = new SamuraiContextNoTracking();

    private static void Main(string[] args)
    {
        GetSamurais();
    }
    private static void GetSamurais()
    {
        var samurais = _context.Samurais
            .TagWith("ConsoleApp.Program.GetSamurais method")
            .ToList();
        Console.WriteLine($"Samurai count is {samurais.Count}");
        foreach (var samurai in samurais)
        {
            Console.WriteLine(samurai.Name);
        }
    }
}
```

Debugging the application with QuickWatch using the expression `_context.ChangeTracker.Entries(), results` will return a result view Empty, and the different queries are being executed normally.