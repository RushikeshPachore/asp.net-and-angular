using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class TblEmployeeHobby
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id;


    
        public int EmpId { get; set; }

      
        public int HobId {  get; set; }


        public virtual TblEmployee Employees { get; set; }

        public virtual TblHobbies Hobby {  get; set; }

    }
}
