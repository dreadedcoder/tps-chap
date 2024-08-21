namespace ChapChap.Api.Models;
public record TransactionRequest(Guid UserId, Guid ReferenceId, decimal Amount)
{
    public DateTime? CreatedOn => DateTime.UtcNow;
}

