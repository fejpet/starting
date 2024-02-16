public class Order
{
    public Guid Id {get;private set;}
    public String Value {get;private set;}

    public Order(String value) {
        Id = Guid.NewGuid();
        Value = value;
    }

    public override String ToString() {
        return $"Order({Id}='{Value}')";
    }
}