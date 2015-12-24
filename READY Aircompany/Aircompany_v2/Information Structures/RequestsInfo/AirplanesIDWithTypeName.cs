using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport
{
    class AirplanesIDWithTypeName
    {
        public int ID_airplane { get; set; }
        public string name { get; set; }


        public AirplanesIDWithTypeName(int airplaneID,  string name)
        {
            ID_airplane = airplaneID;
            this.name = name;
        }
    }
}
