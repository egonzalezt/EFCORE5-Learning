# Testing

[Microsoft Choosing a testing strategy](https://learn.microsoft.com/en-us/ef/core/testing/choosing-a-testing-strategy)

## Common types of automated tests

* Unit test

Test small units of your code

* Integration test

Test that your logic interacts with other services or modules

* Functional Test

Verify results of interaction

### Testing EF CORE directly or indirectly

* Validate your DbContext against the database
    Db return a Pk of a newly inserted record.
    Do queries, include bringing back graphs of data expected?

* Validate your business logic against the DbContext

* Validate your business logic that uses the DbContext and database

    you can make some business logic or complete logic but in some scenarios hitting the database when testing is undesirable it could slow things down or if the database isn't there, the test doesn't care about that but it would impact the test. the best way to avoid these situations is to use a different provider call in-memory database.

## Building your first test

Before testing your database take care of which database are you working on, working with your actual database can damage or disrupt your actual data before you need to change your connection string.

```csharp
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    if (!optionsBuilder.IsConfigured)
    {
        optionsBuilder.UseSqlServer(
        "Server=localhost\\SQLEXPRESS;Database=SamuraiDbTesting;Trusted_Connection=True;");
    }
}
```

finally, add a new Unit testing project, and add a new reference to the domain and infrastructure project making our first tests remember to **change your** database connection string** because our tests perform these operations to have a clean and new database

```csharp
context.Database.EnsureDeleted();
context.Database.EnsureCreated();
```

this test verifies that a new samurai is created and check if the database auto-assigns a new PK to the samurai

```csharp
using (var context=new SamuraiContext())
{
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();
    var samurai = new Samurai();
    context.Samurais.Add(samurai);
    Debug.WriteLine($"Before save: {samurai.Id}");
    context.SaveChanges();
    Debug.WriteLine($"After save: {samurai.Id}");
    Assert.AreNotEqual(0, samurai.Id);

}
```

if you have enabled the logs EF CORE will log the queries and other messages printed with `Debug.WriteLine()`

### Choose your engine or database to be tested

* Use your project database

    Needs to test specific behaviors of the target database.

* SqlLite or SQL CE

    Need to test generic SQL Database behaviors

* EF CORE in memory Database

    Need to test EF CORE behavior or biz logic that uses EF CORE

## Use In-Memory Database

This provider emulates to be a relational database 

* Emulates RDBMS via In-Memory lists

* Handles generic RDBMS scenarios

* Great alternative for test mock

* Requires mods to our existing solution

You need to add the package `Microsoft.EntityFrameworkCore.InMemory` then made some changes to the context.

first, we are making the context more flexible enough to let you determine which provider to use when instantiating the constructor elsewhere.

```csharp
public EfcoreContext(DbContextOptions opt)
    : base(opt)
{ }
```

Then add the default constructor because this is going to be used by the SQL Server provider, which overrides the constructor inherited from the object class.

```csharp
public EfcoreContext()
{ }
```

Finally, use the options builder to have more flexibility so if in our tests we add an in-memory database the code will omit the configuration of our normal database this will cause that we don't need to modify our connection string 

```csharp
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    if(!optionsBuilder.IsConfigured)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=SamuraiDb;Trusted_Connection=True;",
                options => options.MaxBatchSize(150))
                .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information);
    }
}
```

### Build your first In-Memory database tests

First, add an [In-Memory DataBase provider](https://learn.microsoft.com/en-us/ef/core/providers/in-memory/?tabs=dotnet-core-cli)

First, you need to create your DbContextsOptionsBuilder and specify that you are going to work with an In-Memory database.

```csharp
var builder = new DbContextOptionsBuilder();
            builder.UseInMemoryDatabase("ConnStr");
```

now this database has a connection string in this case `ConnStr` but the main reason for this option is to give EF CORE the possibility to create different databases if you want a different database per unit test, and you can create a different test with the same connection string it will use the same database, this is the way how EF CORE tracks our database.

```csharp
public void Test01()
{
    builder.UseInMemoryDatabase("Test01DB")
}
```
This is a fresh, empty and unique in-memory database

```csharp
public void Test01()
{
    builder.UseInMemoryDatabase("GlobalDb")
}

public void Test02()
{
    builder.UseInMemoryDatabase("GlobalDb")
}
```
this is a fresh database but will get reused in the second method `Test02()`

#### Setup Context

now that we create our in-memory database we need to pass this database to our context thanks to the changes before [Building your](#building-your-first-test) first test](#building-your-first-test) just passes the options to the context.

```csharp
var builder = new DbContextOptionsBuilder();
builder.UseInMemoryDatabase("TestConnectionString");
using (var context = new EfcoreContext(builder.Options))
{
    var samurai = new Samurai();
    context.Samurais.Add(samurai);
    Assert.AreEqual(EntityState.Added,
    context.Entry(samurai).State);
}
```

this test ensures that EF CORE adds to your database a new samurai checking if the state of the entity is set as added

```csharp
    Assert.AreEqual(EntityState.Added,
    context.Entry(samurai).State);
```

## Keep in mind

EF CORE's raw SQL methods only work with relational database providers, which means that doesn't work with an in-memory database.

