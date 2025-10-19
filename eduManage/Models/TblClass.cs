using System;
using System.Collections.Generic;

namespace eduManage.Models;

public partial class TblClass
{
    public int ClassId { get; set; }

    public string ClassName { get; set; } = null!;

    public string? Description { get; set; }

    public string? Subject { get; set; }

    public int TeacherId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? Schedule { get; set; }

    public bool? IsActive { get; set; }

    public string? Image { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? ModifedDate { get; set; }

    public int? MaxStudents { get; set; }

    public virtual ICollection<TblClassMember> TblClassMembers { get; set; } = new List<TblClassMember>();

    public virtual TblUser Teacher { get; set; } = null!;
}
