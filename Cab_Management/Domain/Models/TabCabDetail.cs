using System;
using System.Collections.Generic;

namespace Domain;

public partial class TabCabDetail
{
    public int Cabid { get; set; }

    public string RegistrationNun { get; set; } = null!;

    public int? CabTypeId { get; set; }

    public virtual TabCabType? CabType { get; set; }

    public virtual ICollection<TabDriverDetail> TabDriverDetails { get; } = new List<TabDriverDetail>();
}
