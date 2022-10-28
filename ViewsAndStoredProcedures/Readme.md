# Views and Stored Procedures

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

## Keyless Entities

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

