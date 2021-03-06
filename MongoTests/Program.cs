﻿using Core;
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

            collection.Add(model);

            Task.Delay(10).Wait();

            collection.Update(model);

            var modelFetched = collection.GetById(model.Id);

            Console.WriteLine($"{model.Id} == {modelFetched.Id}");
            Console.WriteLine($"{modelFetched.DateCreated.Ticks} != {modelFetched.DateUpdated.Ticks}");
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
