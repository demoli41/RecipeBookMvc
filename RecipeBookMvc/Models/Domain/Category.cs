using System.ComponentModel.DataAnnotations;

namespace RecipeBookMvc.Models.Domain
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Необхідна назва категорії")]
        public string? CategoryName { get; set; }
    }
}