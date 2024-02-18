using System.Text.Json;

namespace daprDb;

public class Product
{
    public int Id { get; set; }
    public DateOnly Available { get; set; }
    public string Name { get; set; }

    public float Price { get; set; }
    public string Currency { get; set; }

    public string? Details { get; set; }

    public override String ToString()
    {
        return JsonSerializer.Serialize(this);
    }

    public static Product CreateFromJson(String jsonValue)
    {
        var options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = false
        };
        return JsonSerializer.Deserialize<Product>(jsonValue, options);
    }
}
