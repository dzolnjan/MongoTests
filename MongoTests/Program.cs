using Core;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace MongoTests
{
    class Program
    {
        static void Main(string[] args)
        {
            // Register Id type serializer and custom class maps
            BsonSerializer.RegisterSerializer(typeof(Id), new CustomIdSerializer());
            BsonSerializer.RegisterSerializer(typeof(IdGuid), new IdGuidSerializer());
            BsonClassMap.RegisterClassMap<ModelBase>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
                cm.UnmapMember(c => c.DontPutMeInDb);
            });

            var db = GetDatabase();

            var collection = db.GetCollection<Model>("model");

            var model = new Model() { SomeProp = Guid.NewGuid().ToString(), IdGuidProp = IdGuid.Generate() };

            collection
                    .InsertOneAsync(model)
                    .Wait();

            Task.Delay(10).Wait();

            collection.Update(model);

            //var filter = Builders<Model>.Filter.Where(x => x.Id == new Id(model.Id));
            var filter = Builders<Model>.Filter.Where(x => x.Id == model.Id);
            var modelFetched = collection
                .Find(filter)
                .SingleOrDefaultAsync()
                .Result;

            Console.WriteLine($"{model.Id} == {modelFetched.Id}");
            Console.WriteLine($"{model.DateUpdated.Ticks} != {modelFetched.DateUpdated.Ticks}");
            Console.ReadKey();
        }

        public static IMongoDatabase GetDatabase()
        {
            var _client = new MongoClient("mongodb://localhost:27017/tests");
            var _database = _client.GetDatabase("tests");
            return _database;
        }
    }
}
