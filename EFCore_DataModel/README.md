# SQL Build by EF CORE

## Dbset

A dbset is a repository for objects of a particular type for example person or vehicle or dog objects.

to make the different queries you need to use your dbsets for example for adding a samurai or removing it 

```csharp
_context.Samurais.Add(new Samurai{Name="egonzalezt"});
```

passing a new samurai to the contexts EF CORE the contexts are now aware of the samurai EF CORE with the dbcontext tracks the entities.

```csharp
_context.SaveChanges();
```

with the new Samurai, EF CORE is aware to insert it in the database.

EF CORE uses an internal type called EntityEntry to track these objects which has information about for example samurai object or person object and knows its current state in this case that a new object is added.

Now with the method, SaveChanges EFCORE will look at all of the different objects that the context is tracking and get its state, finally, this method converts the properties of the object and maps it into an SQL command depending on the SQL Engine provider to construct the correct SQL statement.

## Transactions

Finally looking at the SQL Server profiler(this is included with MS SSMS) EF CORE by default makes [transactions](https://learn.microsoft.com/en-us/ef/core/saving/transactions) the advantages of making transactions during the saving process or another process if something happens EFCORE will roll back the operations if needed. and always EF CORE can be configured to manage these transactions. For example, adding a new samurai EF CORE sets `set transaction isolation level read committed` which means that users cannot read this samurai until the transaction is done.

## Query Tags 
 
Another feature of tracking EF CORE actions is the query tags that add a comment to the generated SQL

```csharp
var samurais = _context.Samurais
                .TagWith("Getting all the samurais saved on the database")
                .ToList();
```
![image](https://user-images.githubusercontent.com/53051438/197365740-0566d74e-ed96-4460-900d-e50b4c7e29a5.png)

EF CORE adds a double dash to add these comments so you don't have to worry about a SQL injection attack.

## Logging 

Now using profiling tools like SQL Server Profiler is not always a good option but EF CORE can log their queries using .NET CORE: Microsoft.Extensions.Logging. Now the reason to add logging to .NET is to track and keep control of how EF CORE Build the different queries because if you have a battle of samurais with 1.000.000 or more samurais and you need just to get samurais with some characteristics is not a good idea that EF CORE create queries like this:

```SQL
SELECT * FROM BATTLES WHERE BATTLES.DATE == 24/09/1877
``` 

getting 1 million samurais may impact the performance of your application

logging with console applications is required to add extra code on the dbcontext of the samurai application using applications that work on ASP.NET CORE logging is already built in but you will add just a little bit extra to the ASP.NET Core logic.

but this can be [configured](https://www.entityframeworktutorial.net/efcore/logging-in-entityframework-core.aspx) there are different ways to log and depends on the target if you are using a library to log or the builtin options 

[Microsoft Example](https://learn.microsoft.com/en-us/ef/core/logging-events-diagnostics/simple-logging)

```csharp
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=SamuraiDb;Trusted_Connection=True;")
        .LogTo(Console.WriteLine);
}
```

or 

```csharp
private StreamWriter _writer = new StreamWriter("EFCORELog.txt",append:true);

protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=SamuraiDb;Trusted_Connection=True;")
        .LogTo(_writer.WriteLine);
}
```

or

```csharp
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=SamuraiDb;Trusted_Connection=True;")
        .LogTo(log => Debug.WriteLine(log));
}
```

these examples log all the different information that EF CORE makes that could be unnecessary because maybe you need to log just the build queries or the error logs for that reason Microsoft provides different message categories [Documentation](https://learn.microsoft.com/en-us/ef/core/logging-events-diagnostics/simple-logging#message-categories)

Finally, logging requires setting a level because logging your queries to the user could be not a good option because this makes your application more vulnerable, log queries are required in a developer environment [setup log levels](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging#configure-logging) and [types of log levels](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.loglevel?view=dotnet-plat-ext-6.0)

```csharp
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder.UseSqlServer(" ")
        .LogTo(Console.WriteLine, new[] {DbLoggerCategory.Database.Command.Name}, Microsoft.Extensions.Logging.LogLevel.Information );
}
```

with this example this is the result:

![image](https://user-images.githubusercontent.com/53051438/197368976-e09e63a3-1869-4c62-b005-9143172fe1a6.png)
