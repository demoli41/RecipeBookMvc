using RecipeBookMvc.Models.Domain;
using RecipeBookMvc.Models.DTO;

namespace RecipeBookMvc.Repositories.Abstract
{
    public interface ICategoryService
    {
        bool Add(Category model);
        bool Update(Category model);
        Category GetById(int id);
        bool Delete(int id);
        IQueryable<Category> List();
    }
}
