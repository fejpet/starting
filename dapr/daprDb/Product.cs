namespace daprDb;

public class Product
{
    public int Id { get; set; }
    public DateOnly Available {get;set;}
    public string Name {get;set;}

    public float Price {get;set;}
    public string Currency {get;set;}

    public string? Details { get; set; }
}
