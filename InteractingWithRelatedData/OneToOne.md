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

  ```csharp
  var samurai = _context.Samruais.AsNoTracking().FirstOrDefault(s => s.Id == 5);
  samurai.Horse = new Horse ( Name = "Ed" );
  
  using var newContext = new EfcoreContext();
  newContext.Samurais.Attach(samurai)
  newContext.SaveChanges();
  ```
