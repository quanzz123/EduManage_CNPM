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

    [InverseProperty("User")]
    public virtual ICollection<TblClassMember> TblClassMembers { get; set; } = new List<TblClassMember>();

    [InverseProperty("Teacher")]
    public virtual ICollection<TblClass> TblClasses { get; set; } = new List<TblClass>();
}
