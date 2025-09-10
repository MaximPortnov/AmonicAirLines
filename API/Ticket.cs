namespace API
{
    public class Passenger
    {
        public DateTime birthdate { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string passportNumber { get; set; }
        public string phone { get; set; }
        public int passportCountryID { get; set; }

    }
    public class MyTicket
    {
        public int UserID { get; set; }
        public List<int> ScheduleID { get; set; }
        public int CabinTypeID { get; set; }
        public List<Passenger> Passengers { get; set; }
    }
}
