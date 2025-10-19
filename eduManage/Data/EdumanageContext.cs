using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using eduManage.Models;

namespace eduManage.Data;

public partial class EdumanageContext : DbContext
{
    public EdumanageContext()
    {
    }

    public EdumanageContext(DbContextOptions<EdumanageContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblClass> TblClasses { get; set; }

    public virtual DbSet<TblClassMember> TblClassMembers { get; set; }

    public virtual DbSet<TblRole> TblRoles { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblClass>(entity =>
        {
            entity.HasOne(d => d.Teacher).WithMany(p => p.TblClasses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblClasses_tblUsers");
        });

        modelBuilder.Entity<TblClassMember>(entity =>
        {
            entity.HasOne(d => d.Class).WithMany(p => p.TblClassMembers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblClassMembers_tblClasses");

            entity.HasOne(d => d.User).WithMany(p => p.TblClassMembers).HasConstraintName("FK_tblClassMembers_tblUsers");
        });

        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.HasOne(d => d.Role).WithMany(p => p.TblUsers).HasConstraintName("FK_tblUsers_tblRoles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
