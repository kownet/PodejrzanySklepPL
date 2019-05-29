using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Pspl.Shared.Models
{
    public class Ad
    {
        [BsonId]
        public ObjectId InternalId { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
    }
}