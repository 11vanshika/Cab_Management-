using System;
using System.Collections.Generic;

namespace Domain;

public partial class BookingView
{
    public int BookingId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? RegistrationNun { get; set; }

    public string? CabName { get; set; }

    public string? ScheduleDate { get; set; }

    public string? ScheduleTime { get; set; }

    public string? SourceAddress { get; set; }

    public string? DestinationAddress { get; set; }

    public double? Distance { get; set; }

    public long? TotalFare { get; set; }
}
