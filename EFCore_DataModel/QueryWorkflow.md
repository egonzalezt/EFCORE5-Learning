# Query Work Flow

To perform this operation: `_context.Samurais.ToList()` EF CORE Follows these steps:

* Express and execute the query `_context.Samurais.ToList()`
* Translate the query into proper SQL, internally EF CORE cache queries and reuses them to avoid repeating operations.
* Connect to the database and execute the SQL command into the database
* Database returns the information as rows and columns and EF CORE has to map these rows and columns into objects "Materializing the results"
* The results objects will be added to Entity entries to track the new objects, by default EF Dbcontext will create these entries for each of the resulting objects and keep track of those

## Build queries

EF CORE uses LINQ to build queries and LINQ allows you to write these queries with Query Syntax or Method Syntax [Official Docs](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/query-syntax-and-method-syntax-in-linq)

### Query Syntax 

Is very similar to SQL commands 

```csharp
IEnumerable<int> numQuery1 =
    from num in numbers
    where num % 2 == 0
    orderby num
    select num;
```

### Method Syntax 

```csharp
IEnumerable<int> numQuery2 = numbers.Where(num => num % 2 == 0).OrderBy(n => n);
```

## Deferred Query Execution

The query itself is disconnected from the method that executes the query the code below defines the query in one variable and then in another statement

```csharp
var query = _context.Samurais;
var samurais = query.ToList();
```

There is another way to trigger a query to execute instead of using the LINQ method you can use enumerating queries

```
var query = _context.Samurais

foreach(var s in query)
{
    Console.WriteLine(s.Name);
}
```
Take care with these methods because they affect the performance of your application for example:

```
foreach(var s in _context.Samurais)
{
    Console.WriteLine(s.Name);
}
```

at the beginning of this execution the database gets opened and until the operations inside the brackets are not complete the confection is still open until the enumeration is complete and all the results have been streamed back.

### Considerations

Depending on your application avoid making a huge effort on enumeration because these operations may impact your application performance

* Minimal effort operation 
    ```csharp
    foreach(var s in context.Samurais)
    {
        Console.WriteLine(s.Name)
    }
    ```

* Many things to do

    Lots of operations and methods are executed inside the statement, making the database connection still open until all the processes are completed.

    ```csharp
    foreach(var s in context.Samurais)
    {
        ValidateName(s.Name);
        GetBattlesHistories(s.Id);
        RemoveFistBattle(s.id);
    }
    ```
* Smarter way

    if you need to perform operations that require a huge or considerable effort maybe is not good to make these operations with the database connection open it's much better to filter the data with the necessary information or just get the required data and save the results into memory and process that data without the necessity of overloading the database also this can make inconsistency problems because if another user needs the same data that could affect the performance and the data consistency.

    ```csharp
    var samurais = context.Samurais.ToList();

    foreach(var s in samurais)
    {
        ValidateName(s.Name);
        GetBattlesHistories(s.Id);
        RemoveFistBattle(s.id);
    }
    ```
