# One-To-Many Relationship 

The way how one-to-many relationship is represented in a child table has a foreign key column pointing to the parent 

## EF CORE

### Option 1

Parent table
```csharp
public class Samurai
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Quote> Quotes { get; set; } = newList<Quote>();
}
```
Child table

In this example EF CORE automatically detects the one-to-many relationship with this attribute `public List<Quote> Quotes { get; set; }` and creates the foreign key for the Quote table

### Option 2

Parent table
```csharp
public class Samurai
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Quote> Quotes { get; set; } = newList<Quote>();
}
```
Child table
```csharp
public class Quote
{
    public int Id { get; set; }
    public string Text { get; set; }
    public Samurai Samurai { get; set; }
}
```

When the child table stores the type of the parent in this case Samurai Child has a navigation property back to the parent. Property is required by default.

### Option 3 

Parent table
```csharp
public class Samurai
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Quote> Quotes { get; set; } = newList<Quote>();
}
```
Child table
```csharp
public class Quote
{
    public int Id { get; set; }
    public string Text { get; set; }
    public Samurai Samurai { get; set; }
    public int SamuraiId { get; set; }
}
```

The child can also have a property that's a foreign key back to the parent EF CORE with their conventions with this structure `<ParentName><Id>` like SamuraiId EF CORE know that is a foreign key referring to the parent another option is `<ParentName>` without Id. 

#### Take Care 

```csharp
public class Quote
{
    public int Id { get; set; }
    public string Text { get; set; }
    public int SamuraiFk { get; set; }
}
```
if you put it as SamuraiFk EF CORE does not detect it as a foreign key it just is mapped as another property to the table but it is just the conventions and these can be overridden using the different mapper tools.

### Notes

How you define the relationships in your code will also impact whether the relationship is required or optional

If you need to traverse from the child to the parent is a good idea to add a foreign key. Also using this method SamuraiId which is an integer cannot be null meaning that a quote can not exist if there is no Samurai for that quote.

```csharp
public class Quote
{
    public int Id { get; set; }
    public string Text { get; set; }
    public int SamuraiId { get; set; }
}
```
When we have a navigation property and no foreign key there is nothing on the code that explicitly indicates that Samurai samurai can not be null as a result your Quote can exist without having a Samurai.

```csharp
public class Quote
{
    public int Id { get; set; }
    public string Text { get; set; }
    public Samurai Samurai { get; set; }
}
```
