using Core;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MongoTests
{
    public static class MongoCollectionExtensions
    {
        /// <summary>
        /// Adds a new document to the collection
        /// </summary>
        /// <param name="document"></param>
        public static void Add<T>(this IMongoCollection<T> MongoCollection, T document) where T : ModelBase
        {
            try
            {
                MongoCollection
                    .InsertOneAsync(document)
                    .Wait();
            }
            catch (AggregateException ex)
            {
                //MongoWriteException innerException = ex.InnerException as MongoWriteException;
                //if (innerException != null && innerException.WriteError.Category == ServerErrorCategory.DuplicateKey)
                //{
                //    throw new DuplicateValueException(innerException.Message);
                //}
                throw;
            }
        }

        public static DeleteResult Delete<T>(this IMongoCollection<T> MongoCollection, Id id, WriteConcern concern = null) where T : ModelBase
        {
            return MongoCollection
                .DeleteOneAsync(ItemWithId<T>(id))
                .Result;
        }

        public static T GetById<T>(this IMongoCollection<T> MongoCollection, Id id) where T : ModelBase
        {
            return MongoCollection
                .Find(ItemWithId<T>(id))
                .SingleOrDefaultAsync()
                .Result;
        }

        public static List<T> All<T>(this IMongoCollection<T> MongoCollection) where T : ModelBase
        {
            return MongoCollection
                .Find(new BsonDocument())
                .ToListAsync()
                .Result;
        }

        /// <summary>
        /// Updates an existing document. If no document exists in the underlying
        /// connection then this method does nothing.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static ReplaceOneResult Update<T>(this IMongoCollection<T> MongoCollection, T document) where T : ModelBase
        {
            document.DateUpdated = DateTime.UtcNow;

            return MongoCollection
                .ReplaceOneAsync(
                    ItemWithId<T>(document.Id),
                    document,
                    new UpdateOptions { IsUpsert = false }
                    )
                .Result;
        }

        /// <summary>
        /// Performs an upsert, when you don't care whether you're inserting or updating an object
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static ReplaceOneResult Upsert<T>(this IMongoCollection<T> MongoCollection, T document) where T : ModelBase
        {
            return MongoCollection
                .ReplaceOneAsync(
                    ItemWithId<T>(document.Id),
                    document,
                    new UpdateOptions { IsUpsert = true }
                    )
                .Result;
        }

        /// <summary>
        /// Helper method... doesn't do much but we do this all the time so it's nice to have
        /// </summary>
        public static FilterDefinition<T> ItemWithId<T>(Id id) where T : ModelBase
        {
            return Builders<T>.Filter.Where(x => x.Id == id);
        }

        /// <summary>
        /// Helper method that creates a filter to fetch all records having id specified in the list of objectId
        /// </summary>
        /// <param name="lsId">List of ObjetId</param>
        /// <returns></returns>
        public static FilterDefinition<T> ItemWithListOfId<T>(List<Id> lsId) where T : ModelBase
        {
            return Builders<T>.Filter.In("_id", lsId);
        }

    }
}
