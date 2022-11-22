using System;
using System.Collections.Generic;

namespace Domain;

public partial class TbCabDetail
{
    public int Cabid { get; set; }

    public string? RegistrationNun { get; set; }

    public int? CabTypeId { get; set; }

    public int? UserId { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int? Status { get; set; }

    public virtual TbCabType? CabType { get; set; }

    public virtual TbUser? User { get; set; }
}
