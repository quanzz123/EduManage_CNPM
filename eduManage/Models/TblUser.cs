using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eduManage.Models;

[Table("tblUsers")]
public partial class TblUser
{
    [Key]
    public int UserId { get; set; }

    public int? RoleId { get; set; }

    [StringLength(50)]
    public string UserName { get; set; } = null!;

    [StringLength(255)]
    public string PassworkHash { get; set; } = null!;

    [StringLength(100)]
    public string? FullName { get; set; }

    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(20)]
    public string? Phone { get; set; }

    [StringLength(150)]
    public string? Address { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateDate { get; set; }

    public bool? IsActive { get; set; }

    [ForeignKey("RoleId")]
    [InverseProperty("TblUsers")]
    public virtual TblRole? Role { get; set; }

    [InverseProperty("AssignmentNavigation")]
    public virtual ICollection<Submission> Submissions { get; set; } = new List<Submission>();

<<<<<<< HEAD
    public virtual ICollection<TblAttendanceRecord> TblAttendanceRecords { get; set; } = new List<TblAttendanceRecord>();

    public virtual ICollection<TblAttendanceSession> TblAttendanceSessions { get; set; } = new List<TblAttendanceSession>();

=======
    [InverseProperty("User")]
    public virtual ICollection<TblAttendanceRecord> TblAttendanceRecords { get; set; } = new List<TblAttendanceRecord>();

    [InverseProperty("User")]
    public virtual ICollection<TblAttendanceSession> TblAttendanceSessions { get; set; } = new List<TblAttendanceSession>();

    [InverseProperty("User")]
>>>>>>> 094386d1a1a01a0e43ad1b03c56e72832ca52cdc
    public virtual ICollection<TblClassMember> TblClassMembers { get; set; } = new List<TblClassMember>();

    [InverseProperty("Teacher")]
    public virtual ICollection<TblClass> TblClasses { get; set; } = new List<TblClass>();

    [InverseProperty("User")]
    public virtual ICollection<TblLearningProgress> TblLearningProgresses { get; set; } = new List<TblLearningProgress>();
}
