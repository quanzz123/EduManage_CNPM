using System;
using System.Collections.Generic;

namespace eduManage.Models;

public partial class TblClassMember
{
    public int MemberId { get; set; }

    public int ClassId { get; set; }

    public DateTime? JoinDate { get; set; }

    public string? Status { get; set; }

    public double? Progress { get; set; }

    public double? FinalScore { get; set; }

    public string? Note { get; set; }

    public int? UserId { get; set; }

    public virtual TblClass Class { get; set; } = null!;

    public virtual TblUser? User { get; set; }
}
