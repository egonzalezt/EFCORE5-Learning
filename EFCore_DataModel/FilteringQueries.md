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

filtering can be made with hard coded values like `s.Name == "egonzalezt"` or can be set into a new variable.

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
