using Microsoft.AspNetCore.Mvc;
using RecipeBookMvc.Repositories.Abstract;

namespace RecipeBookMvc.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly IRecipeService _recipeService;
        public HomeController(IRecipeService recipeService)
        {
             _recipeService = recipeService;
        }
        public IActionResult Index(string term="", int currentPage = 1)
        {
            var recipes = _recipeService.List(term,true,currentPage);
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
