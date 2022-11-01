# Bulk Operations

If we want to add two samurais EF CORE makes two different queries to perform the same operation, SQL server provider by default requires a minimum of four operations to trigger the bulk support 

with bulk operations now we can add a group of samurais without making N SQL queries just making one query to add that group 

Before

![image](https://user-images.githubusercontent.com/53051438/197402391-a6bb3846-5e2d-4695-b145-3bddf177fde6.png)

After

![image](https://user-images.githubusercontent.com/53051438/197402429-792d3bc2-3c11-492a-b256-46e4c8601d60.png)

Now with bulk operations adding multiple samurais was performed with a merge join adding multiple samurais in just one query

EF CORE can support the process to add different types of objects in the same query 

* With the same type

```csharp
_context.Samurais.AddRange(
    new Samurai { Name = "Shimada" },
    new Samurai { Name = "Okamoto" });
_context.Battles.AddRange(
    new Battle { Name = "Battle of Anegawa" },
    new Battle { Name = "Battle of Nagashino" });
```

* With different type

```csharp
_context.AddRange(new Samurai { Name = "Shimada" },
    new Samurai { Name = "Okamoto" },
    new Battle { Name = "Battle of Anegawa" },
    new Battle { Name = "Battle of Nagashino" });
```

by default EF CORE SQL server provider has a max Batch size of 42 and a minimum of 4

## Batch Operation Batch Size

* Default size and other configurations are set by default by the database provider.

* Additional Commands will be sent in extra batches.

    For example, if the commands exceed the SQL server's maximum batch size of 42 if your query exceeds that number it breaks the commands into batches.

* Override batch size in DbContext OnConfiguring

```csharp
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=SamuraiDb;Trusted_Connection=True;",
    options => options.MaxBatchSize(150))
    .LogTo(Console.WriteLine, new[] {DbLoggerCategory.Database.Command.Name}, LogLevel.Information );
}
```