# DutchTreat

https://dutchtreat.projects.lsamorim.com

I created this project to explore the structure of a AspNet Empty Template.

## Entity Framework Core

### My Setup (don't do it if you're gonna run the project)

#### First time

[1] cmd: navigate to the project directory

[2] run: ```dotnet ef database update ``` - That will setup migrations

[3] run: ```dotnet ef migrations add InitialDb``` - That will create a migration item named as *InitialDb* containing instructions to create the **Entities' tables**

[4] run: ```dotnet ef database update ``` - That will execute all instructions of migrations that were created in **Data/Migrations** directory

#### After derive from IdentityDbContext
[1] cmd: navigate to the project directory

[2] run: ```dotnet ef migrations add Identity``` - That will create a migration item named as *Identity* containing instructions to create the entities' for **Identity tables**

[3] run: ```dotnet ef database drop``` - That will drop the database

[4] run: ```dotnet ef database update``` - That will execute all instructions of migrations that were created in **Data/Migrations** directory
