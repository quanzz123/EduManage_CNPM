using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eduManage.Models;

[Table("tblClasses")]
public partial class TblClass
{
    [Key]
    [Column("ClassID")]
    public int ClassId { get; set; }

    [StringLength(250)]
    public string ClassName { get; set; } = null!;

    public string? Description { get; set; }

    [StringLength(250)]
    public string? Subject { get; set; }

    [Column("TeacherID")]
    public int TeacherId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? StartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EndDate { get; set; }

    [StringLength(50)]
    public string? Schedule { get; set; }

    public bool? IsActive { get; set; }

    [StringLength(250)]
    public string? Image { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifedDate { get; set; }

    public int? MaxStudents { get; set; }

    [InverseProperty("Class")]
    public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();

    [InverseProperty("Class")]
    public virtual ICollection<TblAttendanceSession> TblAttendanceSessions { get; set; } = new List<TblAttendanceSession>();

    [InverseProperty("Class")]
    public virtual ICollection<TblClassMember> TblClassMembers { get; set; } = new List<TblClassMember>();

    [InverseProperty("Class")]
    public virtual ICollection<TblLesson> TblLessons { get; set; } = new List<TblLesson>();

    public virtual ICollection<TblQuiz> TblQuizzes { get; set; } = new List<TblQuiz>();

    public virtual TblUser Teacher { get; set; } = null!;
}
