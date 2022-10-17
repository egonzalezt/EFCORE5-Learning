# One-To-One 

A one-to-one relationship is a type of cardinality that refers to the relationship between two entities A and B in which one element of A may only be linked to one element of B, and vice versa. In our example for the battle, the Samurai needs a horse and one horse belongs to one samurai, not too many samurais.

## EF CORE

Horse Class 

```csharp
public class Horse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int SamuraiId { get; set; }
}
```

Samurai Class

```csharp
public class Samurai
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Quote> Quotes { get; set; } = newList<Quote>();
    public List<Battle> Battles { get; set; } = newList<Battle>();
    public Horse Horse { get; set; }
}
```

* The Horse store the SamuraiId of the samurai that belongs to.

* The Samurai has a navigation property to define the samurai's horse.

These two attributes in the class are enough for EF CORE to identify the relationship.

### One-To-One Relationship is always optional

A samurai can have a horse and a samurai can be without a horse that's why is use a navigation property, there is no way to apply that constraint in the model or the database.

if you want to require that a samurai always has a horse, you will have to do that in your business logic.

### Dependent Default: Must have a parent 

A horse must have a samurai and in the fact that we are using an integer foreign key that's non-nullable a horse exists only if that horse belongs to a samurai but there are methods to change that default dependency 

Additional Considerations 
* Integer FK is non-nullable by default
* Allow "orphaned" horses with a nullable foreign key or a mapping
* EF CORE can usually determine principal & dependent.
* Fluent API Mappings can be used to adjust EF CORE's conventional interpretation.

### Migration

Finally, create a new migration and apply it to the database creating a new table called Horse, the migration is saved on a file called `20221017195241_onetoonehorse.cs`.

![image](https://user-images.githubusercontent.com/53051438/196270848-15a57902-45af-404f-9cde-d9d2de84a164.png)
