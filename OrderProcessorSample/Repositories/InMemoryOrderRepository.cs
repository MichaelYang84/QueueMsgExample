using OrderProcessorSample.Models;

namespace OrderProcessorSample.Repositories;

public class InMemoryOrderRepository : IOrderRepository
{
    private readonly List<Order> _orders = new();

    public Task SaveAsync(Order order)
    {
        _orders.Add(order);
        return Task.CompletedTask;
    }

    public IReadOnlyList<Order> GetAll() => _orders;
}
