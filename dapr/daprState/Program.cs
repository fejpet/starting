
Dictionary<string, string> repository = new();

Order order = new (Guid.NewGuid().ToString(), "Some value");

repository[order.Id] = order.ToString();
Console.WriteLine($"Saving Order: {order}");

Order result = Order.CreateFromJson(repository[order.Id]);
Console.WriteLine($"Getting Order: id:{result.Id} value:{result.Value}");

repository.Remove(order.Id);
Console.WriteLine($"Deleting Order: {order}");