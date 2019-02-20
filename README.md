# DutchTreat

https://dutchtreat.projects.lsamorim.com

I created this project to explore the structure of a AspNet Empty Template.

## Entity Framework Core

### Setup
[1] cmd: navigate to the project directory

[2] run: ```dotnet ef database update ``` - That will setup migrations

[3] run: ```dotnet ef migrations add InitialDb``` - That will create a migration item named as *InitialDb* containing instructions to create the entities' tables

[4] run: ```dotnet ef database update ``` - That will execute all instructions of migrations that were created on **Step 3**
