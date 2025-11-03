using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eduManage.Models;

[Table("tblClassMembers")]
public partial class TblClassMember
{
    [Key]
    [Column("MemberID")]
    public int MemberId { get; set; }

    [Column("ClassID")]
    public int ClassId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? JoinDate { get; set; }

    [StringLength(50)]
    public string? Status { get; set; }

    public double? Progress { get; set; }

    public double? FinalScore { get; set; }

    public string? Note { get; set; }

    [Column("UserID")]
    public int? UserId { get; set; }

    [ForeignKey("ClassId")]
    [InverseProperty("TblClassMembers")]
    public virtual TblClass Class { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("TblClassMembers")]
    public virtual TblUser? User { get; set; }
}
