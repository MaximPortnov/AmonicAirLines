using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmonicAirLines.Classes
{
    internal class Ticket
    {
        public int UserID {  get; set; }
        public List<int> ScheduleID { get; set; }
        public int CabinTypeID {  get; set; }
        public List<Passenger> Passengers { get; set; }
    }
}
