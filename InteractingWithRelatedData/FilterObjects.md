# Related Data to Filter Objects

Now imagine that you wanna get all the samurais that have quotes that contain the word "saved" but you don't wanna get the quotes just the samurais. 

```csharp
var samurais = _context.Samurais
        .Where(s => s.Quotes.Any(q => q.Text.Contains("saved"))).ToList();
```

![image](https://user-images.githubusercontent.com/53051438/198184670-be923e7e-432d-4dd7-be01-35b4259c2a68.png)
