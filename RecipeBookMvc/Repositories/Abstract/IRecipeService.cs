using RecipeBookMvc.Models.Domain;
using RecipeBookMvc.Models.DTO;

namespace RecipeBookMvc.Repositories.Abstract
{
    public interface IRecipeService
    {
        bool Add(Recipe model);
        bool Update(Recipe model);
        Recipe GetById(int id);
        bool Delete(int id);

        RecipeListVM List(string term = "", int? categoryId = null, bool paging = false, int currentPage = 0);

        List<int> GetCategoryByRecipeId(int recipeId);
    }
}