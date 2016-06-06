using Core;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace MongoTests
{
    public class CustomIdSerializer : SerializerBase<Id>
    {
        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Id value)
        {
            if (context.Writer.State == MongoDB.Bson.IO.BsonWriterState.Name)
            {
                context.Writer.WriteName("_id");
            }
            if (context.Writer.State == MongoDB.Bson.IO.BsonWriterState.Value)
            {
                context.Writer.WriteObjectId(ObjectId.Parse(value.Value));
            }
        }

        public override Id Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var _id = context.Reader.ReadObjectId();

            var id = new Id(_id.ToString());

            return id;
        }
    }
}
