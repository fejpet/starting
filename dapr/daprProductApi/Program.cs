using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ProductDb>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/product/", async (Product p, ProductDb db) =>
{
    p.Version = Guid.NewGuid();
    db.Products.Add(p);
    await db.SaveChangesAsync();
    return Results.Created($"/notes/{p.Id}", p);
});

app.MapGet("/products", async (ProductDb db) => await db.Products.ToListAsync())
.WithName("GetProduct")
.WithOpenApi();

app.MapGet("/products/{id:int}", async (int id, ProductDb db) =>
{
    return await db.Products.FindAsync(id)
            is Product p
                ? Results.Ok(p)
                : Results.NotFound();
});


app.MapPut("/products/{id}", async (int id, Product input, ProductDb db) =>
{
    var p = await db.Products.FindAsync(id);

    if (p is null) return Results.NotFound();

    if (!p.Version.Equals(input.Version))
    {
        return Results.Conflict();
    }
    p.Name = input.Name;
    p.Available = input.Available;
    p.Price = input.Price;
    p.Currency = input.Currency;
    p.Details = input.Details;
    p.Version = Guid.NewGuid();

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/todoitems/{id}", async (int id, ProductDb db) =>
{
    if (await db.Products.FindAsync(id) is Product todo)
    {
        db.Products.Remove(todo);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.Run();

public record Product(int Id)
{
    public string Name { get; set; } = default;
    public DateOnly Available { get; set; } = default;
    public float Price { get; set; } = default;
    public String Currency { get; set; } = "EUR";
    public String Details { get; set; } = default;
    public Guid Version { get; set; } = default;

}

public class ProductDb : DbContext
{
    private string connectionString;
    public ProductDb(DbContextOptions<ProductDb> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().ToTable("products");
    }
    public DbSet<Product> Products => Set<Product>();
}