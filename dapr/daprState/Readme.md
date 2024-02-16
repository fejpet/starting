Improve a simple in-memory dictionary with an external state storage

dotnet new -o daprState
cd daprState
dotnet new gitignore
dotnet build
dapr run --app-id statemng -- dotnet run

docker exec -it dapr_redis redis-cli

> docker exec -it dapr_redis redis-cli
127.0.0.1:6379> keys *
1) "statemng||21ebee15-54fc-4a2f-91eb-829a3cd27b01"
2) "statemng||720ee7b8-2b54-4e12-a34f-24921cd67a6d"
127.0.0.1:6379>
127.0.0.1:6379> HGET "statemng||21ebee15-54fc-4a2f-91eb-829a3cd27b01" data
"{\"id\":\"21ebee15-54fc-4a2f-91eb-829a3cd27b01\",\"value\":\"Some value\"}"


PostgreSQL
docker run -d -p 5432:5432 -e POSTGRES_PASSWORD=example --name dapr_postgres postgres


> docker exec -it dapr_postgres bash
root@68203f2ee149:/# psql -U postgres
psql (16.2 (Debian 16.2-1.pgdg120+2))
Type "help" for help.

postgres=# CREATE DATABASE my_dapr;
CREATE DATABASE
postgres=# \l
                                                      List of databases
   Name    |  Owner   | Encoding | Locale Provider |  Collate   |   Ctype    | ICU Locale | ICU Rules |   Access privileges
-----------+----------+----------+-----------------+------------+------------+------------+-----------+-----------------------
 my_dapr   | postgres | UTF8     | libc            | en_US.utf8 | en_US.utf8 |            |           |
 postgres=# \q


## execute application with new statestore which points to postgres
dapr run --app-id statemng -d ..\resource\components\ -- dotnet run


 postgres=# \c my_dapr
You are now connected to database "my_dapr" as user "postgres".
my_dapr=# \dt
             List of relations
 Schema |     Name      | Type  |  Owner
--------+---------------+-------+----------
 public | dapr_metadata | table | postgres
 public | state         | table | postgres
(2 rows)

my_dapr=# select * from state;