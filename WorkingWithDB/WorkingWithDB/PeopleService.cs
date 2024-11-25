using MongoDB.Bson;
using MongoDB.Driver;
using WorkingWithDatabase;


namespace WorkingWithDB
{
    public class PeopleService(IRepository<People> repository) : IPeopleService
    {
        private IRepository<People> peopleRepository = repository;
        public int PeopleCount { get => peopleRepository.ReadAll().Result.Count; }


        
        public async void WriteAllPeopleAsync()
        {
            var cursor = await peopleRepository.ReadAll();
            var peopleList = cursor.ToList();
            foreach (var people in peopleList)
            {
                Console.WriteLine($"{people.Name} is {people.Age} years old and has {people.Balance}$");
            }
        }

        public async void WritePeopleAsync(FilterDefinition<People> filter)
        {
            var peopleList = await peopleRepository.ReadByFilter(filter);
            if(peopleList.Count == 0)
            {
                Console.WriteLine("There is no result by this filter");
            }
            foreach (var people in peopleList)
            {
                Console.WriteLine($"{people.Name} is {people.Age} years old and has {people.Balance}$");
            }
        }


        public bool CreateNewPeople()
        {
            People human = new People();

            Console.Write("Name: ");
            bool Try = Reader.Try(out string? nameResult);
            if(!Try) return false;

            Console.Write("Age: ");
            Try = Reader.Try(out int ageResult, x => x > 0 && x < 120);
            if(!Try) return false;

            Console.Write("Balance: ");
            Try = Reader.Try(out int balanceResult);
            if(!Try) return false;

            human.Name = nameResult;
            human.Age = ageResult;
            human.Balance = balanceResult;

            Console.WriteLine($"Save: {human.Name} {human.Age} years old with {human.Balance}$");
            peopleRepository.SaveInDB(human);
            return true;
        }


        public void CreateNewRandomPeople()
        {
            int numberOfPeople = 1;
            Console.Write("Number of people: ");
            if (Reader.Try(out int result, x => x > 0))
            {
                numberOfPeople = result;
            }
            List<People> peopleList = new List<People>();
            for (int p = numberOfPeople; p > 0; p--)
            {
                Random random = new Random();
                var chars = "abcdefghijklmnopqrstuvwxyz";
                var name = chars[random.Next(chars.Length)].ToString().ToUpper();
                for (int j = 0; j < 4; j++)
                {
                    name += chars[random.Next(chars.Length)];
                }
                var age = random.Next(1, 100);
                var balance = random.Next(100, 1000);
                People human = new People() { Name = name, Age = age, Balance = balance };
                peopleList.Add(human);
            }
            peopleRepository.SaveInDB(peopleList.ToArray());
            Console.WriteLine("Randoming is over");
        }

        public void PeopleEdit(BsonDocument filter)
        {
            var peopleList = peopleRepository.ReadByFilter(filter).Result;
            var human = peopleList.FirstOrDefault();
            Console.WriteLine("Edit Name||Age||Balance");
            string[] peopleKeys = { "Name", "Age", "Balance" };
            if (Reader.Try(out string? key, line => peopleKeys.Any(peopleKey => peopleKey == line)))
            {
                Console.Write($"Editing {key}:");
                if (Reader.Try(out BsonValue? value))
                {
                    BsonElement bsonElement = new BsonElement(key, value);
                    peopleRepository.ReplaceOne(human.ToBsonDocument(), bsonElement);
                }
            }
        }


        public void Search()
        {
            Console.WriteLine("Search by Name/Age/Balance");
            string[] peopleKeys = { "Name", "Age", "Balance" };
            if (Reader.Try(out string? key, line => peopleKeys.Any(peopleKey => peopleKey == line)))
            {
                Console.Write($"Searching by {key}:");
                BsonValue value = 0;
                switch (key)
                {
                    case "Name":
                        if (Reader.Try(out string? stringValue))
                        {
                            value = stringValue;
                        }
                        break;

                    case "Age" or "Balance":
                        if (Reader.Try(out int intValue))
                        {
                            value = intValue;
                        }
                        break;
                }
                var filter = new BsonDocument(key, value);
                WritePeopleAsync(filter);
            }
            Console.WriteLine("Back to Menu");
        }

        public bool SearchByName()
        {
            Reader.TryWhile(out string containsString);
            Console.WriteLine("Search in Progress");
            var builder = Builders<People>.Filter;
            var filter = builder.Where(human => human.Name.Contains(containsString));
            WritePeopleAsync(filter);
            return true;
        }
        public void SearchByAgeCompare()
        {
            Console.Write("Write age with compare symbol. Ex >15, <=20: ");
            Reader.TryWhile(out Compare compare);
            var filter = new BsonDocument("Age", compare.ToBsonDocument());
            WritePeopleAsync(filter);
        }
    }
}