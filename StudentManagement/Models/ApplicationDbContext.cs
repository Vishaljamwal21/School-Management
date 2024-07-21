using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace StudentManagement.Models;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<Exam> Exams { get; set; }

    public virtual DbSet<Expense> Expenses { get; set; }

    public virtual DbSet<Fee> Fees { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentAttendance> StudentAttendances { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    public virtual DbSet<TeacherAttendance> TeacherAttendances { get; set; }

    public virtual DbSet<TeacherSubject> TeacherSubjects { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=HP\\SQLEXPRESS;Database=SchoolSysDB;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admin__719FE488FD569663");

            entity.ToTable("Admin");

            entity.Property(e => e.Dob).HasColumnName("DOB");
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Role)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Admin");
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.ClassId).HasName("PK__Class__CB1927C0224CC91C");

            entity.ToTable("Class");

            entity.Property(e => e.ClassName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Exam>(entity =>
        {
            entity.HasKey(e => e.ExamId).HasName("PK__Exam__297521C7492754EE");

            entity.ToTable("Exam");

            entity.Property(e => e.RollNo)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Class).WithMany(p => p.Exams)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("FK__Exam__ClassId__286302EC");

            entity.HasOne(d => d.Subject).WithMany(p => p.Exams)
                .HasForeignKey(d => d.SubjectId)
                .HasConstraintName("FK__Exam__SubjectId__29572725");
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.ExpenseId).HasName("PK__Expense__1445CFD36164B94E");

            entity.ToTable("Expense");

            entity.HasOne(d => d.Class).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("FK__Expense__ClassId__2C3393D0");

            entity.HasOne(d => d.Subject).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.SubjectId)
                .HasConstraintName("FK__Expense__Subject__2D27B809");
        });

        modelBuilder.Entity<Fee>(entity =>
        {
            entity.HasKey(e => e.FeesId).HasName("PK__Fees__7BD9FD27A556F26B");

            entity.HasOne(d => d.Class).WithMany(p => p.Fees)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("FK__Fees__ClassId__25869641");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Student__32C52B99FD8D168E");

            entity.ToTable("Student");

            entity.Property(e => e.Address).IsUnicode(false);
            entity.Property(e => e.Dob).HasColumnName("DOB");
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RollNo)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Class).WithMany(p => p.Students)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("FK__Student__ClassId__15502E78");
        });

        modelBuilder.Entity<StudentAttendance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__StudentA__3214EC07005DD8E6");

            entity.ToTable("StudentAttendance");

            entity.Property(e => e.RollNo)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Class).WithMany(p => p.StudentAttendances)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("FK__StudentAt__Class__21B6055D");

            entity.HasOne(d => d.Subject).WithMany(p => p.StudentAttendances)
                .HasForeignKey(d => d.SubjectId)
                .HasConstraintName("FK__StudentAt__Subje__22AA2996");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.SubjectId).HasName("PK__Subject__AC1BA3A8690FFF6B");

            entity.ToTable("Subject");

            entity.Property(e => e.SubjectName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Class).WithMany(p => p.Subjects)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("FK__Subject__ClassId__1273C1CD");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.TeacherId).HasName("PK__Teacher__EDF25964176EA920");

            entity.ToTable("Teacher");

            entity.Property(e => e.Address).IsUnicode(false);
            entity.Property(e => e.Dob).HasColumnName("DOB");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Teacher");
        });

        modelBuilder.Entity<TeacherAttendance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TeacherA__3214EC07C3C530EB");

            entity.ToTable("TeacherAttendance");

            entity.HasOne(d => d.Teacher).WithMany(p => p.TeacherAttendances)
                .HasForeignKey(d => d.TeacherId)
                .HasConstraintName("FK__TeacherAt__Teach__1ED998B2");
        });

        modelBuilder.Entity<TeacherSubject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TeacherS__3214EC07F48ADD39");

            entity.ToTable("TeacherSubject");

            entity.HasOne(d => d.Class).WithMany(p => p.TeacherSubjects)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("FK__TeacherSu__Class__1A14E395");

            entity.HasOne(d => d.Subject).WithMany(p => p.TeacherSubjects)
                .HasForeignKey(d => d.SubjectId)
                .HasConstraintName("FK__TeacherSu__Subje__1B0907CE");

            entity.HasOne(d => d.Teacher).WithMany(p => p.TeacherSubjects)
                .HasForeignKey(d => d.TeacherId)
                .HasConstraintName("FK__TeacherSu__Teach__1BFD2C07");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
