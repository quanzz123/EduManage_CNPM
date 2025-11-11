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

    public virtual DbSet<Assignment> Assignments { get; set; }

    public virtual DbSet<Submission> Submissions { get; set; }

    public virtual DbSet<TblAttendanceRecord> TblAttendanceRecords { get; set; }

    public virtual DbSet<TblAttendanceSession> TblAttendanceSessions { get; set; }

    public virtual DbSet<TblAttendanceStatus> TblAttendanceStatuses { get; set; }

    public virtual DbSet<TblClass> TblClasses { get; set; }

    public virtual DbSet<TblClassMember> TblClassMembers { get; set; }

    public virtual DbSet<TblLearningProgress> TblLearningProgresses { get; set; }

    public virtual DbSet<TblLessionContent> TblLessionContents { get; set; }

    public virtual DbSet<TblLesson> TblLessons { get; set; }

    public virtual DbSet<TblRole> TblRoles { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Assignment>(entity =>
        {
            entity.HasOne(d => d.Class).WithMany(p => p.Assignments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Assignments_tblClasses");
        });

        modelBuilder.Entity<Submission>(entity =>
        {
            entity.HasOne(d => d.Assignment).WithMany(p => p.Submissions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Submissions_Assignments");

            entity.HasOne(d => d.AssignmentNavigation).WithMany(p => p.Submissions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Submissions_tblUsers");
        });

        modelBuilder.Entity<TblAttendanceRecord>(entity =>
        {
            entity.HasOne(d => d.Session).WithMany(p => p.TblAttendanceRecords)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblAttendanceRecords_tblAttendanceSessions");

            entity.HasOne(d => d.Status).WithMany(p => p.TblAttendanceRecords)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblAttendanceRecords_tblAttendanceStatus");

            entity.HasOne(d => d.User).WithMany(p => p.TblAttendanceRecords)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblAttendanceRecords_tblUsers");
        });

        modelBuilder.Entity<TblAttendanceSession>(entity =>
        {
            entity.HasOne(d => d.Class).WithMany(p => p.TblAttendanceSessions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblAttendanceSessions_tblClasses");

            entity.HasOne(d => d.User).WithMany(p => p.TblAttendanceSessions).HasConstraintName("FK_tblAttendanceSessions_tblUsers");
        });

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

        modelBuilder.Entity<TblLearningProgress>(entity =>
        {
            entity.HasOne(d => d.LastContent).WithMany(p => p.TblLearningProgresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblLearningProgress_tblLessionContent");

            entity.HasOne(d => d.Lesson).WithMany(p => p.TblLearningProgresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblLearningProgress_tblLessons");

            entity.HasOne(d => d.User).WithMany(p => p.TblLearningProgresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblLearningProgress_tblUsers");
        });

        modelBuilder.Entity<TblLessionContent>(entity =>
        {
            entity.HasOne(d => d.Lession).WithMany(p => p.TblLessionContents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblLessionContent_tblLessons");
        });

        modelBuilder.Entity<TblLesson>(entity =>
        {
            entity.HasOne(d => d.Class).WithMany(p => p.TblLessons)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblLessons_tblClasses");
        });

        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.HasOne(d => d.Role).WithMany(p => p.TblUsers).HasConstraintName("FK_tblUsers_tblRoles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
