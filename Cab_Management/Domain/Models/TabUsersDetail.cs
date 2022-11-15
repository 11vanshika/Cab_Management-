using System;
using System.Collections.Generic;

namespace Domain;

public partial class TabUsersDetail
{
    public int UserId { get; set; }

    public string EmailId { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int UserType { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int Status { get; set; }

    public virtual ICollection<TabPersonalDetail> TabPersonalDetails { get; } = new List<TabPersonalDetail>();

    public virtual TbUserType UserTypeNavigation { get; set; } = null!;
}
