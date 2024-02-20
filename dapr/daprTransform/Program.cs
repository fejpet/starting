using System.Text.Json.Serialization;
using Dapr.Client;

var client = DaprClient.CreateInvokeHttpClient(appId: "order-processor");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.MapGet("/checkout", async () =>
{
    Console.WriteLine("checkout");
    var order = new Order(1);
    var cts = new CancellationTokenSource();
    var response = await client.PostAsJsonAsync("/orders", order, cts.Token);
    Console.WriteLine($"result: {response.StatusCode}");
})
.WithName("GetProductHUF")
.WithOpenApi();

app.Run();


public record Order([property: JsonPropertyName("orderId")] int OrderId)
{
    public Guid Version { get; set; } = default;
}