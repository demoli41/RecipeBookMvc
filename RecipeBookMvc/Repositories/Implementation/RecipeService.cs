using RecipeBookMvc.Repositories.Abstract;
using RecipeBookMvc.Models.Domain;
using RecipeBookMvc.Models.DTO;

namespace RecipeBookMvc.Repositories.Implementation
{
    public class RecipeService : IRecipeService
    {
        private readonly DatabaseContext ctx;
        public RecipeService(DatabaseContext ctx)
        {
            this.ctx = ctx;
        }
        public bool Add(Recipe model)
        {
            try
            {
                ctx.Recipe.Add(model);
                ctx.SaveChanges();
                foreach (int categoryId in model.Categorys)
                {
                    var RecipeCategory = new RecipeCategory
                    {
                        RecipeId = model.Id,
                        CategoryId = categoryId
                    };
                    ctx.RecipeCategory.Add(RecipeCategory);
                }
                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var data = this.GetById(id);
                if (data == null)
                    return false;
                var recipeCategorys = ctx.RecipeCategory.Where(a => a.RecipeId == data.Id);
                foreach (var recipeCategory in recipeCategorys)
                {
                    ctx.RecipeCategory.Remove(recipeCategory);
                }
                ctx.Recipe.Remove(data);
                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Recipe GetById(int id)
        {
            var recipe = ctx.Recipe.Find(id);

            if (recipe != null)
            {
                var categoryNames = ctx.RecipeCategory
                                       .Where(rc => rc.RecipeId == recipe.Id)
                                       .Join(ctx.Category, rc => rc.CategoryId, c => c.Id, (rc, c) => c.CategoryName)
                                       .ToList();
                recipe.CategoryNames = string.Join(", ", categoryNames);
            }

            return recipe;
        }

        public RecipeListVM List(string term = "", int? categoryId = null, bool paging = false, int currentPage = 0, string sortOrder = "")
        {
            var data = new RecipeListVM();
            var list = ctx.Recipe.AsQueryable();

            // Пошук
            if (!string.IsNullOrEmpty(term))
            {
                term = term.ToLower();
                list = list.Where(a => a.Title.ToLower().Contains(term));
            }

            // Фільтрація за категоріями
            if (categoryId.HasValue)
            {
                var recipeIds = ctx.RecipeCategory
                                  .Where(rc => rc.CategoryId == categoryId)
                                  .Select(rc => rc.RecipeId)
                                  .ToList();
                list = list.Where(r => recipeIds.Contains(r.Id));
            }

            // Сортування
            if (!string.IsNullOrEmpty(sortOrder))
            {
                if (sortOrder.ToLower() == "asc")
                {
                    list = list.OrderBy(r => r.Title);
                }
                else if (sortOrder.ToLower() == "desc")
                {
                    list = list.OrderByDescending(r => r.Title);
                }
            }

            // Пагінація
            if (paging)
            {
                int pageSize = 10;
                int count = list.Count();
                int totalPages = (int)Math.Ceiling(count / (double)pageSize);
                list = list.Skip((currentPage - 1) * pageSize).Take(pageSize);
                data.PageSize = pageSize;
                data.CurrentPage = currentPage;
                data.TotalPages = totalPages;
            }

            var recipeList = list.ToList();
            foreach (var recipe in recipeList)
            {
                var categoryNames = ctx.RecipeCategory
                                       .Where(rc => rc.RecipeId == recipe.Id)
                                       .Join(ctx.Category, rc => rc.CategoryId, c => c.Id, (rc, c) => c.CategoryName)
                                       .ToList();
                recipe.CategoryNames = string.Join(", ", categoryNames);
            }

            data.RecipeList = recipeList.AsQueryable();
            return data;
        }

        public bool Update(Recipe model)
        {
            try
            {
                var categorysToDelete = ctx.RecipeCategory.Where(a => a.RecipeId == model.Id && !model.Categorys.Contains(a.CategoryId)).ToList();
                foreach (var rCat in categorysToDelete)
                {
                    ctx.RecipeCategory.Remove(rCat);
                }
                foreach (int catId in model.Categorys)
                {
                    var recipeCategory = ctx.RecipeCategory.FirstOrDefault(a => a.RecipeId == model.Id && a.CategoryId == catId);
                    if (recipeCategory == null)
                    {
                        recipeCategory = new RecipeCategory
                        {
                            CategoryId = catId,
                            RecipeId = model.Id
                        };
                        ctx.RecipeCategory.Add(recipeCategory);
                    }
                }
                ctx.Recipe.Update(model);
                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<int> GetCategoryByRecipeId(int recipeId)
        {
            var categoryIds = ctx.RecipeCategory.Where(a => a.RecipeId == recipeId).Select(a => a.CategoryId).ToList();
            return categoryIds;
        }
    }
}