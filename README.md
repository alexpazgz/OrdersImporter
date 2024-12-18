## Order Importer (.Net)
Order Importer is a Clean Architecture Solution. It is .NET Web API using .NET 8.0 and Entity Framework Core, elaborated about clean architecture principles, and design considerations. 
This API get order from external api, store in DB, create a file and response with a summary about orders.

## Technologies
* [.NET 8](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-6.0)
* [Entity Framework Core 8](https://docs.microsoft.com/en-us/ef/core/)
* [AutoMapper](https://automapper.org/)
* [NUnit](https://nunit.org/), [FluentAssertions](https://fluentassertions.com/)

## Getting Started
The easiest way to get started is download solution.
1. Install the latest [.NET 8 SDK]
2. Install the latest DOTNET & EF CLI Tools by using this command `dotnet tool install --global dotnet-ef` 
3. Install the latest version of Visual Studio IDE 2022 (v16.8 and above) 
4. Run in Visual Studio.
 
### Database Configuration

Solution is configured to use an in-memory database. This ensures that all users will be able to run the solution without needing to set up additional infrastructure (e.g. SQL Server).

If you would like to use SQL Server, you will need to update **WebApi\appsettings.Development.json** as follows:

```json
  "UseInMemoryDatabase": false,
```

Verify that the **DefaultConnection** connection string within **appsettings.Development.json** points to a valid SQL Server instance.

### Create Database(SQL Server)

Ensure that "UseInMemoryDatabase" is disabled, as described within previous section.

Then, in package manager console:
* Make sure, we have selected the **Infrastructure** project as default and **Web API** as startup project.
* Then, we will run the database update command: `Update-Database`
* When finish, The database is created as expected.

## Overview

### Domain

This will contain all entities, enums, exceptions, interfaces, types and logic specific to the domain layer.

### Business

This layer contains all application logic. It is dependent on the domain layer, but has no dependencies on any other layer or project. This layer defines interfaces that are implemented by outside layers. For example, if the application need to access a notification service, a new interface would be added to application and an implementation would be created within infrastructure.

### Infrastructure

This layer contains classes for accessing external resources such as file systems, web services, smtp, and so on. These classes should be based on interfaces defined within the application layer.

### WebApi

This layer is a Web API using ASP.NET Core 8.0 with swagger. This layer depends on both the Business and Infrastructure layers, however, the dependency on Infrastructure is only to support dependency injection. 

### Console App

This layer is a Console app using Concole .NET Core 8.0. This layer depends on both the Business and Infrastructure layers, however, the dependency on Infrastructure is only to support dependency injection. 

## How to use it.
1. Run it in visual studio WebApi project as startup. There several ways to usage:
1.1. Postman.
1.2. Swagger(Only Get methods)
1.3. Web browser.

2. Run it in visual studio Console app.

## 1. WebApi
Set WebApi project as startup.

### 1.2. Postman
- Get orders (Ensure to change portnumber.):
```json
curl --location 'https://localhost:7213/Order/v1/GetOrders'
```

### 1.2. Swagger

En GetOrders clic on "try it out" and then "execute".

### 1. 3. Web browser
Writing https://localhost:7232/Order/v1/GetOrders on the web. Ensure to change portnumber.

### 2. ConsoleApp
Set Console app as startup project and run visual studio.