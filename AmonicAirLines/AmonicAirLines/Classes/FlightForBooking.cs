using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmonicAirLines.Classes
{
    public class FlightForBooking
    {
        public List<int> Ids { get; set; } = new List<int>();
        public string From { get; set; }
        public string To { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public string FlightNumbers { get; set; }
        public decimal CabinPrice { get; set; }
        public int NumberOfStops { get; set; }
    }
}
