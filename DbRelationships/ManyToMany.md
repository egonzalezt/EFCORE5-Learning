# Many-To-Many

A many-to-many relationship is a type of cardinality that refers to the relationship between two entities, say, A and B, where A may contain a parent instance for which there are many children in B and vice versa. For example, think of A as Authors, and B as Books. In our example, a Samurai can participate in many battles and a battle can participate many Samurais.

## EF CORE

For simple Many-To-Many relationships, you only need to add properties in the related classes that point to each other and are referred to skip navigation because we skip from one end of the relationship to another end of the relationship.

```csharp
public class Samurai
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Quote> Quotes { get; set; } = newList<Quote>();
    public List<Battle> Battles { get; set; } = newList<Battle>();
}
```
```csharp
public class Battle
{
    public int BattleId { get; set; }
    public string Name { get; set; }
    public List<Samurai> Samurais { get; set; } = newList<Samurai>();
}
```

As the definition of many-to-many relationships requires a weak table, EF CORE comprehends that the database will need to have a join table to represent each samurai battle connection using EF CORE migrations it will create the table, keys, and indexes for us. And when we run a command EF CORE creates the necessary SQL script to perform the different operations.

"PICTURE 004 CREATE SOMETHING SIMILAR"

Is possible to make this relationship more explicit by creating the Join table or Weak table but is optional.

## Code

The current database just have the basic relationship between Samurai and Quote 

![image](https://user-images.githubusercontent.com/53051438/196252094-18c1ea7d-877c-44f2-9cfb-a4de77d1444f.png)

with the new code about battles run the migrations command 

* `add-migration manytomanyexample`
* `update-database`

![image](https://user-images.githubusercontent.com/53051438/196252631-6c4e2d36-3070-45c4-889a-c1b512ceea58.png)

As expected, EF CORE automatically creates the weak table BattleSamurai
