using Grpc.Core;
using ChapChap.gRPC.Services;
using ChapChap.Payments;

namespace ChapChap.gRPC.UnitTests
{
    public class MakePaymentServiceTests
    {
        [Fact]
        public async Task CreateTransaction_ShouldReturnSuccessResponse()
        {
            // Arrange
            var service = new MakePaymentService();
            var request = new TransactionRequest
            {
                ReferenceId = Guid.NewGuid().ToString()
            };
            var callContext = new DefaultCallContext();

            // Act
            var response = await service.CreateTransaction(request, callContext);

            // Assert
            Assert.NotNull(response);
            Assert.Equal($"Transaction withh ReferenceId: {request.ReferenceId} processed successfully",
                response.Message);
            Assert.Equal(TransactionResponse.Types.Status.Success, response.Status);
        }
    }

    // Dummy implementation of ServerCallContext for testing purposes
    public class DefaultCallContext : ServerCallContext
    {

        protected override Task WriteResponseHeadersAsyncCore(Metadata responseHeaders)
        {
            throw new NotImplementedException();
        }

        protected override ContextPropagationToken CreatePropagationTokenCore(ContextPropagationOptions? options)
        {
            throw new NotImplementedException();
        }


        protected override string MethodCore => throw new NotImplementedException();

        protected override string HostCore => throw new NotImplementedException();

        protected override string PeerCore => throw new NotImplementedException();

        protected override DateTime DeadlineCore => throw new NotImplementedException();

        protected override Metadata RequestHeadersCore => throw new NotImplementedException();

        protected override CancellationToken CancellationTokenCore => throw new NotImplementedException();

        protected override Metadata ResponseTrailersCore => throw new NotImplementedException();

        protected override Status StatusCore { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        protected override WriteOptions? WriteOptionsCore { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        protected override AuthContext AuthContextCore => throw new NotImplementedException();
    }

}