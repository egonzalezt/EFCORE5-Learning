# Intro

The objective of this wiki is to document all the related information about entity framework core 5.0 

[Official Docs](https://learn.microsoft.com/en-us/ef/)

## Requirements

* Entity Framework Version 5.0
* Microsoft Visual Studio 2022
* C#

## Setup

* [Install .NET 5.0](https://dotnet.microsoft.com/en-us/download/dotnet/5.0)

* [Visual Studio 2022](https://visualstudio.microsoft.com/vs)

## Project structure

* Build and interact with data models
* Setup logging on the database
* Get EF Core to see your db logic
* EF Core and ASP.NET Core app testing

### Guide

Before start follow this structure to understand the folder structure on this docs

* [Db Relationships](DbRelationships/README.md)
* [Data Model](EFCore_DataModel/README.md)
* [Migrations](Migrations/README.md)
* [Related Data](InteractingWithRelatedData/README.md)
* [Power Tools](PowerTools/README.md)
* [Views n' Stored Procedures](ViewsAndStoredProcedures/Readme.md)
* [Testing](Testing/Readme.md)

## [What is EF Core?](https://learn.microsoft.com/en-us/ef/core/)

Microsoft's official cross-platform data access framework for .NET

EF CORE is an ORM that is designed to reduce the friction between how data is structured in a relational database thanks to ORM we do not need to build our queries, and methods to transform these queries into objects these job is performed by ORM

ORM infers that classes or objects have a similar structure EF CORE has a mapping layer that gives us a lot more flexibility in how to get from objects to tables and object properties to table columns.

EF CORE can be connected with a lot of different database providers like Mariadb, Mysql, MS SQL, PostgresSQL, etc. 

## Suggestions? 

Feel free to make PR with new suggestions or recommendations about this documentation. Any recommendation will be received and analyzed thanks :)
