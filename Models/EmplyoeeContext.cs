using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class EmplyoeeContext : DbContext
    {
        public EmplyoeeContext(DbContextOptions<EmplyoeeContext> options) : base(options) { }

        public DbSet<TblEmployee> TblEmployee { get; set; }

        public DbSet<TblHobbies> TblHobbies { get; set; }




        public DbSet<TblImage> TblImage { get; set; }

        public DbSet<TblDesignation> TblDesignation { get; set; }

        public DbSet<TblEmployeeHobby> TblEmployeeHobbies { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<TblEmployeeHobby>()
                 .HasKey(eh => eh.Id);

            // Explicitly define the relationship between Employee and TblImage
            modelBuilder.Entity<TblImage>()
                .HasOne(img => img.Employee)    // TblImage has one Employee Navigation property
                .WithMany(emp => emp.Images)    // Employee has many images
                .HasForeignKey(img => img.EmployeeId)  // Foreign key on EmployeeId
                .OnDelete(DeleteBehavior.Cascade);    // Optional: Set the delete behavior (cascade or restrict)


            modelBuilder.Entity<TblEmployeeHobby>()
                 .HasOne(eh => eh.Employees) //navigation in tblemphobby
                 .WithMany(e => e.EmployeeHobbies) // Navigation property in TblEmployee
                 .HasForeignKey(eh => eh.EmpId)
                 .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<TblEmployeeHobby>()
                .HasOne(eh => eh.Hobby)
                .WithMany() // No navigation property in TblHobbies
                .HasForeignKey(eh => eh.HobId)
                .OnDelete(DeleteBehavior.Cascade);
        }


  
        //this indicates that the TblEmployee entity has a collection
        //of TblImage instances (via the Images navigation property). 




    }
}
