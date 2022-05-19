# Crimzon Finances
Meant to be self hosted for now. images(invoices) are saved in SQLServer so maybe needs to change if cloud is ever used ??
API for handling finance related crap per user: 
- Purchases
- Transfers

API for handling finance related crap 

## TODO:
- [x] Add Docker and docker-compose for easy deployment and database aceess
- [ ] Add Folders ; the ability to sort (transactions and purchases) into them.
- [x] Add Transaction ID to both purchase and transaction models.
- [ ] Tidy up this sad mess of a README file xD
- [ ] Add loans entity
## Architecure
#### Clean With CQRS cause i like it.

  Domain depends on nought
  Application depends on Domain
  API depends on Appliction

  Persistence depends on Application and Domain

  Infrastructure depends on Application
  API depends on INfastruction caue of dependancy injection to work

  CQRS -> Command Query Responsibility Segregation


## Responsibility

#### API
 Recieves HTTP Requests and Responds to them only.
 The main project to run 

#### Application
  Proccess the business Logic

  fluent validation

#### Domain
  Contains the business entities and is the center of the universe!

#### Persistence
  contains the SQL connection and generates the SQL Code needed

#### Infrastructure
  houses security related stuff
  Interfaces are in Application project, implementaion in Infrastructure.
  #### packages
  1. microsoft entity frameworkcore sqlserver

## Packages:
 
 1. Entity Framework Core
 1. Entity Framework Core Tools
 2. Entity Framework Core Design
 3. MediatR.Extensions.Microsoft.DependencyInjection
 4. AutoMapper.Extensions.Microsoft.DependencyInjection
 5. FluentValidation.AspNetCore
 6. Microsoft.AspNetCore.Identity.EntityFrameworkCore 
 7. Microsoft.AspNetCore.Authentication.JwtBearer
 8. X.PagedList - not used
 9. NWebsec.AspNetCore.Middleware - security 

### Commands
#### Migrations
```
    dotnet ef migrations add initial -p persistence/ -s API/

    dotnet ef database update
```
#### User Secerets

```
dotnet user-secret init
dotnet user-secret add TokenKey:Bullshit -s API 
dotnet user-secret add DATABASE_USER:Bullshit -s API 
dotnet user-secret add DATABASE_PASS:Bullshit -s API 
dotnet user-secret add DATABASE_STRING:Bullshit -s API 
```

