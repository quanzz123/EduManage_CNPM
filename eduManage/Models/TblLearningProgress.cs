using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eduManage.Models;

[Table("tblLearningProgress")]
public partial class TblLearningProgress
{
    [Key]
    public int ProgressId { get; set; }

    public int UserId { get; set; }

    public int LessonId { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? CompletionRate { get; set; }

    public bool? IsCompleted { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastAccessTime { get; set; }

    public int? TotalTimeSpent { get; set; }

    public int LastContentId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedDate { get; set; }

    [ForeignKey("LastContentId")]
    [InverseProperty("TblLearningProgresses")]
    public virtual TblLessionContent LastContent { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("TblLearningProgresses")]
    public virtual TblUser User { get; set; } = null!;
}
