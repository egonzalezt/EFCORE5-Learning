# Many-To-Many Payload

When we need to add more data to the relationship between samurai and battle we need a place to store that new data for that reason we need to create a class that represents that relationship BattleSamurai.

## EF CORE

### First way to implement

For the BattleSamurai we need to know the date of the battle for that reason we need to create a weak entity called BattleSamurai 

```csharp
public class BattleSamurai
{
    public int SamuraiId { get; set; }
    public int BattleId { get; set; }
    public DateTime DateJoined { get; set; }
}
```
here we can add the new property DateJoined this additional data in the Join is referred to as a Payload, using this approach is required to add some code on the method OnModelBuilding with Fluent API

A many-to-many relationship is described with methods named HasMany and WithMany

```csharp 
modelBuilder.Entity<End1>()
    .HasMany(e1 => e1.E2List)
    .WithMany(e2 => e2.E1List)
```
Express many to many using HasMany/WithMany

End#1 HAS MANY End#2 via the list navigation property 

That other end also is WITH MANY of End#1, represented by E1 list property starting with either end. There is no difference.

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
       modelBuilder.Entity<Samurai>()
        .HasMany(s => s.Battles)
        .WithMany(b => b.Samurais)
        .UsingEntity<BattleSamurai>
         (bs => bs.HasOne<Battle>().WithMany(),
          bs => bs.HasOne<Samurai>().WithMany())
        .Property(bs => bs.DateJoined)
        .HasDefaultValueSql("getdate()")
}
```

Let's see how it works

* `modelBuilder.Entity<Samurai>()` model builder points to the generic entity of Samurai 

* `.HasMany(s => s.Battles)` is a lambda expression to point to the Battles property of the Samurai Class

* `.WithMany(b => b.Samurais)` points to the Samurais property of the Battle Class

this chunk of code is talking about the particular relationship of BattleSamurai

```csharp
.UsingEntity<BattleSamurai>
    (bs => bs.HasOne<Battle>().WithMany(),
     bs => bs.HasOne<Samurai>().WithMany())
```

With the piece of code we are indicating to EF CORE instead of inferring the join (weak table) there is a class that performs the join and is required to express the relationship between BattleSamurai and the other two classes to refer to the joining in this case Battle and Samurai.

At this point the operation is completed but the new Payload we need to populate and that's why there are two new expressions

* `.Property(bs => bs.DateJoined).HasDefaultValueSql("getdate()")` express about the property DateJoined apply with HasDefaultValueSql the T-SQL Method `getdate()`

#### Making a new migration 

When we perform the migration it creates a file called `20221017191021_manytomanypayload.cs`

Looking at the migrations it renames the names from the different columns from BattleSamurai class due to its conventions

```csharp
migrationBuilder.RenameColumn(
    name: "SamuraisId",
    table: "BattleSamurai",
    newName: "SamuraiId");

migrationBuilder.RenameColumn(
    name: "BattlesBattleId",
    table: "BattleSamurai",
    newName: "BattleId");
```

but what happens if we need to keep the original names? to do that is required to use Fluent Mapping

```csharp
modelBuilder.Entity<BattleSamurai>()
    .Property(bs => bs.SamuraiId).HasColumnName("SamuraisId")

modelBuilder.Entity<BattleSamurai>()
    .Property(bs => bs.BattleId).HasColumnName("BattlesBattleId")    
```

EF CORE Know that the BattleSamurai class map to the BattleSamurai table but what happens if the class is renamed to BattleParticipant it's required to indicate the table to map

```csharp
modelBuilder.Entity<BattleParticipant>().ToTable("BattleSamurai")
```

if you wanna look at the history of the migration and see which migration has been applied run the command `get-migration`

```bash
id                               name              safeName          applied
--                               ----              --------          -------
20221016230041_init              init              init                 True
20221017181503_manytomanyexample manytomanyexample manytomanyexample    True
20221017191021_manytomanypayload manytomanypayload manytomanypayload   False
```

![image](https://user-images.githubusercontent.com/53051438/196265581-5cae69d4-d6e8-40b3-8b87-5e0c4043676a.png)

EF CORE creates the new table with the extra attribute DateJoined

