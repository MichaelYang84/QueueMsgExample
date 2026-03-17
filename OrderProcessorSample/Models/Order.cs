namespace OrderProcessorSample.Models;

public class Order
{
    public int OrderId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public DateTime Timestamp { get; set; }
}
