# DutchTreat

https://dutchtreat.projects.lsamorim.com

I created this project to explore the structure of a AspNet Empty Template.

## Entity Framework Core

### Setup
[1] cmd: navigate to the project directory

[2] run: ```dotnet ef database update ``` - That will setup migrations

[3] run: ```dotnet ef migrations add InitialDb``` - That will create a migration item containing instruction to create the entity tables

[4] run: ```dotnet ef database update ``` - That will create all instructions of migrations the were created on **Step 3**
