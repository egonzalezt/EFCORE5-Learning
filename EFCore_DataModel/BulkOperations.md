# Bulk Operations

If we want to add two samurais EF CORE makes two different queries to perform the same operation, SQL server provider by default requires a minimum of four operations to trigger the bulk support 

with bulk operations now we can add a group of samurais without making N SQL queries just making one query to add that group 

Before

![image](https://user-images.githubusercontent.com/53051438/197402391-a6bb3846-5e2d-4695-b145-3bddf177fde6.png)

After

![image](https://user-images.githubusercontent.com/53051438/197402429-792d3bc2-3c11-492a-b256-46e4c8601d60.png)
