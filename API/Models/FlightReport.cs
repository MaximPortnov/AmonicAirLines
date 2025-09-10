using System;
using System.Collections.Generic;

namespace API.Models;

public partial class FlightReport
{
    public int Id { get; set; }

    public string? Gender { get; set; }

    public int? Age { get; set; }

    public int? CabinTypeId { get; set; }

    public int? DestinationAirportId { get; set; }

    public int? AnswerCount { get; set; }

    public string? Answer { get; set; }

    public int? Month { get; set; }

    public int? Year { get; set; }

    public virtual CabinType? CabinType { get; set; }

    public virtual Airport? DestinationAirport { get; set; }
}
