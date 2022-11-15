using System;
using System.Collections.Generic;

namespace Domain;

public partial class TbUserType
{
    public int UserTypeId { get; set; }

    public string UserTypeName { get; set; } = null!;

    public virtual ICollection<TabUsersDetail> TabUsersDetails { get; } = new List<TabUsersDetail>();
}
