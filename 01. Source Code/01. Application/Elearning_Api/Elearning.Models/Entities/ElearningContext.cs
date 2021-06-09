using System;
using Elearning.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Elearning.Models.Entities
{
    public partial class ElearningContext : DbContext
    {
        public ElearningContext()
        {
        }

        public ElearningContext(DbContextOptions<ElearningContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AnswerLearner> AnswerLearner { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<GroupFunction> GroupFunction { get; set; }
        public virtual DbSet<GroupPermission> GroupPermission { get; set; }
        public virtual DbSet<GroupUser> GroupUser { get; set; }
        public virtual DbSet<Lesson> Lesson { get; set; }
        public virtual DbSet<LessonCourse> LessonCourse { get; set; }
        public virtual DbSet<LessonQuestion> LessonQuestion { get; set; }
        public virtual DbSet<Permission> Permission { get; set; }
        public virtual DbSet<Question> Question { get; set; }
        public virtual DbSet<Topic> Topic { get; set; }
        public virtual DbSet<Test> Test { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserPermission> UserPermission { get; set; }
        public virtual DbSet<Program> Program { get; set; }
        public virtual DbSet<Course> Course { get; set; }
        public virtual DbSet<Learner> Learner { get; set; }
        public virtual DbSet<LearnerCourse> LearnerCourse { get; set; }
        public virtual DbSet<EmployeeCourse> EmployeeCourse { get; set; }
        public virtual DbSet<Answer> Answer { get; set; }
        public virtual DbSet<HomeSlider> HomeSlider { get; set; }
        public virtual DbSet<EmployeeSpecialist> EmployeeSpecialist { get; set; }
        public virtual DbSet<HomeSpecialist> HomeSpecialist { get; set; }
        public virtual DbSet<HomeSetting> HomeSetting { get; set; }
        public virtual DbSet<HomeService> HomeService { get; set; }
        public virtual DbSet<LessonHistory> LessonHistory { get; set; }
        public virtual DbSet<Mark> Mark { get; set; }
        public virtual DbSet<Province> Province { get; set; }
        public virtual DbSet<District> District { get; set; }
        public virtual DbSet<Ward> Ward { get; set; }
        public virtual DbSet<Nation> Nation { get; set; }
        public virtual DbSet<FileTemplate> FileTemplate { get; set; }
        public virtual DbSet<UserHistories> UserHistories { get; set; }
        public virtual DbSet<UserDevices> UserDevice { get; set; }
        public virtual DbSet<ManagerUnit> ManagerUnit { get; set; }
        public virtual DbSet<HomeLink> HomeLink { get; set; }
        public virtual DbSet<ApprovalHistory> ApprovalHistory { get; set; }
        public virtual DbSet<SystemParam> SystemParams { get; set; }
        public virtual DbSet<SystemParamGroup> SystemParamGroups { get; set; }
        public virtual DbSet<LessonApprovalHistory> LessonApprovalHistory { get; set; }
        public virtual DbSet<QuestionApprovalHistory> QuestionApprovalHistory { get; set; }
        public virtual DbSet<LessonFrame> LessonFrame { get; set; }
        public virtual DbSet<LessonFrameHistory> LessonFrameHistory { get; set; }
        public virtual DbSet<LessonAnswerLearner> LessonAnswerLearner { get; set; }
        public virtual DbSet<LessonFrameQuestion> LessonFrameQuestion { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=14.248.84.128,1445;Database=Elearning;User Id=sa;Password=Admin@123#456;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.Address).HasMaxLength(500);

                entity.Property(e => e.Birthday).HasColumnType("datetime");

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.PhoneNumber).HasMaxLength(50);

                entity.Property(e => e.UpdateBy)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<GroupFunction>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(300);
            });

            modelBuilder.Entity<GroupPermission>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.GroupUserId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.PermissionId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.HasOne(d => d.GroupUser)
                    .WithMany(p => p.GroupPermission)
                    .HasForeignKey(d => d.GroupUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GroupPermission_GroupUser");

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.GroupPermission)
                    .HasForeignKey(d => d.PermissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GroupPermission_Permission");
            });

            modelBuilder.Entity<GroupUser>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.UpdateBy)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.GroupFunctionId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.ScreenCode).HasMaxLength(50);

                entity.HasOne(d => d.GroupFunction)
                    .WithMany(p => p.Permission)
                    .HasForeignKey(d => d.GroupFunctionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Permission_GroupFunction");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.ObjectId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.SecurityStamp)
                    .IsRequired()
                    .HasMaxLength(32);

                entity.Property(e => e.UpdateBy)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<UserPermission>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.PermissionId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.UserPermission)
                    .HasForeignKey(d => d.PermissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserPermission_Permission");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserPermission)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserPermission_User");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
