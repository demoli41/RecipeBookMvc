using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeBookMvc.Models.Domain
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        public int RecipeId { get; set; }

        [Required(ErrorMessage = "Review text is required")]
        public string Text { get; set; }

        public string UserId { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [ForeignKey("RecipeId")]
        public Recipe Recipe { get; set; }
    }
}
