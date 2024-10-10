using System.ComponentModel.DataAnnotations;

namespace RecipeBookMvc.Models.Domain
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string? CategoryName { get; set; }
    }
}
