using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport
{
    public class PassengerInfo : Info
    {
        public int? PassportID { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string FamilyName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Sex { get; set; }

        public PassengerInfo(int? passwordID, string surname, string name, string familyName, DateTime? birthDate, string sex)
        {
            PassportID = passwordID;
            Surname = surname;
            Name = name;
            FamilyName = familyName;
            BirthDate = birthDate;
            if (sex != null)
                Sex = sex;
            else Sex = "";
        }

    }
}

