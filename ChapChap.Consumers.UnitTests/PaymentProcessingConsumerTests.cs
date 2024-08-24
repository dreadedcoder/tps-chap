using Moq;
using Microsoft.Extensions.Logging;
using MassTransit;
using ChapChap.Consumers.Messages;
using ChapChap.Payments;
using ChapChap.Consumers.Data;
using ChapChap.Consumers.Services;

namespace ChapChap.Consumers.UnitTests
{
    public class PaymentProcessingConsumerTests
    {
        private readonly Mock<ILogger<PaymentProcessingConsumer>> _loggerMock;
        private readonly Mock<ITransactionRepository> _txnRepositoryMock;
        private readonly Mock<IPaymentClient> _paymentClientMock;
        private readonly PaymentProcessingConsumer _consumer;

        public PaymentProcessingConsumerTests()
        {
            _loggerMock = new Mock<ILogger<PaymentProcessingConsumer>>();
            _txnRepositoryMock = new Mock<ITransactionRepository>();
            _paymentClientMock = new Mock<IPaymentClient>();

            _consumer = new PaymentProcessingConsumer(
                _loggerMock.Object,
                _txnRepositoryMock.Object,
                _paymentClientMock.Object);
        }

        [Fact]
        public async Task Consume_SuccessfulTransaction_LogsAndSavesTransaction()
        {
            // Arrange
            var message = new PaymentMessage
            {
                Amount = 100.0m,
                ReferenceId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            var contextMock = new Mock<ConsumeContext<PaymentMessage>>();
            contextMock.Setup(x => x.Message).Returns(message);

            var grpcResponse = new TransactionResponse
            {
                Status = TransactionResponse.Types.Status.Success,
                Message = "Transaction Completed Successfully"
            };

            _paymentClientMock
                .Setup(x => x.CreatePaymentTransaction(It.IsAny<TransactionRequest>()))
                .ReturnsAsync(grpcResponse);

            // Act
            await _consumer.Consume(contextMock.Object);

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Consumr received PaymentMessage for ReferenceId")),
                    null, 
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                Times.Once);


            _paymentClientMock.Verify(
                x => x.CreatePaymentTransaction(It.IsAny<TransactionRequest>()), Times.Once);

            _txnRepositoryMock.Verify(
                x => x.AddTransactionAsync(It.Is<Transaction>(t =>
                    t.Amount == message.Amount &&
                    t.ReferenceId == message.ReferenceId &&
                    t.UserId == message.UserId &&
                    t.Status == grpcResponse.Status.ToString() &&
                    t.Message == grpcResponse.Message)), Times.Once);
        }

        [Fact]
        public async Task Consume_GrpcClientThrowsException_LogsError()
        {
            // Arrange
            var message = new PaymentMessage
            {
                Amount = 100.0m,
                ReferenceId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            var contextMock = new Mock<ConsumeContext<PaymentMessage>>();
            contextMock.Setup(x => x.Message).Returns(message);

            _paymentClientMock
                .Setup(x => x.CreatePaymentTransaction(It.IsAny<TransactionRequest>()))
                .ThrowsAsync(new Exception("gRPC error"));

            // Act
            await _consumer.Consume(contextMock.Object);

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(
                        $"Error in processing transaction with {message.ReferenceId} for {message.UserId}")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                Times.Once);


            _txnRepositoryMock.Verify(x => x.AddTransactionAsync(It.IsAny<Transaction>()), Times.Never);
        }

        [Fact]
        public async Task Consume_RepositoryThrowsException_LogsError()
        {
            // Arrange
            var message = new PaymentMessage
            {
                Amount = 100.0m,
                ReferenceId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            var contextMock = new Mock<ConsumeContext<PaymentMessage>>();
            contextMock.Setup(x => x.Message).Returns(message);

            var grpcResponse = new TransactionResponse
            {
                Status = TransactionResponse.Types.Status.Success,
                Message = "Transaction Completed Successfully"
            };

            _paymentClientMock
                .Setup(x => x.CreatePaymentTransaction(It.IsAny<TransactionRequest>()))
                .ReturnsAsync(grpcResponse);

            _txnRepositoryMock
                .Setup(x => x.AddTransactionAsync(It.IsAny<Transaction>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            await _consumer.Consume(contextMock.Object);

            // Assert 
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(
                        $"Error in processing transaction with {message.ReferenceId} for {message.UserId}")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                Times.Once);
        }
    }
}