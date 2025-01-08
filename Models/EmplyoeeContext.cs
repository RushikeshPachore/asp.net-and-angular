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

        public DbSet<Category> Category { get; set; }

        public DbSet<SubCategory> SubCategory { get; set; }

        public DbSet<TblQuestion> TblQuestion { get; set; }

        public DbSet<TblAnswer> TblAnswer { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<TblEmployeeHobby>()
              .HasKey(eh => eh.Id);

            // Explicitly define the relationship between Employee and TblImage
            modelBuilder.Entity<TblImage>()
            .HasOne(img => img.Employee)    // TblImage has one Employee Navigation property
            .WithMany(emp => emp.Images)    // Employee has many images
            .HasForeignKey(img => img.EmployeeId)  // Foreign key on EmployeeId in TblImage
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

            modelBuilder.Entity<TblAnswer>()
           .HasOne(a => a.Employee)
           .WithMany(e => e.Answers) //Adjust based on your relationship (e.g., .WithMany() or .WithOne())
           .HasForeignKey(a => a.EmployeeId)
           .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TblAnswer>()
                 .HasOne(a => a.Question)
                 .WithMany()
                 .HasForeignKey(a => a.QuestionId)
                 .OnDelete(DeleteBehavior.Restrict);

        
            //lets go step by step, i have answer model , in this EmpId as Fk , so it can store multiple answer of a single employee with id
            //one by one in a database in a seperate row with id and answer.so check it if its right as im not able to post in db .

            //this indicates that the TblEmployee entity has a collection
            //of TblImage instances (via the Images navigation property). 


        }

    }
}
