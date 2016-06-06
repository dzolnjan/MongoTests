using Core;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System;

namespace MongoTests
{
    public class IdGuidSerializer : SerializerBase<IdGuid>
    {
        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, IdGuid value)
        {
            if (context.Writer.State == MongoDB.Bson.IO.BsonWriterState.Name)
            {
                context.Writer.WriteName("_id");
            }
            if (context.Writer.State == MongoDB.Bson.IO.BsonWriterState.Value)
            {
                context.Writer.WriteString(value.Value.ToString());
            }
        }

        public override IdGuid Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var _id = context.Reader.ReadString();

            var id = new IdGuid(Guid.Parse(_id));

            return id;
        }
    }
}
