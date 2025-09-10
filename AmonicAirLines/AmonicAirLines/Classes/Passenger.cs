using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmonicAirLines.Classes
{
    internal class Passenger
    {
        public DateTime birthdate { get; set;  }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string passportNumber { get; set; }
        public string phone {  get; set; }
        public int passportCountryID { get; set; }

    }
}
