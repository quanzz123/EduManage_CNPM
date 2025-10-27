using System;
using System.Collections.Generic;

namespace eduManage.Models;

public partial class Submission
{
    public int SubmissionId { get; set; }

    public int AssignmentId { get; set; }

    public int StudentId { get; set; }

    public string? FileUrl { get; set; }

    public DateTime? SubmitDate { get; set; }

    public double? Score { get; set; }

    public string? Feedback { get; set; }

    public string? Status { get; set; }

    public virtual Assignment Assignment { get; set; } = null!;

    public virtual TblUser AssignmentNavigation { get; set; } = null!;
}
