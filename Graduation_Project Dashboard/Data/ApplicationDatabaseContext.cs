using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Microsoft.AspNetCore.Identity;
using Reflections.Framework.RoleBasedSecurity;
using System.Reflection.Emit;
using Graduation_Project.Shared.Models;
using M = Graduation_Project.Shared.Models;

namespace Graduation_Project_Dashboard.Data
{
    public partial class ApplicationDatabaseContext : IdentityDbContext<M.User, M.Role, Guid>
    {
        public ApplicationDatabaseContext(DbContextOptions<ApplicationDatabaseContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            OnModelCreatingPartialDataSeed(modelBuilder);
            modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");





            modelBuilder.Entity<Content>(entity =>
            {
                entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ContentCreatedByNavigations)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Content_Users");

                entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ContentUpdatedByNavigations)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Content_Users1");
            });


            modelBuilder.Entity<TeacherSubject>()
               .HasKey(ts => new { ts.TeacherId, ts.SubjectId });

            modelBuilder.Entity<TeacherSubject>()
                .HasOne(ts => ts.Teacher)
                .WithMany(t => t.TeacherSubjects)
                .HasForeignKey(ts => ts.TeacherId);

            modelBuilder.Entity<TeacherSubject>()
                .HasOne(ts => ts.Subject)
                .WithMany(s => s.TeacherSubjects)
                .HasForeignKey(ts => ts.SubjectId);

            modelBuilder.Entity<TeacherGrade>()
                .HasKey(tg => new { tg.TeacherId, tg.GradeId });

            modelBuilder.Entity<TeacherGrade>()
                .HasOne(tg => tg.Teacher)
                .WithMany(t => t.TeacherGrades)
                .HasForeignKey(tg => tg.TeacherId);

            modelBuilder.Entity<TeacherGrade>()
                .HasOne(tg => tg.Grade)
                .WithMany(g => g.TeacherGrades)
                .HasForeignKey(tg => tg.GradeId);



            modelBuilder.Entity<TeacherClass>()
         .HasKey(tc => new { tc.TeacherId, tc.ClassId });

            modelBuilder.Entity<TeacherClass>()
                .HasOne(tc => tc.Teacher)
                .WithMany(t => t.TeacherClasses)
                .HasForeignKey(tc => tc.TeacherId);

            modelBuilder.Entity<TeacherClass>()
                .HasOne(tc => tc.Class)
                .WithMany(c => c.TeacherClasses)
                .HasForeignKey(tc => tc.ClassId);

            modelBuilder.Entity<CanteenTransactionProduct>()
         .HasKey(ctp => new { ctp.CanteenTransactionId, ctp.ProductId });
        }


        public DbSet<User> Users { get; set; }
        
        public DbSet<M.Role> Roles { get; set; }
        

      
        public virtual DbSet<Content> Contents { get; set; }
       
        public virtual DbSet<M.File> Files { get; set; }
        public virtual DbSet<Parent> Parents { get; set; }
        public virtual DbSet<Student> Students { get; set; }

        public virtual DbSet<School> Schools { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }
        public virtual DbSet<Wallet> Wallets { get; set; }

        public virtual DbSet<CanteenTransaction> CanteenTransactions { get; set; }

        public virtual DbSet<ParentTransaction> ParentTransactions { get; set; }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<BlockedProduct> BlockedProducts { get; set; }
        public virtual DbSet<Grade> Grades { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<TeacherGrade> TeacherGrades { get; set; }
        public virtual DbSet<TeacherSubject> TeacherSubjects { get; set; }
        public virtual DbSet<TeacherClass> TeacherClasses { get; set; }
        public virtual DbSet<Chat> Chats { get; set; }
        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<CanteenTransactionProduct> CanteenTransactionProducts { get; set; }






        partial void OnModelCreatingPartialGlobalFilters(ModelBuilder modelBuilder);
        partial void OnModelCreatingPartialDataSeed(ModelBuilder modelBuilder);
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
