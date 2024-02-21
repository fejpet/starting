using System.Text.Json;
using System.Text.Json.Serialization;
public class Order
{
    public String Id {get;private set;}

    public String Value {get;private set;}

    public Order(String id, String value) {
        Id = id;
        Value = value;
    }

    public override String ToString() {
        return JsonSerializer.Serialize(this);
    }

    public static Order CreateFromJson(String jsonValue) {
        var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = false
            };
        return JsonSerializer.Deserialize<Order>(jsonValue, options);
    }


}
