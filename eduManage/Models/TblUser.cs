using System;
using System.Collections.Generic;

namespace eduManage.Models;

public partial class TblUser
{
    public int UserId { get; set; }

    public int? RoleId { get; set; }

    public string UserName { get; set; } = null!;

    public string PassworkHash { get; set; } = null!;

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public DateTime? CreateDate { get; set; }

    public bool? IsActive { get; set; }

    public virtual TblRole? Role { get; set; }

    public virtual ICollection<Submission> Submissions { get; set; } = new List<Submission>();

    public virtual ICollection<TblAttendanceRecord> TblAttendanceRecords { get; set; } = new List<TblAttendanceRecord>();

    public virtual ICollection<TblAttendanceSession> TblAttendanceSessions { get; set; } = new List<TblAttendanceSession>();

    public virtual ICollection<TblClassMember> TblClassMembers { get; set; } = new List<TblClassMember>();

    public virtual ICollection<TblClass> TblClasses { get; set; } = new List<TblClass>();

    public virtual ICollection<TblLearningProgress> TblLearningProgresses { get; set; } = new List<TblLearningProgress>();

    public virtual ICollection<TblQuizAttempt> TblQuizAttempts { get; set; } = new List<TblQuizAttempt>();

    public virtual ICollection<TblStudentAnswer> TblStudentAnswers { get; set; } = new List<TblStudentAnswer>();
}
