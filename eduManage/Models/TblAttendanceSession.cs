using System;
using System.Collections.Generic;
<<<<<<< HEAD

namespace eduManage.Models;

public partial class TblAttendanceSession
{
=======
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eduManage.Models;

[Table("tblAttendanceSessions")]
public partial class TblAttendanceSession
{
    [Key]
>>>>>>> 094386d1a1a01a0e43ad1b03c56e72832ca52cdc
    public int SessionsId { get; set; }

    public int ClassId { get; set; }

<<<<<<< HEAD
=======
    [Column(TypeName = "datetime")]
>>>>>>> 094386d1a1a01a0e43ad1b03c56e72832ca52cdc
    public DateTime? SessionDate { get; set; }

    public int? UserId { get; set; }

<<<<<<< HEAD
    public virtual TblClass Class { get; set; } = null!;

    public virtual ICollection<TblAttendanceRecord> TblAttendanceRecords { get; set; } = new List<TblAttendanceRecord>();

=======
    [ForeignKey("ClassId")]
    [InverseProperty("TblAttendanceSessions")]
    public virtual TblClass Class { get; set; } = null!;

    [InverseProperty("Session")]
    public virtual ICollection<TblAttendanceRecord> TblAttendanceRecords { get; set; } = new List<TblAttendanceRecord>();

    [ForeignKey("UserId")]
    [InverseProperty("TblAttendanceSessions")]
>>>>>>> 094386d1a1a01a0e43ad1b03c56e72832ca52cdc
    public virtual TblUser? User { get; set; }
}
