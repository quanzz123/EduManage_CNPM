using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace eduManage.Models;

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
            entity.Property(e => e.ClassId).HasColumnName("ClassID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Deadline).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(250);
            entity.Property(e => e.ModifyDate).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(250);

            entity.HasOne(d => d.Class).WithMany(p => p.Assignments)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Assignments_tblClasses");
        });

        modelBuilder.Entity<Submission>(entity =>
        {
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.SubmitDate).HasColumnType("datetime");

            entity.HasOne(d => d.Assignment).WithMany(p => p.Submissions)
                .HasForeignKey(d => d.AssignmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Submissions_Assignments");

            entity.HasOne(d => d.AssignmentNavigation).WithMany(p => p.Submissions)
                .HasForeignKey(d => d.AssignmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Submissions_tblUsers");
        });

        modelBuilder.Entity<TblAttendanceRecord>(entity =>
        {
            entity.HasKey(e => e.RecordId);

            entity.ToTable("tblAttendanceRecords");

            entity.Property(e => e.Note).HasMaxLength(250);
            entity.Property(e => e.RecordedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Session).WithMany(p => p.TblAttendanceRecords)
                .HasForeignKey(d => d.SessionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblAttendanceRecords_tblAttendanceSessions");

            entity.HasOne(d => d.Status).WithMany(p => p.TblAttendanceRecords)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblAttendanceRecords_tblAttendanceStatus");
        });

        modelBuilder.Entity<TblAttendanceSession>(entity =>
        {
            entity.HasKey(e => e.SessionsId);

            entity.ToTable("tblAttendanceSessions");

            entity.Property(e => e.SessionDate).HasColumnType("datetime");

            entity.HasOne(d => d.Class).WithMany(p => p.TblAttendanceSessions)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblAttendanceSessions_tblClasses");
        });

        modelBuilder.Entity<TblAttendanceStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId);

            entity.ToTable("tblAttendanceStatus");

            entity.Property(e => e.Description).HasMaxLength(250);
            entity.Property(e => e.Title).HasMaxLength(250);
        });

        modelBuilder.Entity<TblClass>(entity =>
        {
            entity.HasKey(e => e.ClassId);

            entity.ToTable("tblClasses");

            entity.Property(e => e.ClassId).HasColumnName("ClassID");
            entity.Property(e => e.ClassName).HasMaxLength(250);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.Image).HasMaxLength(250);
            entity.Property(e => e.ModifedDate).HasColumnType("datetime");
            entity.Property(e => e.Schedule).HasMaxLength(50);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Subject).HasMaxLength(250);
            entity.Property(e => e.TeacherId).HasColumnName("TeacherID");

            entity.HasOne(d => d.Teacher).WithMany(p => p.TblClasses)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblClasses_tblUsers");
        });

        modelBuilder.Entity<TblClassMember>(entity =>
        {
            entity.HasKey(e => e.MemberId);

            entity.ToTable("tblClassMembers");

            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.ClassId).HasColumnName("ClassID");
            entity.Property(e => e.JoinDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Class).WithMany(p => p.TblClassMembers)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblClassMembers_tblClasses");

            entity.HasOne(d => d.User).WithMany(p => p.TblClassMembers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_tblClassMembers_tblUsers");
        });

        modelBuilder.Entity<TblLearningProgress>(entity =>
        {
            entity.HasKey(e => e.ProgressId);

            entity.ToTable("tblLearningProgress");

            entity.Property(e => e.CompletionRate).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.LastAccessTime).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.LastContent).WithMany(p => p.TblLearningProgresses)
                .HasForeignKey(d => d.LastContentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblLearningProgress_tblLessionContent");

            entity.HasOne(d => d.Lesson).WithMany(p => p.TblLearningProgresses)
                .HasForeignKey(d => d.LessonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblLearningProgress_tblLessons");

            entity.HasOne(d => d.User).WithMany(p => p.TblLearningProgresses)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblLearningProgress_tblUsers");
        });

        modelBuilder.Entity<TblLessionContent>(entity =>
        {
            entity.HasKey(e => e.ContentId);

            entity.ToTable("tblLessionContent");

            entity.Property(e => e.ContentType).HasMaxLength(50);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(250);

            entity.HasOne(d => d.Lession).WithMany(p => p.TblLessionContents)
                .HasForeignKey(d => d.LessionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblLessionContent_tblLessons");
        });

        modelBuilder.Entity<TblLesson>(entity =>
        {
            entity.HasKey(e => e.LessonId);

            entity.ToTable("tblLessons");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(250);
            entity.Property(e => e.Title).HasMaxLength(250);

            entity.HasOne(d => d.Class).WithMany(p => p.TblLessons)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblLessons_tblClasses");
        });

        modelBuilder.Entity<TblRole>(entity =>
        {
            entity.HasKey(e => e.RoleId);

            entity.ToTable("tblRoles");

            entity.Property(e => e.RoleDescription).HasMaxLength(250);
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("tblUsers");

            entity.Property(e => e.Address).HasMaxLength(150);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.PassworkHash).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.UserName).HasMaxLength(50);

            entity.HasOne(d => d.Role).WithMany(p => p.TblUsers)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_tblUsers_tblRoles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
