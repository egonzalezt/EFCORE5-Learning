# [Views and Stored Procedures](https://learn.microsoft.com/en-us/ef/core/querying/sql-queries)

EF CORE has the ability to execute raw SQL na stored procedures and map to database views making easy to query read-only data.

EF CORE Allows you to work directly with 

* Stored Procedures
* Views
* Scalar functions
* Table view Functions
* Map to queries in DbContext
  using this strategy you can use your own queries instead of EF CORE build that query

# Using views and other Database Object with migrations

If you need to build a function that gets the earliest battle in which a samurai has fought you can build your own SQL command

```SQL
CREATE FUNCTION[dbo].[EarliestBattleFoughtBySamurai](@samuraiId int)
RETURNS char(30) AS
BEGIN
  DECLARE @ret char(30)
  SELECT TOP 1 @ret = Name
  FROM Battles
  WHERE Battles.BattleId IN(SELECT BattleId
        FROM BattleSamurai
        WHERE SamuraiId = @samuraiId)
            ORDER BY StartDate
            RETURN @ret
END
```

and also add a view to list the samurais, giving the name, number of battle that they've fought and with a scalar function to get the earlist battle

```SQL
CREATE VIEW dbo.SamuraiBattleStats
AS
SELECT dbo.Samurais.Name,
COUNT(dbo.BattleSamurai.BattleId) AS NumberOfBattles,
        dbo.EarliestBattleFoughtBySamurai(MIN(dbo.Samurais.Id)) 
AS EarliestBattle
 FROM dbo.BattleSamurai INNER JOIN
      dbo.Samurais ON dbo.BattleSamurai.SamuraiId = dbo.Samurais.Id
 GROUP BY dbo.Samurais.Name, dbo.BattleSamurai.SamuraiId
```

Now how you can implement these SQL examples on your database

first you need to create a new migration and yes there is no new changes but you are going to add these new changes, using `migrationBuilder.Sql("SQL CMD");` this method takes raw sql as a parameter and the way to add sql is using [Verbatim](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/verbatim)

Also on the down method you need to remove these methods if you revert the migration

```csharp
public partial class SamuraiBattleStats : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(
          @"CREATE FUNCTION[dbo].[EarliestBattleFoughtBySamurai](@samuraiId int)
            RETURNS char(30) AS
            BEGIN
              DECLARE @ret char(30)
              SELECT TOP 1 @ret = Name
              FROM Battles
              WHERE Battles.BattleId IN(SELECT BattleId
                                 FROM BattleSamurai
                                WHERE SamuraiId = @samuraiId)
              ORDER BY StartDate
              RETURN @ret
            END")
        migrationBuilder.Sql(
        @"CREATE VIEW dbo.SamuraiBattleStats
          AS
          SELECT dbo.Samurais.Name,
          COUNT(dbo.BattleSamurai.BattleId) AS NumberOfBattles,
                  dbo.EarliestBattleFoughtBySamurai(MIN(dbo.Samurais.Id)) 
  	     AS EarliestBattle
          FROM dbo.BattleSamurai INNER JOIN
               dbo.Samurais ON dbo.BattleSamurai.SamuraiId = dbo.Samurais.Id
          GROUP BY dbo.Samurais.Name, dbo.BattleSamurai.SamuraiId");
    }
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("DROP VIEW dbo.SamuraiBattleStats");
        migrationBuilder.Sql("DROP FUNCTION dbo.EarliestBattleFoughtBySamurai");
    }
}
```

And now running the migrations EF CORE creates the views and the functions on our database

![image](https://user-images.githubusercontent.com/53051438/198600720-c051cf33-9e8a-4e9a-bce9-3f1dbd12552d.png)

## [Keyless Entities](https://learn.microsoft.com/en-us/ef/core/modeling/keyless-entity-types)

There is another way to map entities that EF CORE will consider them as read-only, and allows you to map to views and tables that have no Primary key 

By default keyless entities will never tracked by EF CORE and if you setup to be track EF CORE will ignores this instruction.

also can map tables or views with no primary key

to create a keyless entity first you need to create a entity which does not specify any key like SamuraiId, then you need to add into the DbContext as a DbSet and tell to fluent api that the entity is keyless

```csharp
public DbSet<KeylessEntity> KeylessEntity { get; set; }
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
  modelBuilder.Entity<KeylessEntity>().HasNoKey().ToView("KeylessEntity");
}
```

### Take care

Now our `KeylessEntity` is added as a DbSet C# compilar will threats as a normal entity with key and we know that this entity does not have any key

methods like

```csharp
_context.KeylessEntity.Find(2)
```

The compiler will not trigger any alert but at execution EF CORE will raise a new exception.

## Working with Raw SQL

You can execute Raw SQL methods from your contexts:

```csharp
_context.Samurais.FromSQLRaw("some sql string").ToList();
_context.Samurais.FromSQLRawAsync("some sql string").ToList();
_context.Samurais.FromSQLInterpolated($"some sql string {var}").ToList();
_context.Samurais.FromSQLInterpolatedAsync($"some sql string {var}").ToList();
```

### Example

* FromSQLRaw

  ```csharp
  var samurais = _context.Samurais.FromSqlRaw(
                "Select Id, Name from Samurais").Include(s=>s.Quotes).ToList();
  ```
* FromSQLInterpolated

  ```csharp
  string name = "Kikuchyo";
  var samurais = _context.Samurais
      .FromSqlInterpolated($"Select * from Samurais Where Name= {name}")
      .ToList();
  ```

### Raw SQL Limitations

* Must return data for all properties of the entity type
  `Select Name from samurais` will raise a new exception
* Column names in results match mapped column names
* Query can't contain related data
* Only query entities and keyless entities know by DbContext
  If you return a entity that is not define on the DbSet EF CORE will raise a new exception, and also you can't select navegation properties in SQL
  to make this you need to do:
    ```csharp
    _context.Samurais.FromSqlRaw("Select Id, Name from Samurais").Include(s=>s.Quotes).ToList();
    ```
  This strategy cannot be perform on stored procedures.

### Take care

It's important when using `FromSqlRaw` to use parameters when doing things like filtering. so you don't have to worry about SQL Injection attacks.

Avoid using FromSQLRaw and use interpolation if you made the interpolation EF CORE will raise a new exception `.FromSqlRaw($"Select * from Samurais Where Name= {name}")` but if you add extra quotes to avoid the exception `.FromSqlRaw($"Select * from Samurais Where Name= '{name}'")`

⚠️ Warning

Be very careful when using FromSqlRaw, and always make sure values are either from a safe origin, or are properly sanitized. **SQL injection attacks can have disasterous consequences for your application.**

**Danger**

```csharp
string name = "Kikuchyo";
var samurais = _context.Samurais
  .FromSqlRaw($"Select * from Samurais Where Name= '{name}'")
  .ToList();
```

## Stored Procedures

You can also run Stored Procedures by making a new migration and adding your stored procedures using `migrationBuilder.Sql("Your SQL Stored Procedure")`

```csharp
public partial class newsprocs : Migration
{
  protected override void Up(MigrationBuilder migrationBuilder)
  {
      migrationBuilder.Sql(
       @"CREATE PROCEDURE dbo.SamuraisWhoSaidAWord
         @text VARCHAR(20)
         AS
         SELECT      Samurais.Id, Samurais.Name
         FROM        Samurais INNER JOIN
                     Quotes ON Samurais.Id = Quotes.SamuraiId
         WHERE      (Quotes.Text LIKE '%'+@text+'%')");
      migrationBuilder.Sql(
        @"CREATE PROCEDURE dbo.DeleteQuotesForSamurai
          @samuraiId int
          AS
          DELETE FROM Quotes
          WHERE Quotes.SamuraiId=@samuraiId");
  }
  protected override void Down(MigrationBuilder migrationBuilder)
  {
  }
}
```

Run the migration and create a Raw SQL to execute your stored procedure

```csharp

var text = "Happy";
var samurais = _context.Samurais.FromSqlRaw(
"EXEC dbo.SamuraisWhoSaidAWord {0}", text).ToList();
```

```csharp
var text = "Happy";
var samurais = _context.Samurais.FromSqlInterpolated(
$"EXEC dbo.SamuraisWhoSaidAWord {text}").ToList();
```

running your store procedure you require a value to execute that stored procedure for that reason you need to pass that value directly on the string like this `EXEC dbo.SamuraisWhoSaidAWord {0}` or using a variable `"EXEC dbo.SamuraisWhoSaidAWord {text}"`
