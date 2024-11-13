using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class TblHobbies
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HobbyId { get; set; }
       
        //[Required]
        [MaxLength(100)]
        public string HobbyName { get; set; }

        //public List<TblEmployeeHobby> EmployeeHobbies { get; set; }
    }
}
