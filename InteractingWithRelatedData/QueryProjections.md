# Query Projections

Define the shape of query results

In EF CORE we use select property to get which properties of an object we want returned, if you're returning more than a single property then you will have to contain the new properties within a type

```csharp
var someProps = _context.Samurais.Select(s => new { s.Id, s.Name }).ToList();
```

using this method that is unknow is defined as [nonymous type](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/anonymous-types), the problem is that if you need to pass that anonymous type to another method you cant do that/

for that reason is good to cast into a new class, struc, record or another type.

```csharp
public struct IdAndName
{
  public IdAndName(int id, string name)
  {
    Id = id;
    Name = name;
  }
  public int Id;
  public string Name
}

public void GetSomeProperties()
{
  var someProps = _context.Samurais.Select(s => new IdAndName(s.Id, s.Name)).ToList();
}
```

You can add more properties to your projection like the list of quotes or maybe another custom value like the total of quotes

```csharp
var someProps = _context.Samurais.Select(s => new { s.Id, s.Name, s.Quotes }).ToList();
```

```csharp
var someProps = _context.Samurais.Select(s => new { s.Id, s.Name, NumberOfQuotes = s.Quotes.Count }).ToList();
```

![image](https://user-images.githubusercontent.com/53051438/198053249-2216fa05-39ae-4824-8fb5-6f0dbe419257.png)

but also you can made projections with filters 

```csharp
var someProps = _context.Samurais.Select(s => new { s.Id, s.Name, 
          SavedOfQuotes =  s.Quotes.Where(q => q.Text.Contains("saved"))
        }).ToList();
```
