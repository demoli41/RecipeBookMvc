using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using RecipeBookMvc.Repositories.Abstract;
using RecipeBookMvc.Repositories.Implementation;
using RecipeBookMvc.Models.Domain;

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
        public IActionResult Index(string term = "", int currentPage = 1, int? categoryId = null, string sortOrder = "")
        {
            ViewBag.Categories = _categoryService.List();
            var recipes = _recipeService.List(term, categoryId, true, currentPage, sortOrder);
            return View(recipes);
        }

        public IActionResult About()
        {
            return View();
        }
        public IActionResult RecipeDetail(int id)
        {
            var recipe = _recipeService.GetById(id);
            return View(recipe);
        }
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Console.WriteLine($"Culture: {culture}");
            if (!string.IsNullOrEmpty(culture))
            {
                Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );
            }

            return LocalRedirect(returnUrl ?? "/");
        }

        [HttpPost]
        public IActionResult DeleteReview(int reviewId, int id)
        {
            var userId = User?.Identity?.Name; 
            if (userId == null)
            {
                return Unauthorized(); 
            }

            var isAdmin = User.IsInRole("Admin");
            if (_recipeService.DeleteReview(reviewId, userId, isAdmin))
            {
                return RedirectToAction("RecipeDetail", new { id });
            }

            return Forbid("You do not have permission to delete this review.");
        }

        [HttpPost]
        public IActionResult AddReview(int id, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return BadRequest("Review text cannot be empty.");
            }

            var userId = User?.Identity?.Name; 
            if (userId == null)
            {
                return Unauthorized();
            }

            var review = new Review
            {
                RecipeId = id,
                Text = text,
                UserId = userId,
                CreatedDate = DateTime.Now
            };

            if (_recipeService.AddReview(review))
            {
                return RedirectToAction("RecipeDetail", new { id=id });
            }

            return StatusCode(500, "Failed to add review.");
        }
    }
}