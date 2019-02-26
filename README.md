# DutchTreat

https://dutchtreat.projects.lsamorim.com

I created this project to explore the structure of a AspNet Empty Template.

## Entity Framework Core

### My Setup (don't do it if you're gonna run the project)

#### First time

[1] cmd: navigate to the project directory

[2] run: ```dotnet ef database update ``` - That will setup migrations

[3] run: ```dotnet ef migrations add MyMigrationName``` - That will create a migration item named as *MyMigrationName* containing instructions to create Tables for all DbSet of the DbContext (including DbSets for Identity from IdentityDbContext)

[4] run: ```dotnet ef database update ``` - That will execute all instructions of migrations that were created in **Data/Migrations** directory

#### The project has the DutchSeeder will ensure the creation of DutchDb and all its tables by calling ```_ctx.Database.EnsureCreated();``` on ```DutchSeeder.cs```
