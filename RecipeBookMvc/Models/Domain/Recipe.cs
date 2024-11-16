using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeBookMvc.Models.Domain
{
    public class Recipe
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Recipe title required")]
        public string? Title { get; set; }
        [Required(ErrorMessage = "Complexity required")]
        public string? Complexity { get; set; }
        public string? RecipeImage { get; set; }
        [Required(ErrorMessage ="Field with ingredients required")]

        public string? Ingredients { get; set; }
        [Required(ErrorMessage ="Instruction required")]

        public string? Instruction { get; set; }
        public string? UserId { get; set; }

        [NotMapped]
        public IFormFile ? ImageFile { get; set; }
        [NotMapped]
        [Required(ErrorMessage ="Categorys required")]

        public List<int> ? Categorys { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem>? CategoryList { get; set; }
        [NotMapped]
        public string ? CategoryNames { get; set; }

        [NotMapped]
        public MultiSelectList ? MultiCategoryList { get; set; }
    }
}
