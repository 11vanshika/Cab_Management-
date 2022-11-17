using System;
using System.Collections.Generic;

namespace Domain;

public partial class TbUserRole
{
    public int UserRoleId { get; set; }

    public string? UserRoleName { get; set; }

    public virtual ICollection<TbUser> TbUsers { get; } = new List<TbUser>();
}
