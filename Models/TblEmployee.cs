using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApplication1.Models
{
    public class TblEmployee
    {
        [Key] //To make Id as PK
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  //autogenerate
        public int Id { get; set; }
        [StringLength(50)]
        public  string Name { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(50)]
        public string? Gender { get; set; }
        public DateTime Doj {  get; set; }
        public string Email { get; set; }
        public int? Age { get; set; }

       
        [ForeignKey("Designation")]
        public int? DesignationID {  get; set; }

        // Navigation property to the related TblDesignation entity
     
        public virtual TblDesignation? Designation { get; set; }


        [NotMapped]
        public string? DesignationName => Designation?.Designation;  // This will get the Designation name from the related entity


        public string? Hobbies {  get; set; }

        //[ForeignKey("Hobby")]
        //public int? HobbyId { get; set; }

        //public virtual TblHobbies? Hobby { get; set; }
       // [NotMapped]
        //public string? HobbyName => Hobby?.HobbyName;

        //public List<int> Hobbies { get; set; }

        // [JsonIgnore]
        //  public ICollection<TblEmployeeHobby> EmployeeHobbies { get; set; } = new List<TblEmployeeHobby>();

    }
}
