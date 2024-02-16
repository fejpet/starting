
Dictionary<string, Order> repository = new();

Order order = new ("Some value");

repository[order.Id.ToString()] = order;
Console.WriteLine($"Saving Order: {order}");

Order result = repository[order.Id.ToString()];
Console.WriteLine($"Getting Order: {result}");

repository.Remove(order.Id.ToString());
Console.WriteLine($"Deleting Order: {order}");