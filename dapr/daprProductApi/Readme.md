
# Prepare
## Check initial status
http://localhost:5294/swagger/index.html


## add entity framework
dotnet add package Microsoft.EntityFrameworkCore

## add connection string to appsettings.json
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=5432;Database=my_app;User Id=postgres;Password=example"
  },

## create record and dbcontext

## add psql nuget 
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL

## add EF migrations
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore

## create EF first migration (table create) 
dotnet ef migrations add firstmigration --project .\daprProductApi.csproj
dotnet ef migrations list --project .\daprProductApi.csproj

dotnet ef database update firstmigration --project .\daprProductApi.csproj  

# DAPRize application
## Add reference to the package to csproj
    <ItemGroup>
      <PackageReference Include="Dapr.AspNetCore" Version="1.12.*-*" />
    </ItemGroup>
## Adding Vault from DAPR

