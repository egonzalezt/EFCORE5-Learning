# Create your first App

Once you have set up your environment create a Visual project as a library 

IMAGE 001

IMAGE 002

Create a console application 

IMAGE 003

With this console application, you will have split your domain layer with your objects and business logic, Data access or infrastructure layer where EF CORE is located, and finally the presentation layer

Dbcontext needs to expose DbSets which become wrappers to the different types that you'll interact with while you're using the context.

How you define your Dbcontexts is important to how EF CORE treats your data at runtime, as well as how it's able to interact with your database and it can define how you use the model in your coding


_context.Database.EnsureCreated();
check if the database exists if not create the database

## Notes 

For practical uses on the EfcoreContext.cs will be located the database connection string does not use this practice on your real application for security reasons

```csharp
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
  optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=SamuraiDb;Trusted_Connection=True;");
}
```
