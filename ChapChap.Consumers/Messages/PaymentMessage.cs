using System;

namespace ChapChap.Consumers.Messages
{
    public class PaymentMessage
    {
        public Guid UserId { get; set; }

        public Guid ReferenceId { get; set; }   

        public decimal Amount { get; set; }
    }
}


