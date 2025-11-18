using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eduManage.Models;

[Table("tblAttendanceRecords")]
public partial class TblAttendanceRecord
{
    [Key]
    public int RecordId { get; set; }

    public int SessionId { get; set; }

    public int UserId { get; set; }

    public int StatusId { get; set; }

    [StringLength(250)]
    public string? Note { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? RecordedAt { get; set; }

    [ForeignKey("SessionId")]
    [InverseProperty("TblAttendanceRecords")]
    public virtual TblAttendanceSession Session { get; set; } = null!;

    [ForeignKey("StatusId")]
    [InverseProperty("TblAttendanceRecords")]
    public virtual TblAttendanceStatus Status { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("TblAttendanceRecords")]
    public virtual TblUser User { get; set; } = null!;
}
