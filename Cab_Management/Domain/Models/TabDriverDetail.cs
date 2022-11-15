using System;
using System.Collections.Generic;

namespace Domain;

public partial class TabDriverDetail
{
    public int DriverId { get; set; }

    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public string EmailId { get; set; } = null!;

    public string? Password { get; set; }

    public int AddressDetails { get; set; }

    public long ContactNum { get; set; }

    public string Dlnumber { get; set; } = null!;

    public int? IsValidLicense { get; set; }

    public int? CabId { get; set; }

    public virtual TabAddress AddressDetailsNavigation { get; set; } = null!;

    public virtual TabCabDetail? Cab { get; set; }
}
