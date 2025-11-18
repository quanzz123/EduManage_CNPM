using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eduManage.Models;

[Table("tblAttendanceSessions")]
public partial class TblAttendanceSession
{
    [Key]
    public int SessionsId { get; set; }

    public int ClassId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? SessionDate { get; set; }

    public int? UserId { get; set; }

    [ForeignKey("ClassId")]
    [InverseProperty("TblAttendanceSessions")]
    public virtual TblClass Class { get; set; } = null!;

    [InverseProperty("Session")]
    public virtual ICollection<TblAttendanceRecord> TblAttendanceRecords { get; set; } = new List<TblAttendanceRecord>();

    [ForeignKey("UserId")]
    [InverseProperty("TblAttendanceSessions")]
    public virtual TblUser? User { get; set; }
}
