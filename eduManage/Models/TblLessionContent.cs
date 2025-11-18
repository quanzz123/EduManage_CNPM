using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eduManage.Models;

[Table("tblLessionContent")]
public partial class TblLessionContent
{
    [Key]
    public int ContentId { get; set; }

    public int LessionId { get; set; }

    [StringLength(250)]
    public string Title { get; set; } = null!;

    [StringLength(50)]
    public string? ContentType { get; set; }

    public string? ContentUrl { get; set; }

    public int? Duration { get; set; }

    public int? OrderIdx { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateDate { get; set; }

    [ForeignKey("LessionId")]
    [InverseProperty("TblLessionContents")]
    public virtual TblLesson Lession { get; set; } = null!;

    [InverseProperty("LastContent")]
    public virtual ICollection<TblLearningProgress> TblLearningProgresses { get; set; } = new List<TblLearningProgress>();
}
