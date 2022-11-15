using System;
using System.Collections.Generic;

namespace Domain;

public partial class TabCabType
{
    public int CabTypeId { get; set; }

    public string CabName { get; set; } = null!;

    public virtual ICollection<TabCabDetail> TabCabDetails { get; } = new List<TabCabDetail>();
}
