using Moq;
using Microsoft.Extensions.Logging;
using MassTransit;
using ChapChap.Api.Models;
using ChapChap.Consumers.Messages;
using Microsoft.AspNetCore.Http.HttpResults;
using ChapChap.Api.Services;

namespace ChapChap.Api.UnitTests
{
    /// <summary>
    /// ChapChap.Api Service tests
    /// </summary>
    public class TransactionRequestServiceTests
    {
        private readonly Mock<ILogger<TransactionRequestService>> _mockLogger;
        private readonly Mock<IBus> _mockBus;
        private readonly MassTransitOptions _massTransitOptions;
        private readonly TransactionRequestService _transactionRequestService;

        public TransactionRequestServiceTests()
        {
            _mockLogger = new Mock<ILogger<TransactionRequestService>>();
            _mockBus = new Mock<IBus>();

            _massTransitOptions = new MassTransitOptions
            {
                RabbitMQ = new Models.RabbitMQ
                {
                    Host = "rabbitmq://localhost",
                    TransactionQueue = "api-txn-processing-queue"
                }
            };

            _transactionRequestService = new TransactionRequestService(_mockBus.Object, _massTransitOptions,
                _mockLogger.Object);
        }

        [Fact]
        public async Task ProcessTransactionAsync_ReturnsBadRequest_WhenAmountIsLessThanOrEqualToZero()
        {
            //arrange
            var request = new TransactionRequest(Guid.NewGuid(), Guid.NewGuid(), 0M);

            //act
            var result = await _transactionRequestService.ProcessTransactionRequestAsync(request);

            //assert
            Assert.IsType<BadRequest<string>>(result);
            var badRequestResult = result as BadRequest<string>;

            Assert.NotNull(badRequestResult);
            Assert.Equal("Amount should be greater than 0", actual: badRequestResult.Value);
        }

        [Fact]
        public async Task ProcessTransactionAsync_SendsMessageToQueue_WhenRequestIsValid()
        {
            //arrange
            var request = new TransactionRequest(UserId: Guid.NewGuid(), ReferenceId: Guid.NewGuid(), 500M);
            var mockSendEndpoint = new Mock<ISendEndpoint>();

            _mockBus.Setup(bus => bus.GetSendEndpoint(It.IsAny<Uri>()))
                .ReturnsAsync(mockSendEndpoint.Object);

            //act
            var result = await _transactionRequestService.ProcessTransactionRequestAsync(request);

            //assert
            Assert.IsType<Ok>(result);
            _mockBus.Verify(bus => bus.GetSendEndpoint(It.Is<Uri>(uri =>
                uri.ToString() == $"{_massTransitOptions.RabbitMQ.Host}/{_massTransitOptions.RabbitMQ.TransactionQueue}"
            )), Times.Once);

            mockSendEndpoint.Verify(endpoint => endpoint.Send(It.IsAny<PaymentMessage>(), default), Times.Once);
        }

        [Fact]
        public async Task ProcessTransactionAsync_LogsInformation_WhenRequestIsReceived()
        {
            //arrange
            var request = new TransactionRequest(UserId: Guid.NewGuid(), ReferenceId: Guid.NewGuid(), 500M);

            var mockSendEndpoint = new Mock<ISendEndpoint>();
            _mockBus.Setup(bus => bus.GetSendEndpoint(It.IsAny<Uri>()))
                .ReturnsAsync(mockSendEndpoint.Object);

            //act
            await _transactionRequestService.ProcessTransactionRequestAsync(request);

            //assert
            _mockLogger.Verify(
                logger => logger.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(), 
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Received") &&
                                      v.ToString().Contains(request.ReferenceId.ToString()) &&
                                      v.ToString().Contains(request.UserId.ToString())),
                    null,// Exception sh ould be null for an information log
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)), // Formatter can be any
                Times.Once);

        }
    }

}