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

## Running application

* You can run an application via following command: `dotnet run` you must be in `MySpot.Api` directory.
* And you can test it with this command: `curl http://localhost:5000/WeatherForecast -i`
* You can test the application with RestClient plugin to VS Code.
  * Create `MySpot.http` in `MySpot.Api`
  * Populate it with following content:

    ```http

      @url = http://localhost:5000

      ###
      GET {{url}}

      ###
      GET {{url}}/WeatherForecast
    ```

## Adding migrations

* Navigate to project where you want to add migration:

```powershell
cd .\src\MySpot.Infrastructure
```

* Add NuGet package to MySpot.Api

```cs
Microsoft.EntityFrameworkCore.Design
```

* Add migration:

```powershell
dotnet ef migrations add Init -o ./DAL/Migrations --startup-project ../MySpot.Api
```

* Navigate to startup project

```powershell
cd ../MySpot.Api
```

* Apply migration:

```powershell
dotnet ef database update
```
