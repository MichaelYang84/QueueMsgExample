using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using OrderProcessorSample.Services;

namespace OrderProcessorSample.Functions;

public class OrderQueueFunction
{
    private readonly OrderMessageProcessor _processor;
    private readonly ILogger<OrderQueueFunction> _logger;

    public OrderQueueFunction(
        OrderMessageProcessor processor,
        ILogger<OrderQueueFunction> logger)
    {
        _processor = processor;
        _logger = logger;
    }

    [Function("ProcessOrderQueueMessage")]
    public async Task Run(
        [ServiceBusTrigger("orders-queue", Connection = "ServiceBusConnection")]
        string message)
    {
        _logger.LogInformation("Received Service Bus message.");

        var success = await _processor.ProcessAsync(message);

        if (!success)
        {
            _logger.LogWarning("Message processing failed.");
        }
    }
}
