using System;
using System.Collections.Generic;

namespace Domain;

public partial class TbBooking
{
    public int BookingId { get; set; }

    public int? CabId { get; set; }

    public int? UserId { get; set; }

    public int? TripId { get; set; }

    public string? ScheduleDate { get; set; }

    public string? ScheduleTime { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int? Status { get; set; }

    public virtual TbCabDetail? Cab { get; set; }

    public virtual TbTripDetail? Trip { get; set; }

    public virtual TbUser? User { get; set; }
}
