using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

ConcurrentDictionary<int, Product> products = new();

foreach (int index in Enumerable.Range(1, 5))
{
    products[index] =
    new Product
    (
        index,
        "Name" + index,
        DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        1234.54f,
        "HUF",
        "Details" + index,
        Guid.NewGuid()
    );
}

app.MapGet("/product", () =>
{
    return products.Values;
})
.WithName("GetProduct")
.WithOpenApi();

app.MapPut("/product", async Task<Results<Ok<Product>, Created, NotFound<Product>, BadRequest<Product>, Conflict<object>>> (Product product) =>
{
    if (product.Id == 0)
    {
        Product newProduct = new Product(products.Keys.Max() + 1, product.Name, product.Available, product.Price, product.Currency, product.Details, Guid.NewGuid());
        products[newProduct.Id] = newProduct;
        return TypedResults.Created("/product/" + newProduct.Id);
    }
    if (!products.ContainsKey(product.Id))
    {
        return TypedResults.NotFound(product);
    }
    try
    {
        products.AddOrUpdate(product.Id, product, (key, old) =>
        {
            if (product.Version.Equals(old.Version))
            {
                //create new record with new guid
                return new Product(product.Id, product.Name, product.Available, product.Price, product.Currency, product.Details, Guid.NewGuid());
            }
            throw new InvalidDataException();
        });
        return TypedResults.Ok(products[product.Id]);
    }
    catch (InvalidDataException ex)
    {
        object result = new { Error = "update conflict", Id = product.Id };
        return TypedResults.Conflict(result);
    }
    return TypedResults.BadRequest(product);
})
.Produces(400)
.Produces<Product>()
.WithOpenApi();

app.MapDelete("/product/{id}", async Task<Results<Ok<Product>, Conflict<object>, NotFound>> (int id) =>
{
    if (products.ContainsKey(id))
    {
        Product product;
        if (products.TryRemove(id, out product))
        {
            return TypedResults.Ok(product);
        }
        else
        {
            object result = new { Error = "cannot remove", Id = id };
            return TypedResults.Conflict(result);
        }
    }
    return TypedResults.NotFound();
})
.Produces(404)
.Produces(409)
.Produces<Product>()
.WithOpenApi();

app.Run();

record Product(int Id, String Name, DateOnly Available, float Price, String Currency, String Details, Guid Version);
