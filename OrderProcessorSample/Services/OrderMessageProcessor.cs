using System.Text.Json;
using Microsoft.Extensions.Logging;
using OrderProcessorSample.Models;
using OrderProcessorSample.Repositories;

namespace OrderProcessorSample.Services;

public class OrderMessageProcessor
{
    private readonly IOrderRepository _repository;
    private readonly ILogger<OrderMessageProcessor> _logger;

    public OrderMessageProcessor(
        IOrderRepository repository,
        ILogger<OrderMessageProcessor> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<bool> ProcessAsync(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            _logger.LogWarning("Received empty message.");
            return false;
        }

        Order? order;
        try
        {
            order = JsonSerializer.Deserialize<Order>(
                message,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Malformed JSON message: {Message}", message);
            return false;
        }

        if (order is null)
        {
            _logger.LogWarning("Deserialized order is null.");
            return false;
        }

        if (!IsValid(order))
        {
            _logger.LogWarning("Invalid order message. OrderId: {OrderId}", order.OrderId);
            return false;
        }

        await _repository.SaveAsync(order);

        _logger.LogInformation(
            "Order processed successfully. OrderId: {OrderId}, CustomerName: {CustomerName}, TotalAmount: {TotalAmount}",
            order.OrderId,
            order.CustomerName,
            order.TotalAmount);

        return true;
    }

    private static bool IsValid(Order order)
    {
        if (order.OrderId == 0)
            return false;

        if (string.IsNullOrWhiteSpace(order.CustomerName))
            return false;

        if (order.TotalAmount <= 0)
            return false;

        if (order.Timestamp == default)
            return false;

        return true;
    }
}
