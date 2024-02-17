using Microsoft.EntityFrameworkCore;
using Graduation_Project.Shared.DTO;
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


            //modelBuilder.Entity<Gender>(entity =>
            //{
            //    entity.Property(e => e.Created).HasDefaultValueSql("(getdate())");

            //    entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.GenderCreatedByNavigations).HasConstraintName("FK_Gender_Users");

            //    entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.GenderUpdatedByNavigations).HasConstraintName("FK_Gender_Users1");
            //});

          

            //modelBuilder.Entity<Profile>(entity =>
            //{
            //    entity.Property(e => e.UserId).ValueGeneratedNever();

            //    entity.HasOne(d => d.Insurance).WithMany(p => p.Profiles)
            //  .OnDelete(DeleteBehavior.ClientSetNull)
            //  .HasConstraintName("FK_Profile_InsuranceCompany");

            //    entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProfileCreatedByNavigations)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_Profile_Users1");

            //    entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ProfileUpdatedByNavigations)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_Profile_Users");
            //});

            //modelBuilder.Entity<User>(b =>
            //{
            //    // Each User can have many entries in the UserRole join table  
            //    b.HasMany(e => e.UserRoles)
            //        .WithOne(e => e.User)
            //        .HasForeignKey(ur => ur.UserId)
            //        .IsRequired();
            //});

            //modelBuilder.Entity<M.Role>(b =>
            //{
            //    // Each Role can have many entries in the UserRole join table  
            //    b.HasMany(e => e.UserRoles)
            //        .WithOne(e => e.Role)
            //        .HasForeignKey(ur => ur.RoleId)
            //        .IsRequired();
            //});

            //modelBuilder.Entity<M.Role>(entity =>
            //{
            //    entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
            //        .IsUnique()
            //        .HasFilter("([NormalizedName] IS NOT NULL)");

            //    entity.Property(e => e.Id).ValueGeneratedNever();
            //});

            //modelBuilder.Entity<User>(entity =>
            //{
            //    entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
            //        .IsUnique()
            //        .HasFilter("([NormalizedUserName] IS NOT NULL)");

            //    entity.Property(e => e.Id).ValueGeneratedNever();

            //    entity.HasMany(d => d.Roles).WithMany(p => p.Users)
            //        .UsingEntity<Dictionary<string, object>>(
            //            "UserRole",
            //            r => r.HasOne<M.Role>().WithMany().HasForeignKey("RoleId"),
            //            l => l.HasOne<User>().WithMany().HasForeignKey("UserId"),
            //            j =>
            //            {
            //                j.HasKey("UserId", "RoleId");
            //                j.ToTable("UserRoles");
            //                j.HasIndex(new[] { "RoleId" }, "IX_UserRoles_RoleId");
            //            });
            //});

           
        }


        public DbSet<User> Users { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
        public DbSet<M.Role> Roles { get; set; }
        public DbSet<RoleClaim> RoleClaims { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }


      
        public virtual DbSet<Content> Contents { get; set; }
       
        public virtual DbSet<M.File> Files { get; set; }
        //public virtual DbSet<Gender> Genders { get; set; }
     
        //public virtual DbSet<Profile> Profiles { get; set; }
      
        partial void OnModelCreatingPartialGlobalFilters(ModelBuilder modelBuilder);
        partial void OnModelCreatingPartialDataSeed(ModelBuilder modelBuilder);
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
