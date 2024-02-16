
using Dapr.Client;

var client = new DaprClientBuilder().Build();

Order order = new ("Some value");

string DAPR_STORE_NAME = "statestore";

await client.SaveStateAsync(DAPR_STORE_NAME, order.Id, order);
Console.WriteLine($"Saving Order: {order}");

Order result = await client.GetStateAsync<Order>(DAPR_STORE_NAME, order.Id);
Console.WriteLine($"Getting Order: id:{result.Id} value:{result.Value}");

//await client.DeleteStateAsync(DAPR_STORE_NAME, order.Id);
//Console.WriteLine($"Deleting Order: {order}");