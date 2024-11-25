using MongoDB.Bson.Serialization.Attributes;

namespace WorkingWithDB
{
    [BsonIgnoreExtraElements]
    public class People
    {
        public string Name = "Unknown";
        public int Age = 0;
        public int Balance = 0;
    }

}
