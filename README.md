# Create solution

* Type `dotnet new` in console to get help regarding this command.
* Type `dotnet new --list` to get list of templates which you can use.
* Type `dotnet new sln` to create new solution. Solution will be named after current directory name.

## Create dir structure

```cs
MySpot/
├───src/
└───tests/
```

* Now go to `src` dir,
* and create new project there by executing following command:
  `dotnet new webapi -n MySpot.Api`
* go back to repo root dir
* add newly created project to solution:
  `dotnet sln add src/MySpot.Api/MySpot.Api.csproj`
* You can verify everything by executing build on the sln:
  `dotnet build`