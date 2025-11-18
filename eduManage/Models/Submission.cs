using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eduManage.Models;

public partial class Submission
{
    [Key]
    public int SubmissionId { get; set; }

    public int AssignmentId { get; set; }

    public int StudentId { get; set; }

    public string? FileUrl { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? SubmitDate { get; set; }

    public double? Score { get; set; }

    public string? Feedback { get; set; }

    [StringLength(50)]
    public string? Status { get; set; }

    [ForeignKey("AssignmentId")]
    [InverseProperty("Submissions")]
    public virtual Assignment Assignment { get; set; } = null!;

    [ForeignKey("AssignmentId")]
    [InverseProperty("Submissions")]
    public virtual TblUser AssignmentNavigation { get; set; } = null!;
}
