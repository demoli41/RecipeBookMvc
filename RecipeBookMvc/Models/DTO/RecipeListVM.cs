using RecipeBookMvc.Models.Domain;

namespace RecipeBookMvc.Models.DTO
{
    public class RecipeListVM
    {
        public IQueryable<Recipe> RecipeList { get; set; }
        public string Term { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; } // Додано розмір сторінки
        public int? SelectedCategoryId { get; set; } // Обрана категорія
        public string SortOrder { get; set; } // Порядок сортування
    }

}
