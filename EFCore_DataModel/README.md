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

