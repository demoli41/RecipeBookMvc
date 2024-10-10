using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeBookMvc.Models.Domain
{
    public class Recipe
    {
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }

        public string? Complexity { get; set; }

        public string? RecipeImage { get; set; }
        [Required]

        public string? Ingredients { get; set; }
        [Required]

        public string? Instruction { get; set; }

        [NotMapped]
        public IFormFile ? ImageFile { get; set; }
        [NotMapped]
        [Required]

        public List<int> ? Categorys { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem>? CategoryList { get; set; }
        [NotMapped]
        public string ? CategoryNames { get; set; }

        [NotMapped]
        public MultiSelectList ? MultiCategoryList { get; set; }
    }
}
