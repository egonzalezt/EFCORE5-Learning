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
    Do queries with includes bringing back graphs of data expected?

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