using System;
using System.Collections.Generic;

namespace eduManage.Models;

public partial class TblAttendanceRecord
{
    public int RecordId { get; set; }

    public int SessionId { get; set; }

    public int UserId { get; set; }

    public int StatusId { get; set; }

    public string? Note { get; set; }

    public DateTime? RecordedAt { get; set; }

    public virtual TblAttendanceSession Session { get; set; } = null!;

    public virtual TblAttendanceStatus Status { get; set; } = null!;

    public virtual TblUser User { get; set; } = null!;
}
