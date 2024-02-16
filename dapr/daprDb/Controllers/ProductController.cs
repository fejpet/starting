using Microsoft.AspNetCore.Mvc;

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
    public IEnumerable<Product> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new Product
        {
            Id = index,
            Available = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Name = Names[index-1],
            Price = Random.Shared.Next(1000),
            Currency = "HUF",
            Details = Details[Random.Shared.Next(Details.Length)]
        })
        .ToArray();
    }
}
