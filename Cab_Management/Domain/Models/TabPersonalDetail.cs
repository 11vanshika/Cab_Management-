using System;
using System.Collections.Generic;

namespace Domain;

public partial class TabPersonalDetail
{
    public int PersonalId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int? Age { get; set; }

    public string Gender { get; set; } = null!;

    public long ContactNumber { get; set; }

    public string EmailId { get; set; } = null!;

    public int AddressDetails { get; set; }

    public virtual TabAddress AddressDetailsNavigation { get; set; } = null!;

    public virtual TabUsersDetail Email { get; set; } = null!;
}
