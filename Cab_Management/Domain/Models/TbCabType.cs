using System;
using System.Collections.Generic;

namespace Domain;

public partial class TbCabType
{
    public int CabTypeId { get; set; }

    public string? CabName { get; set; }

    public virtual ICollection<TbCabDetail> TbCabDetails { get; } = new List<TbCabDetail>();
}
