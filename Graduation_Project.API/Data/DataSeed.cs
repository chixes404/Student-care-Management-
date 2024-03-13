using Graduation_Project.Shared.Models;
using M =Graduation_Project.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.API.Data
{
    public partial class ApplicationDatabaseContext
    {
        partial void OnModelCreatingPartialDataSeed(ModelBuilder modelBuilder)
        {
            // password : P@ssw0rd
            modelBuilder.Entity<User>().HasData(
               new { Id = new Guid("5f8fe2ac-e39f-4905-bb45-7dbbf6048556"), UserName = "admin@system.com", FirstName = "Super", LastName = "Administrator", IsActive = true, NormalizedUserName = "ADMIN@SYSTEM.COM", Email = "admin@system.com", NormalizedEmail = "ADMIN@SYSTEM.COM", EmailConfirmed = true, PasswordHash = "AQAAAAEAACcQAAAAEMYgoo+Nk8o2o7R8s6Wa5TxKAEO1ytTcZhrL1YQHHWVY3DEeBqGd9PLAy91c1pVAig==", SecurityStamp = "VENUWRKWC2VPS67PRJBL26YK52EMR2L3", ConcurrencyStamp = "3fd1dc9a-82ab-4a62-be70-e80e201573af",Address="tanta", PhoneNumberConfirmed = false, TwoFactorEnabled = false, LockoutEnabled = true, AccessFailedCount = 0 }
            );

            modelBuilder.Entity<M.Role>().HasData(
                new { Id = new Guid("bdebdb78-4dc9-40ac-b319-f4e5e7bc3503"), Name = "Administrator", NormalizedName = "ADMINISTRATOR" },
                new { Id = new Guid("b4018cb5-755e-468b-9802-a9917c37730e"), Name = "Teacher", NormalizedName = "TEACHER" },
                new { Id = new Guid("b1c940d9-1014-48f9-8cd8-a410eb35bc1f"), Name = "Employee", NormalizedName = "EMPLOYEE" },
                new { Id = new Guid("ee30e20d-5851-4f96-bc13-6aa7c73ce07c"), Name = "User", NormalizedName = "USER" }



            );

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(
               new { UserId = new Guid("5f8fe2ac-e39f-4905-bb45-7dbbf6048556"), RoleId = new Guid("bdebdb78-4dc9-40ac-b319-f4e5e7bc3503") }
            );


            //modelBuilder.Entity<Gender>().HasData(
            //   new Gender { Id = 1, Gender1 = "Male" },
            //   new Gender { Id = 2, Gender1 = "Female" }
            //);
        }
    }

}
