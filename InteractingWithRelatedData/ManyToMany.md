# Interacting Many-To-Many Related Data

Now working with many-to-many relationships data could be save in many ways

* Adding a existing samurai to an existing battle
* Adding a new samurai to an existing battle
* Create a new battle with a new samurai

## One-To-Many vs Many-To-Many

using One-To-Many you have a single samurai that store a set or a list of quotes and these quotes save the samurai id because is just one samurai but when we talk about Many-To-Many this option is not a good idea because we have a set of battles and a set of samurais so one samurai can be in 1..* battles and one battle can participate 1..* samurais to solve that we need to create a weak table.

## Example

at this moment looking at [Many-To-Many Payload](https://github.com/egonzalezt/EFCORE5-Learning/blob/b47b6675edf80e2d94e4a3100738eefff7ee2782/DbRelationships/ManyToManyPayload.md) is created a table base on the entity [BattleSamurai](https://github.com/egonzalezt/EFCORE5-Learning/blob/b47b6675edf80e2d94e4a3100738eefff7ee2782/Application/EfcoreApp.Domain/EfcoreApp.Infrastructure/EntityFramework/EfcoreContext.cs#L26) by Fluent API 

to understand how EF CORE Vanilla create this relationship you need to remove or comment on model creating method and create a new migration.

```csharp
var batttle = _context.Battle.FirstOrDefault();
battle.Samurais.Add(new Samurai { Name = "Miyamoto Musashi"});
_context.SaveChanges();
```

![image](https://user-images.githubusercontent.com/53051438/198308673-2507cbfc-909b-4fe0-8330-a9a91a1d8de1.png)

This query first create the new samurai and return its id, then pass the id of the battle and the id of the new samurai to the table to the BattleSamurai join table and thats all this operation is automatically made by EF CORE

Now if you need to get the battle with its samurais you dont need to think about how you call that join table BattleSamurai you just need to include the samurais and EF CORE handles the rest of the job

```csharp
var battle = _context.Battles.Include(b => b.Samurais).FirstOrDefault();
```

![image](https://user-images.githubusercontent.com/53051438/198311620-f89d64c5-2194-43e2-8bf5-d1035460a3c7.png)

As you can see EF CORE creates the sql and navigates through the join and get the relation ships

get all the battles with its related samurais

```csharp
var battles = _context.Battles.Include(b => b.Samurais).ToList();
```

set all samurais to all battles

```csharp
var allBattles = _context.Battles.ToList();
var allSamurais = _context.Samurais.ToList();
foreach(var battle in allBattles)
{
  battle.Samurais.AddRange(allSamurais);
}
_context.SaveChanges();
```

this method use eager loading to get all samurais and all battles and process on the context and using add range EF CORE will add just the samurais that are new to that battle the samurais that already exists are just being ignore because its state is unchaged.

if during the process occurus an exception EF CORE always uses transaction so all commited changes will be reverted.

Take care with this approach if there is a lot of battles and a lot of samurais **this operation could impact on your application performance.**