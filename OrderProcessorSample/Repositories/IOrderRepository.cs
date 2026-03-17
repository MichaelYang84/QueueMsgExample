using OrderProcessorSample.Models;

namespace OrderProcessorSample.Repositories;

public interface IOrderRepository
{
    Task SaveAsync(Order order);
}
