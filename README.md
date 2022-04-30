# Crimzon Finances

API for handling finance related crap


## Architecure

  Domain depends on nought
  Application depends on Domain
  API depends on Appliction

  Persistence depends on Application and Domain


  CQRS -> Command Query Responsibility Segregation


## Responsibility

#### API
 Recieves HTTP Requests and Responds to them only.
 The main project to run 

#### Application
  Proccess the business Logic

#### Domain
  Contains the business entities and is the center of the universe!

#### Persistence
  contains the SQL connection and generates the SQL Code needed
  #### packages
  1. microsoft entity frameworkcore sqlserver

## Packages:
 
 1. Entity Framework Core
 1. Entity Framework Core Tools
 2. Entity Framework Core Design
 3. MediatR.Extensions.Microsoft.DependencyInjection
 4. AutoMapper.Extensions.Microsoft.DependencyInjection

### Commands
#### Migrations
```
    dotnet ef migrations add initial -p persistence/ -s API/

    dotnet ef database update
```


