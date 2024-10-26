using Microsoft.AspNetCore.Mvc;
using RecipeBookMvc.Repositories.Abstract;
using RecipeBookMvc.Repositories.Implementation;

namespace RecipeBookMvc.Controllers
{

    public class HomeController : Controller
    {
        private readonly IRecipeService _recipeService;
        private readonly ICategoryService _categoryService;
        public HomeController(IRecipeService recipeService, ICategoryService categoryService)
        {
            _recipeService = recipeService;
            _categoryService = categoryService;
        }
        public IActionResult Index(string term = "", int currentPage = 1, int? categoryId = null)
        {
            ViewBag.Categories = _categoryService.List();
            var recipes = _recipeService.List(term, categoryId, true, currentPage);
            return View(recipes);
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult RecipeDetail(int recipeId)
        {
            var recipe = _recipeService.GetById(recipeId);
            return View(recipe);
        }
    }
}