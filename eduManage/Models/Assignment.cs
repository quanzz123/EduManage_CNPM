using System;
using System.Collections.Generic;

namespace eduManage.Models;

public partial class Assignment
{
    public int AssignmentId { get; set; }

    public int ClassId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? FileUrl { get; set; }

    public DateTime? Deadline { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifyDate { get; set; }

    public bool? IsActive { get; set; }

    public virtual TblClass Class { get; set; } = null!;

    public virtual ICollection<Submission> Submissions { get; set; } = new List<Submission>();
}
