# Filtering in queries

Maybe making a `SELECT *` is not always a good idea and you probably don't always want to get all the rows from a database table.

Filtering queries use lambda expressions as a parameter if you use LINQ method Method Syntax for example if you don't need to get all the samurais just the samurais with the name egonzalezt for example this is the required LINQ method

```csharp
var samurais = _context.Samurais.Where(s => s.Name == "egonzalezt").ToList();
```

this is the lambda expression `s => s.Name == "egonzalezt"` which has these components: 

* An input parameter that represents the samurai type that is going to be querying.

* Lambda operator `=>`

* Expression to be evaluated `s.Name == "egonzalezt"` that uses the input parameter

filtering can be made with hard-coded values like `s.Name == "egonzalezt"` or can be set into a new variable.

using a variable EF CORE will parametrize that variable on the queries

```csharp
private static void QueryFilters()
{
  var filter = "egonzalezt";
  var samurais = _context.Samurais
    .Where(s => EF.Functions.Like(s.Name, filter)).ToList();
}
```

![filter with parameters](https://user-images.githubusercontent.com/53051438/197411643-92cea4c0-8536-42ee-8adf-3d7ddad1a4ef.png)

if the information is sent by a parameter EF CORE gets sure that these parameters are sent to SQL

## Filtering partial text

maybe you don't need to filter a samurai with the name egonzalezt maybe you need to filter all the samurais with `mada` or `moto` on their names.

### Like

```csharp
_context.Samurais.Where(s => EF.Functions.Like(s.Name,"%moto"));
```

### Contains 

is translated into a LIKE SQL statement

```csharp
_context.Samurais.Where(s => s.Name.Contains("mada"));
```

## LINQ to Entities Execution Methods

|ToLIST()|ToListAsync()|
|---|---|
|First()|FirstAsync()|
|FirsOrDefault()|FirstOrDefaultAsync()|
|Single()|SingleAsync()|
|SingleOrDefault()|SingleOrDefaultAsync()|
|Last()|LastAsync()|
|LastOrDefault()|LastOrDefaultAsync()|
|Count()|CountAsync()|
|LongCount()|LongCountAsync()|
|Min(), Max()|MinAsync(), MaxAsync()|
|Average(), Sum()|AverageAsync(), SumAsync()|
||AsAsyncEnumerable|

Not a LINQ method, but a DbSet method that will execute:

Find(keyValue) and FindAsync(keyValue)

these methods have their counterpart as asynchronous methods

### Keep in mind

* The Last methods require the query to have an OrderBy() method otherwise will return the full set and then pick last in memory
* The Single methods expect only one match and will throw if there are none or more than one.
* The First methods return the first of any matches.
* First/Single/Last will throw if no results are returned
* FirstOrDefault/SingleOrDefault/LastOrDefault will return a null if no results are returned.

```csharp
var name = "egonzalezt";
var samurai = _context.Samurais.FirstOrDefault(s => s.Name == name);
```

Find samurai by key

```csharp
var samurai = _context.Samurais.FirstOrDefault(s => s.Id == 2);
```

You can find a samurai by its key using first or default but EF CORE has a method designed specifically to find a samurai by its key.

```csharp
var samurai = _context.Samurais.Find(2);
```

Advantages of the Find method

* Not a LINQ method
* Executes immediately
* if is in memory and tracked by the context EF CORE avoids unneeded database query