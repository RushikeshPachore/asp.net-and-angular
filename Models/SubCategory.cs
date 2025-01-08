using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApplication1.Models
{
    public class SubCategory
    {


        public int Id { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public string SubCategories { get; set; }


        public virtual Category Category { get; set; }                                                                                                                          
    }
}

