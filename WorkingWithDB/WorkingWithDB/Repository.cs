using MongoDB.Bson;
using MongoDB.Driver;
using WorkingWithDB;

namespace WorkingWithDatabase
{
    public class Repository<T> : IRepository<T>
    {
        private readonly MongoClient client = new("mongodb://localhost:27017");
        private readonly string dbName = "bigproject";
        private readonly string collectionName = "people";
        private readonly IMongoDatabase database;
        public IMongoCollection<T> collection;
        public Repository()
        {
            database = client.GetDatabase(dbName);
            collection = database.GetCollection<T>(collectionName);
        }

        public async void SaveInDB(params T[] arrayElements)
        {
            await collection.InsertManyAsync(arrayElements);
        }

        public async Task<List<T>> ReadByFilter(FilterDefinition<T> filter)
        {
            var cursor = await collection.FindAsync(filter);
            Task<List<T>> collectionTask = Task.Run(() => cursor.ToList());
            return collectionTask.Result;
        }
        public async Task<List<T>> ReadAll()
        {
            var cursor = await collection.FindAsync(new BsonDocument());
            Task<List<T>> collectionTask = Task.Run(() => cursor.ToList());
            return collectionTask.Result;
        }
        List<T> ReadAll2()
        {
            return ReadByFilter(new BsonDocument()).Result;
        }


        public void ReplaceOne(BsonDocument element, BsonElement keyWithValue)
        {
            try
            {
                if (element.Contains(keyWithValue.Name))
                {
                    element.SetElement(keyWithValue);
                    T bsonForEdit = (T)Convert.ChangeType(element, typeof(T));
                    collection.FindOneAndReplace(element.ToBsonDocument(), bsonForEdit);
                }
                else Console.WriteLine("Replace Error");
            }
            catch
            {
                Console.WriteLine("Replace Error");
            }
        }

        public void DeleteOne(BsonDocument filter)
        {
            collection.DeleteOne(filter);
        }

        public void DeleteMany(BsonDocument filter)
        {
            collection.DeleteManyAsync(filter);
        }

    }
}
