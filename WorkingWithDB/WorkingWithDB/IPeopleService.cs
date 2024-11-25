using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkingWithDB
{
    public interface IPeopleService
    {
        int PeopleCount { get; }
        bool CreateNewPeople();
        void CreateNewRandomPeople();
        void Search();
        bool SearchByName();
        void SearchByAgeCompare();
        void PeopleEdit(BsonDocument filter);
        void WriteAllPeopleAsync();
        void WritePeopleAsync(FilterDefinition<People> filter);
    }
}
