# DutchTreat

[![Build status](https://build.appcenter.ms/v0.1/apps/b25abc42-dd80-4d66-9f78-ba740b701d87/branches/master/badge)](https://appcenter.ms)

https://dutchtreat.projects.lsamorim.com <br>
I created this project to explore the structure of a AspNet Empty Template.

## Entity Framework Core

### My Setup (don't do it if you're gonna run the project)

#### First time
[1] cmd: navigate to the project directory <br>
[2] run: ```dotnet ef database update ``` - That will setup migrations <br>
[3] run: ```dotnet ef migrations add MyMigrationName``` - That will create a migration item named as *MyMigrationName* containing instructions to create Tables for all DbSet of the DbContext (including DbSets for Identity from IdentityDbContext) <br>
[4] run: ```dotnet ef database update ``` - That will execute all instructions of migrations that were created in **Data/Migrations** directory

#### The project has the DutchSeeder will ensure the creation of DutchDb and all its tables by calling ```_ctx.Database.EnsureCreated();``` on ```DutchSeeder.cs```

**References:** <br>
[**A truly generic repository pattern**](https://cpratt.co/truly-generic-repository/) <br>
[**Equinox Project**](https://github.com/EduardoPires/EquinoxProject) <br>
[**Asp.NET MVC, EF Core, Bootstrap course**](https://app.pluralsight.com/library/courses/aspnetcore-mvc-efcore-bootstrap-angular-web/table-of-contents) <br>
