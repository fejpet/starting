dotnet new -o daprState
cd daprState
dotnet new gitignore
dotnet build
dapr run --app-id statemng -- dotnet run
