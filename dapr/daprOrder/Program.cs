using Microsoft.EntityFrameworkCore;
using Dapr.Client;
using System.Text.Json.Serialization;

var client = new DaprClientBuilder().Build();
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

const string DAPR_SECRET_STORE = "shopSecrectStore";
const string PRODUCT_API_CONNECTIONSTRING = "orderConnectionString";

var secret = await client.GetSecretAsync(DAPR_SECRET_STORE, PRODUCT_API_CONNECTIONSTRING);
var connectionString = secret[PRODUCT_API_CONNECTIONSTRING];
Console.WriteLine(connectionString);

builder.Services.AddDbContext<OrderDb>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/orders/", async (Order p, OrderDb db) =>
{
    Console.WriteLine("post");
    /*
    p.Version = Guid.NewGuid();
    db.Orders.Add(p);
    await db.SaveChangesAsync();*/
    return Results.Created($"/orders/{p.OrderId}", p);
});

app.MapGet("/orders", async (OrderDb db) =>
{
    return Results.Ok(new Order(2));
    //return await db.Orders.ToListAsync();
})
.WithName("GetOrder")
.WithOpenApi();

app.MapGet("/orders/{id:int}", async (int id, OrderDb db) =>
{
    return Results.Ok(new Order(1));
    /*
    return await db.Orders.FindAsync(id)
            is Order p
                ? Results.Ok(p)
                : Results.NotFound();
                */
});


app.MapPut("/orders/{id}", async (int id, Order input, OrderDb db) =>
{
    var p = await db.Orders.FindAsync(id);

    if (p is null) return Results.NotFound();

    if (!p.Version.Equals(input.Version))
    {
        return Results.Conflict();
    }
    p.Version = Guid.NewGuid();

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/orders/{id}", async (int id, OrderDb db) =>
{
    if (await db.Orders.FindAsync(id) is Order order)
    {
        db.Orders.Remove(order);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.Run();

public record Order([property: JsonPropertyName("orderId")] int OrderId)
{
    public Guid Version { get; set; } = default;
}

public class OrderDb : DbContext
{
    private string connectionString;
    public OrderDb(DbContextOptions<OrderDb> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>().ToTable("orders");
    }
    public DbSet<Order> Orders => Set<Order>();
}