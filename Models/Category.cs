using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApplication1.Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Categories { get; set; }


        public virtual ICollection<SubCategory> SubCategories { get; set; }
        // added this collection navigation Because we are using Include , to get both category and subcategory simultaneously at the same time in frontend with
        //one get method only.


    }
}

