using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using OrderProcessorSample.Models;
using OrderProcessorSample.Repositories;
using OrderProcessorSample.Services;
using Xunit;

namespace OrderProcessorSample.Tests;

public class OrderMessageProcessorTests
{
    private readonly Mock<IOrderRepository> _repositoryMock;
    private readonly OrderMessageProcessor _processor;

    public OrderMessageProcessorTests()
    {
        _repositoryMock = new Mock<IOrderRepository>();
        var logger = new NullLogger<OrderMessageProcessor>();
        _processor = new OrderMessageProcessor(_repositoryMock.Object, logger);
    }

    [Fact]
    public async Task ProcessAsync_ValidMessage_ReturnsTrue_AndSavesOrder()
    {
        var message = """
        {
          "orderId": 1001,
          "customerName": "Michael Yang",
          "totalAmount": 120.50,
          "timestamp": "2026-03-16"
        }
        """;

        var result = await _processor.ProcessAsync(message);

        Assert.True(result);

        _repositoryMock.Verify(r => r.SaveAsync(It.Is<Order>(o =>
            o.OrderId == 1001 &&
            o.CustomerName == "Michael Yang" &&
            o.TotalAmount == 120.50m
        )), Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_EmptyMessage_ReturnsFalse()
    {
        var result = await _processor.ProcessAsync(string.Empty);

        Assert.False(result);
        _repositoryMock.Verify(r => r.SaveAsync(It.IsAny<Order>()), Times.Never);
    }

    [Fact]
    public async Task ProcessAsync_MalformedJson_ReturnsFalse()
    {
        var message = "{ invalid-json ";

        var result = await _processor.ProcessAsync(message);

        Assert.False(result);
        _repositoryMock.Verify(r => r.SaveAsync(It.IsAny<Order>()), Times.Never);
    }

    [Fact]
    public async Task ProcessAsync_InvalidOrder_ReturnsFalse()
    {
        var message = """
        {
          "orderId": 0,
          "customerName": "Michael Yang",
          "totalAmount": -10,
          "timestamp": "2026-03-16"
        }
        """;

        var result = await _processor.ProcessAsync(message);

        Assert.False(result);
        _repositoryMock.Verify(r => r.SaveAsync(It.IsAny<Order>()), Times.Never);
    }

    [Fact]
    public async Task ProcessAsync_NullJson_ReturnsFalse()
    {
        var message = "null";

        var result = await _processor.ProcessAsync(message);

        Assert.False(result);
        _repositoryMock.Verify(r => r.SaveAsync(It.IsAny<Order>()), Times.Never);
    }
}
