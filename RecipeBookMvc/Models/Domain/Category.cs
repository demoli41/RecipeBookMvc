using System.ComponentModel.DataAnnotations;

namespace RecipeBookMvc.Models.Domain
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Category name required")]
        public string? CategoryName { get; set; }
    }
}