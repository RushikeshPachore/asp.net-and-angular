using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApplication1.Models
{
    public class TblAnswer
    {

        public int Id { get; set; }

        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }

        public string? Answer { get; set; }

        [ForeignKey("Question")]
        public int QuestionId { get; set; }
    
        public virtual TblEmployee? Employee { get; set; }

        public virtual TblQuestion? Question { get; set; }

    }
}



