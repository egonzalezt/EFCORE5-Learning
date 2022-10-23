# Deleting

Deleting on EF CORE is very different than you expect like

"Take this id = 1 and delete that object" EF CORE does not work like that. 

```csharp
private static void RetrieveAndDeleteASamurai()
{
    var samurai = _context.Samurais.Find(2);
    _context.Samurais.Remove(samurai);
    _context.SaveChanges();
}
```

EF CORE first needs to track the required object to be deleted and then perform the operation to remove that object.

There are two ways to delete an object

* using the context

    ```csharp
    _context.Samurais.Remove(samurai)
    ```
    ```csharp
    _context.Samurais.RemoveRange(samuraiList)
    ```

* using DbContext

    ```csharp
    _context.Remove(samurai)
    ```
    ```csharp
    _context.RemoveRange(samuraiList)
    ```

## Keep in mind

* Don't try to delete objects just with their id.

* If EF CORE standard workflow is causing performance problems or it is just difficult to code is better to try stored procedures instead.

* Soft delete (mark data as deleted yet remains in the database) to make that EF CORE has global query filters to make sure that non of the soft deleted data is included in the queries.