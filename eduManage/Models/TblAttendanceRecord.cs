using System;
using System.Collections.Generic;
<<<<<<< HEAD

namespace eduManage.Models;

public partial class TblAttendanceRecord
{
=======
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eduManage.Models;

[Table("tblAttendanceRecords")]
public partial class TblAttendanceRecord
{
    [Key]
>>>>>>> 094386d1a1a01a0e43ad1b03c56e72832ca52cdc
    public int RecordId { get; set; }

    public int SessionId { get; set; }

    public int UserId { get; set; }

    public int StatusId { get; set; }

<<<<<<< HEAD
    public string? Note { get; set; }

    public DateTime? RecordedAt { get; set; }

    public virtual TblAttendanceSession Session { get; set; } = null!;

    public virtual TblAttendanceStatus Status { get; set; } = null!;

=======
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
>>>>>>> 094386d1a1a01a0e43ad1b03c56e72832ca52cdc
    public virtual TblUser User { get; set; } = null!;
}
