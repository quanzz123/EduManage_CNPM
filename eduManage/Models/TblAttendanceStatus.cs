using System;
using System.Collections.Generic;

namespace eduManage.Models;

public partial class TblAttendanceStatus
{
    public int StatusId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<TblAttendanceRecord> TblAttendanceRecords { get; set; } = new List<TblAttendanceRecord>();
}
