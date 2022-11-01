# Best Practices

During these different wiki sections you discover different ways to create queries, make CRUD operations and use other operations like selecting specific parameters or custom data, filtering, bulk operations or just working with RAW SQL, stored procedures, etc.

Now these are some suggested recommendations for implementing EF CORE

## Operations

If you need to perform some CRUD operation or another query operation to the context do not pass the context through your application because this violates the Single Responsibility principle.

if you are building an API the controller just receives the request, then call another method to do the required validations and other operations like storing to the database and returning the result to the user, the controller itself do not perform EF CORE operations, Validations or data processing.

our application on the main makes all the different operations like that:

```csharp
class Program
{
    private static EfcoreContext _context = new EfcoreContext();
    private static void Main(string[] args)
    {
        InsertNewSamuraiWithAQuote();
        InsertNewSamuraiWithManyQuotes();
    }

    private static void InsertNewSamuraiWithAQuote()
    {
    var samurai = new Samurai
    {
        Name = "Kambei Shimada",
        Quotes = new List<Quote>
        {
          new Quote { Text = "I've come to save you" }
        }
            };
        _context.Samurais.Add(samurai);
        _context.SaveChanges();
        }
    private static void InsertNewSamuraiWithManyQuotes()
    {
        var samurai = new Samurai
        {
            Name = "Kyūzō",
            Quotes = new List<Quote> 
            {
                new Quote {Text = "Watch out for my sharp sword!"},
                new Quote {Text="I told you to watch out for the sharp sword! Oh well!" }
            }
        };
        _context.Samurais.Add(samurai);
        _context.SaveChanges();
    }
}
```

This is not the best option for that reason a new class is being created called `BusinessDataLogic.cs` on this class all the EF CORE logic is handled but for a better structure is recommended to create a different class that handles only one table for example `SamuraiRepository.cs`, `BattleRepository.cs`

Thanks to that this is how it looks the new main

```csharp
internal class Program
{
    private static void Main(string[] args)
    {
        AddSamuraisByName("Shimada", "Okamoto", "Kikuchio", "Hayashida");
    }

    private static void AddSamuraisByName(params string[] names)
    {
        var _bizData = new BusinessDataLogic();
        var newSamuraisCreatedCount = _bizData.AddSamuraisByName(names);
    }
}
```

## Logs

Is important to log your different queries because this way you can track how EF CORE is creating your queries and improve it some of them can update the whole table but just is required to modify one parameter

## Disconnected vs Connected scenarios

Is important when the context tracks your objects some applications like APIs sent the object and lose its track or sometimes it just gets the data and disposes of the context tracking your context is expensive and could impact your app performance if your code gets - track - dispose of many times.

## Security

Never put your connection string as a burned string on your DbContext 

```csharp
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
            
    optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=SamuraiDb;Trusted_Connection=True;",
    options => options.MaxBatchSize(150))
    .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information);
}
```

Better use other approaches like using the connection strings from appsettings.json look at this [example](https://www.connectionstrings.com/store-and-read-connection-string-in-appsettings-json/)](https://www.connectionstrings.com/store-and-read-connection-string-in-appsettings-json/)

* Another way is to use [IConfiguration interface](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.configurationextensions.getconnectionstring?view=dotnet-plat-ext-6.0)

* Also is good to apply the [Options Pattern](https://code-maze.com/aspnet-configuration-options/)