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
            catch(Exception ex)
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
                var recipeCategorys =ctx.RecipeCategory.Where(a => a.RecipeId == data.Id);
                foreach (var recipeCategory  in recipeCategorys)
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
            return ctx.Recipe.Find(id);
        }

        public RecipeListVM List(string term="",bool paging=false,int currentPage=0 )
        {
            var data = new RecipeListVM();

            var list= ctx.Recipe.ToList();
       
            if(!string.IsNullOrEmpty(term))
            {
                term = term.ToLower();
                list = list.Where(a => a.Title.ToLower().Contains(term)).ToList();
            }
            if (paging)
            {
                //paging
                int pageSize = 5;
                int count = list.Count;
                int TotalPages = (int)Math.Ceiling(count / (double)pageSize);
                list=list.Skip((currentPage - 1)*pageSize).Take(pageSize).ToList();
                data.PageSize = pageSize;
                data.CurrentPage = currentPage;
                data.TotalPages = TotalPages;
            }

            foreach (var recipe in list)
            {
                var categorys = (from category in ctx.Category
                                 join rc in ctx.RecipeCategory
                               on category.Id equals rc.CategoryId
                                 where rc.RecipeId == recipe.Id
                                 select category.CategoryName
                               ).ToList();
                var categoryNames = string.Join(',', categorys);
                recipe.CategoryNames = categoryNames;
            }
            data.RecipeList = list.AsQueryable();

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
