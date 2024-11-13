using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class EmplyoeeContext:DbContext
    {
        public EmplyoeeContext(DbContextOptions<EmplyoeeContext> options) : base(options) { }

       // public DbSet<TblEmployeeHobby> TblEmployeeHobby { get; set; }
        public DbSet<TblEmployee> TblEmployee { get; set; }

        public DbSet<TblHobbies> TblHobbies { get; set; }


        public DbSet<TblDesignation> TblDesignation { get; set; }

        



        //Addeed for Composite Pk relation for many to many
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    // Define the many-to-many relationship between Employee and Hobby
        //    modelBuilder.Entity<TblEmployeeHobby>()
        //        .HasKey(eh => new { eh.EmployeeId, eh.HobbyId });  // Composite primary key

        //    modelBuilder.Entity<TblEmployeeHobby>()
        //        .HasOne(eh => eh.Employee)  // Many-to-one with Employee
        //        .WithMany(e => e.EmployeeHobbies)  // One-to-many from Employee side
        //        .HasForeignKey(eh => eh.EmployeeId);

        //    modelBuilder.Entity<TblEmployeeHobby>()
        //        .HasOne(eh => eh.Hobby)  // Many-to-one with Hobby
        //        .WithMany()  // No need for a navigation property on the Hobby side
        //        .HasForeignKey(eh => eh.HobbyId);
        //}


    }
}
