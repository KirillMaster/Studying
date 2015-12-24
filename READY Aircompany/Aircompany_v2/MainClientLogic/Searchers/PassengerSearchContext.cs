using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport
{
    class PassengerSearchContext
    {
        PassengerInfo info;

        List<PassengerInfo> passengers;
        AirportEntities databaseContext;
        public PassengerSearchContext()
        {
            passengers = new List<PassengerInfo>();
            databaseContext = new AirportEntities();

        }
        private bool PredicateToPassengerSearch(Passenger passenger)
        {
            int neededCountOfInitializedFields = 0;
            int currentCountOfEqualFields = 0;

            if (info.PassportID != 0) neededCountOfInitializedFields++;
            if (info.BirthDate != null) neededCountOfInitializedFields++;
            if (info.FamilyName != null) neededCountOfInitializedFields++;
            if (info.Name != null) neededCountOfInitializedFields++;
            if (info.Surname != null) neededCountOfInitializedFields++;
            if (info.Sex != "") neededCountOfInitializedFields++;

            if (passenger.ID_passport == info.PassportID) currentCountOfEqualFields++;
            if (passenger.BirthDate == info.BirthDate) currentCountOfEqualFields++;
            string[] fio = passenger.fio.Split(' ');
            string family = fio[0];
            string name = fio[1];
            string surname = fio[2];

            if (family.Equals(info.FamilyName)) currentCountOfEqualFields++;
            if (name.Equals(info.Name)) currentCountOfEqualFields++;
            if (surname.Equals(info.Surname)) currentCountOfEqualFields++;

          
            if (passenger.sex.Trim().Equals(info.Sex.Trim())) currentCountOfEqualFields++;
            if (neededCountOfInitializedFields == 0) return false;
            if (currentCountOfEqualFields == 0) return false;
            if (currentCountOfEqualFields == neededCountOfInitializedFields) return true;

            return false;


        }
        public List<PassengerInfo> SearchPassenger(PassengerInfo info)
        {

            passengers.Clear();
            Func<Passenger, bool> predicate = PredicateToPassengerSearch;
            this.info = info;
            var res = databaseContext.Passenger.Select(item => item).Where(predicate);
            string surname;
            string name;
            string family;
            string[] mas;
            bool sex;
            foreach (var item in res)
            {
                mas = item.fio.Split(' ');
                surname = mas[2];
                name = mas[1];
                family = mas[0];
             
                passengers.Add(new PassengerInfo(item.ID_passport, surname, name, family, item.BirthDate.Value, item.sex));
            }
            return passengers;

        }
    }
}
