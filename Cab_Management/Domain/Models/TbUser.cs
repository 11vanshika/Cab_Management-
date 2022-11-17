using System;
using System.Collections.Generic;

namespace Domain;

public partial class TbUser
{
    public int UserId { get; set; }

    public string? EmailId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string Password { get; set; } = null!;

    public int? UserRoleId { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int? Status { get; set; }

    public virtual TbUserRole? UserRole { get; set; }
}
