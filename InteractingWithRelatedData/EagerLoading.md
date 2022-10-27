# Eager Loading Related Data

There are many ways to query related data but eager loading allows you to use DbSet include method retrieve data and related data in the same call.
 
 ```csharp
 var samuraiWithQuotes = _context.Samurais.Include(s => s.Quotes).ToList();
 ```
 
 With this code EF CORE will perform a `LEFT JOIN` query to retrive all samurai data, there are two possibilities using a single left join or send multiple queries to get the information 
 
 ```csharp
  var samuraiWithQuotes = _context.Samurais.AsSplitQuery().Include(s => s.Quotes).ToList;
 ```
 
 If you want yo filter your queries before EF CORE 5 you need to use Query Projections to filter your data but with EF CORE 5 you can filter your related data with include.
 
 ```csharp
 var filteredData = _context.Samurais
     .Include(s => s.Quotes.Where(q => q.Text.Contains("Thanks"))).ToList() 
 ```
 
 ![image](https://user-images.githubusercontent.com/53051438/197867840-5e380b02-804a-47d4-abf4-e3ad031347b2.png)
 
 Include is part of dbset so you cant put next to FirstOrDefault will get a compile error

* Include child objects

```csharp
_context.Samurais.Include(s => s.Quotes);
```

* Include children and grandchildren

```csharp
_context.Samurais.Include(s => s.Quotes).ThenInclude(q => q.Translations);
```

* Include different children

```csharp
_context.Samurais
  .Include(s=>s.Quotes)
  .Include(s=>s.Clan)
```
