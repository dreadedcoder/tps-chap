using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace ChapChap.Consumers.Data
{
    /// <summary>
    /// The Mongo db entity
    /// </summary>
    public class Transaction
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        public decimal Amount { get; set; }

        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid UserId { get; set; }

        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid ReferenceId { get; set; }

        public string Status { get; set;} = null!;

        public string Message { get; set; } = null!;
    }
}
