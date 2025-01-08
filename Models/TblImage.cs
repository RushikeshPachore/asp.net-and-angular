using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApplication1.Models
{
    public class TblImage
    {
        public int Id { get; set; }

        [ForeignKey("Employee")] //this Employee should match the navigation property variable down there
        public int EmployeeId { get; set; }

        public string? MultiImage { get; set; }


        public virtual TblEmployee Employee { get; set; } //navigate the employee table through this naigation property
    }
}


// The string "Employee" in the [ForeignKey] attribute should exactly match the name of the navigation property
// in the target entity (in this case, the TblEmployee entity).