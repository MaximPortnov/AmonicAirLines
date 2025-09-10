using System;
using System.Collections.Generic;

namespace API.Models;

public partial class CabinType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<FlightReport> FlightReports { get; set; } = new List<FlightReport>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual ICollection<Amenity> Amenities { get; set; } = new List<Amenity>();
}
