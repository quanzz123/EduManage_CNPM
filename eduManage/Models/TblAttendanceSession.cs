using System;
using System.Collections.Generic;

namespace eduManage.Models;

public partial class TblAttendanceSession
{
    public int SessionsId { get; set; }

    public int ClassId { get; set; }

    public DateTime? SessionDate { get; set; }

    public int? UserId { get; set; }

    public virtual TblClass Class { get; set; } = null!;

    public virtual ICollection<TblAttendanceRecord> TblAttendanceRecords { get; set; } = new List<TblAttendanceRecord>();
}
