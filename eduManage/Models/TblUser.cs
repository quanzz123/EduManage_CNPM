using System;
using System.Collections.Generic;

namespace eduManage.Models;

public partial class TblUser
{
    public int UserId { get; set; }

    public int? RoleId { get; set; }

    public string UserName { get; set; } = null!;

    public string PassworkHash { get; set; } = null!;

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public DateTime? CreateDate { get; set; }

    public bool? IsActive { get; set; }

    public virtual TblRole? Role { get; set; }

    public virtual ICollection<TblClassMember> TblClassMembers { get; set; } = new List<TblClassMember>();

    public virtual ICollection<TblClass> TblClasses { get; set; } = new List<TblClass>();
}
