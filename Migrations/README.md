# Migrations

EF CORE uses migrations to manage how you update and control your database history

## EF CORE migration

with the paradigm of code first mean that base on the code of your project (entities) EF CORE will build the entire database tables and relationships, with that approach the migrations will be created.

For each change to your model, a new migration will be created describing the changes and then the migrations API create the proper SQL script, and finally, that script will be executed on your database.

### First migration

To create a migration there are some NuGet packages required to perform these operations 

* [Migrations Commands](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Tools)

* [MigrationsAPI](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design)


#### Applying your first migration

EXPLAIN HOW TO RUN A MIGRATION

##### Analyzing your first migration 

EF CORE Creates a new folder called migrations where store different files: 

* EfcoreContextModelSnapshot 

    This file keeps track of the current state of the model. also is used to determine how to migrate from one model version to the next. also this file knows which database engine we are working on.

    This file creates the table and columns needed based on the models and their attributes specifying primary keys and foreign keys.

    One thing to keep in mind is that EF CORE recognized the one-to-many relationship

    EF CORE creates indexes for every one of the foreign keys that it discovers in the model.

#### Migrations recommendation

When the models are modified is important to take care of how we make the migrations because there is no problem when we are in a development environment instead of a production environment. After all, on production, you need to take advantage of the ability to generate the SQL and take more control over how and when it's applied to the production database.

* The development database uses the command `update-database`
* The production database uses the command `script-migration`

There are two situations if you make a migration without a database EF CORE will automatically create the database but if you create the script you are responsible to make manually the migration

using the command `update-database -verbose` will show all the processes that EF CORE needs to do to make the migration. With migrations, EF CORE creates a new table on the database to keep track of the history of the migrations executed on the database

#### Migrations result

TAKE A PICTURE OF THE ACTUAL DATABASE WITH THE SCHEMA AND TABLES

FIND IF IS POSSIBLE TO DISPLAY THE DATABASE ON MS SSMS 

Now looking at the new database tables the data type for Name on the Samurais table is nvarchar(max) but why? this value can be changed depending on your database provider, not by EF CORE

## Reverse Engineering

To make this possible the practice of reverse engineering is needed to install Scaffold.

* Powershell: Scaffold-DbContext
* EF CLI: dotnet ef dbcontext scaffold

Requires the connection string to start the process this is an example:

scaffold-dbcontext -provider Microsoft.EntityFrameworkCore.SqlServer -connection `"Server=localhost\SQLEXPRESS;Database=YourSuperDatabase;Trusted_Connection=True;"`

Before running this command remember to create your database and all its tables scaffolding read your database and transform your tables into an object.

## Mapping to Db

EF CORE has conventions or defaults assumptions to map into your database for example `guid id` EF CORE infers that the primary key `property name = column name` but these assumptions did not work all the time for that reason on the context file you can override with Fluent Mappings using Fluent API

```csharp
modelBuilder.Entity<Quotes>().Property(q => q.Text).HasColumnName("Line");
```

using fluent API we can tweak how EF CORE will interpret the model.

Finally, another way to perform these operations using Data Annotations directly on the entity but has more limitations so is better to use fluent API.

```csharp
[Column("Line")]
public string Text {get; set;}
```
