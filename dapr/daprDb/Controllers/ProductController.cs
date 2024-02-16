using Microsoft.AspNetCore.Mvc;
using System.Text;
using Dapr.Client;

namespace daprDb.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private static readonly string[] Names = new[] 
    {
        "Spoon", "Coat", "Table", "Game", "Mouse"
    };
    private static readonly string[] Details = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<ProductController> _logger;

    public ProductController(ILogger<ProductController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetProducts")]
    public async Task<IActionResult> Get()
    {
        using var client = new DaprClientBuilder().Build();
        var command = new Dictionary<string,string>(){ 
            {"sql", "select * from products where id < $1"} ,
            {"params", "[3]"}
        };
        var request = new BindingRequest("sqldb", "query");
        request.Metadata["sql"] = "select * from products where id <= $1";
        request.Metadata["params"] = "[3]";
         //var response = await client.InvokeBindingAsync<Bin, BindingResponse>(bindingName: "sqldb", operation: "query", data: "", metadata: command);
        BindingResponse response = await client.InvokeBindingAsync(request);

        var body = response.Data.ToArray();
var message = Encoding.UTF8.GetString(body);

        Console.WriteLine($"results: {message}");
        return new OkResult();
    }

    private async void insert()
    {
        using var client = new DaprClientBuilder().Build();
        foreach(Product prod in Enumerable.Range(1, 5).Select(index => new Product
        {
            Id = index,
            Available = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Name = Names[index-1],
            Price = Random.Shared.Next(1000),
            Currency = "HUF",
            Details = Details[Random.Shared.Next(Details.Length)]
        })
        .ToArray()) {
            var sqlText = $"insert into products (id, name, availability, price, currency, details) values ({prod.Id}, '{prod.Name}', '{prod.Available}', {prod.Price}, '{prod.Currency}','{prod.Details}' );";
            var command = new Dictionary<string,string>(){ {"sql", sqlText} };
            Console.WriteLine(sqlText);
            await client.InvokeBindingAsync(bindingName: "sqldb", operation: "exec", data: "", metadata: command);
        }
        //return new OkResult();
    }
}
