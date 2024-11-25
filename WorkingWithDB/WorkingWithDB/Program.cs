using WorkingWithDatabase;
using WorkingWithDB;
bool whiletrue = true;
IRepository<People> repository = new Repository<People>();
IPeopleService peopleService = new PeopleService(repository);

while (whiletrue)
{
    Console.WriteLine("Welcome to DB");
    Console.WriteLine("Enter one of options - search/new/nameSearch/all/rand/count/age");
    if (Reader.Try(out string? switcher))
    {
        switch (switcher)
        {
            case "new":
                peopleService.CreateNewPeople();
                Console.WriteLine("Backed to Menu");
                break;
            case "nameSearch":
                peopleService.SearchByName();
                break;
            case "search":
                peopleService.Search();
                break;
                 case "all":
                     peopleService.WriteAllPeopleAsync();
                     break;
                 case "clear" or "":
                     Console.Clear();
                     break;
                 case "rand":
                     peopleService.CreateNewRandomPeople();
                     break;
            case "count":
                Console.WriteLine(peopleService.PeopleCount);
                    break;
            case "age":
                peopleService.SearchByAgeCompare();
                break;
            default:
                Console.Clear();
                Console.WriteLine("Its not correct try again");
                break;
        }
    }
}