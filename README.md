# Crimzon Finances

API for handling finance related crap


## Architecure

  Domain depends on nought
  Application depends on Domain
  API depends on Appliction

  Persistence depends on Application and Domain

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

## Packages:
 
 1 

### Commands

