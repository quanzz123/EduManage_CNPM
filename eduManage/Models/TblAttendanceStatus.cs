using System;
using System.Collections.Generic;
<<<<<<< HEAD

namespace eduManage.Models;

public partial class TblAttendanceStatus
{
    public int StatusId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

=======
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eduManage.Models;

[Table("tblAttendanceStatus")]
public partial class TblAttendanceStatus
{
    [Key]
    public int StatusId { get; set; }

    [StringLength(250)]
    public string? Title { get; set; }

    [StringLength(250)]
    public string? Description { get; set; }

    [InverseProperty("Status")]
>>>>>>> 094386d1a1a01a0e43ad1b03c56e72832ca52cdc
    public virtual ICollection<TblAttendanceRecord> TblAttendanceRecords { get; set; } = new List<TblAttendanceRecord>();
}
