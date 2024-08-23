using System;

namespace ChapChap.Consumers.Messages
{
    /// <summary>
    /// The Payment message sent over the MassTransit queue
    /// </summary>
    public class PaymentMessage
    {
        public Guid UserId { get; set; }

        public Guid ReferenceId { get; set; }   

        public decimal Amount { get; set; }
    }
}


