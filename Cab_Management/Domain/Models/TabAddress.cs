using System;
using System.Collections.Generic;

namespace Domain;

public partial class TabAddress
{
    public int AddressId { get; set; }

    public string Address { get; set; } = null!;

    public string Street { get; set; } = null!;

    public string Locality { get; set; } = null!;

    public string City { get; set; } = null!;

    public string State { get; set; } = null!;

    public string Country { get; set; } = null!;

    public int PinCode { get; set; }

    public virtual ICollection<TabDriverDetail> TabDriverDetails { get; } = new List<TabDriverDetail>();

    public virtual ICollection<TabPersonalDetail> TabPersonalDetails { get; } = new List<TabPersonalDetail>();
}
