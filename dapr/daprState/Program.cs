
Dictionary<string, string> repository = new();

Order order = new ("Some value");

repository[order.Id.ToString()] = order.ToString();
Console.WriteLine($"Saving Order: {order}");

string result = repository[order.Id.ToString()];
Order orderLoaded = Order.CreateFromJson(result);
Console.WriteLine($"Getting Order: {orderLoaded}");

repository.Remove(order.Id.ToString());
Console.WriteLine($"Deleting Order: {order}");