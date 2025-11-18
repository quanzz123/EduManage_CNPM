using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eduManage.Models;

public partial class Assignment
{
    [Key]
    public int AssignmentId { get; set; }

    [Column("ClassID")]
    public int ClassId { get; set; }

    [StringLength(250)]
    public string? Title { get; set; }

    [StringLength(250)]
    public string? Description { get; set; }

    public string? FileUrl { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Deadline { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifyDate { get; set; }

    public bool? IsActive { get; set; }

    [ForeignKey("ClassId")]
    [InverseProperty("Assignments")]
    public virtual TblClass Class { get; set; } = null!;

    [InverseProperty("Assignment")]
    public virtual ICollection<Submission> Submissions { get; set; } = new List<Submission>();
}
