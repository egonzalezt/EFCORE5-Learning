# Persisting data in One-To-One 

On our example samurai has a navegation property `public Horse Horse {get;set;}` and horse has a `public int SamuraiId {get;set;}` property that is one of the easiest ways to build this relationship

EF CORE infers a unique FK conbstrain for the samurai fk column in the database that way the database ensures that you don't have more than one horse connected to any one samurai, if you add a new horse to a samurai the database verify the unique constrain not EF CORE if there is a new hore already added the DataBase will throw an error.

* Add a horse with Samurai Id

  ```csharp
  var horse = new Horse {Name = "Scout", SamuraiId = 2};
  _context.Add(horse);
  _context.SaveChanges();
  ```
* Add a horse with Samurai

  ```csharp
  var samurai = _context.Samurais.Find(3);
  samurai.Horse = new Horse { Name = "horseshoe"};
  _context.SaveChanges();
  ```
  
* Disconnected Scenario
  
  using attach to just save the horse and set its id on the samurai and not update the entire object

  ```csharp
  var samurai = _context.Samruais.AsNoTracking().FirstOrDefault(s => s.Id == 5);
  samurai.Horse = new Horse ( Name = "Ed" );
  
  using var newContext = new EfcoreContext();
  newContext.Samurais.Attach(samurai)
  newContext.SaveChanges();
  ```
## Changing the child of an existing parent

* Is FK nullable?

  It depends on your business for example a horse cannot exists without a samurai because its id is set as an int that is non-nullable

* Is the child object in memory?

  Whether or not the object you're replacing is in memory will also affect the behavior of how you can make these changes.

* Are the objects being tracked?

  Depending if you are working on connected or disconnected scenarios depending on your application and the impact on performance that it could have
  
## Querying One-To-One 

in our example the horse doesn't have any DbSet making not possible making something like this `_context.Horses.Find(3)`

to get a single horse is required to use `.Set<>` method

```csharp
var horse = _context.Set<Horse>().Find(3);
```

or via Samurais DbSet you can get the horse with samurai

```csharp
var horseWithSamurai = _context.Samurais.Include( s => s.Horse)
                                        .FirstOrDefault( s => s.Horse.Id == 3);                                     
```

but there is a problem with this strategy it's possible that a samurai cannot have a horse so in some cases we are getting null horses

```
var horseSamuraiPairs = _context.Samurais
    .Where( s => s.Horse != null)
    .Select( s => new { Horse = s.Horse, Samurai = s })
    .ToList();
```

## DbSet

In this case we dont have a DbSet for horses EF CORE will create a new table with name = <EntityName> but this could be confusing because the table is called horse so you expect that store just a horse to solve this you always can use Fluent Api to explains to EF CORE that entity Horse is going to be mapped as a table called horses.
  
```csharp
modelBuilder.Entity<Horse>().ToTable("Horses");  
```
