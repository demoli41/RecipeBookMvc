using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RecipeBookMvc.Models.Domain;
using RecipeBookMvc.Models.DTO;
using RecipeBookMvc.Repositories.Abstract;
using System.Security.Claims;

namespace RecipeBookMvc.Controllers
{
    [Authorize]
    public class RecipeController : Controller
    {

        private readonly IRecipeService _recipeService;
        private readonly IFileService _fileService;
        private readonly ICategoryService _categoryService;
        public RecipeController(IRecipeService RecipeService, IFileService fileService, ICategoryService categoryService)
        {
            _recipeService = RecipeService;
            _fileService = fileService;
            _categoryService = categoryService;
        }
        public IActionResult Add()
        {
            var model = new Recipe();
            model.CategoryList = _categoryService.List().Select(a => new SelectListItem { Text = a.CategoryName, Value = a.Id.ToString() });
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(Recipe model)
        {
            model.CategoryList = _categoryService.List().Select(a => new SelectListItem { Text = a.CategoryName, Value = a.Id.ToString() });
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            model.UserId = userId;

            if (model.ImageFile != null)
            {
                model.RecipeImage = model.ImageFile.FileName;

                var fileResult = this._fileService.SaveImage(model.ImageFile);
                if (fileResult.Item1 == 0)
                {
                    TempData["msg"] = "File could not be saved";
                    return View(model);
                }
                var imageName = fileResult.Item2;
                model.RecipeImage = imageName;
            }
            else
            {
                model.RecipeImage = "default-image.jpg";
            }

            if (!ModelState.IsValid)
                return View(model);

            var result = _recipeService.Add(model);
            if (result)
            {
                TempData["msg"] = "Recipe added successfully";
                return RedirectToAction(nameof(Add));
            }
            else
            {
                TempData["msg"] = "Error on server side";
                return View(model);
            }
        }



        public IActionResult Edit(int id)
        {
            var model = _recipeService.GetById(id);
            var selectedCategorys = _recipeService.GetCategoryByRecipeId(model.Id);
            MultiSelectList multiCategoryList = new MultiSelectList(_categoryService.List(), "Id", "CategoryName", selectedCategorys);
            model.MultiCategoryList = multiCategoryList;
            return View(model);
        }



        [HttpPost]
        public IActionResult Edit(Recipe model)
        {
            var selectedCategorys = _recipeService.GetCategoryByRecipeId(model.Id);
            MultiSelectList multiCategoryList = new MultiSelectList(_categoryService.List(), "Id", "CategoryName", selectedCategorys);
            model.MultiCategoryList = multiCategoryList;

            if (!ModelState.IsValid)
                return View(model);

            // Отримуємо ID користувача
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            model.UserId = userId; // Встановлюємо правильний UserId

            if (model.ImageFile != null)
            {
                var fileResult = this._fileService.SaveImage(model.ImageFile);
                if (fileResult.Item1 == 0)
                {
                    TempData["msg"] = "File could not be saved";
                    return View(model);
                }
                var imageName = fileResult.Item2;
                model.RecipeImage = imageName;
            }

            var result = _recipeService.Update(model);
            if (result)
            {
                TempData["msg"] = "Recipe updated successfully";
                return RedirectToAction(nameof(RecipeList));
            }
            else
            {
                TempData["msg"] = "Error on server side";
                return View(model);
            }
        }


        public IActionResult RecipeList(string term = "")
        {
            IQueryable<Recipe> data;

            if (User.IsInRole("Admin"))
            {
                data = _recipeService.List(term).RecipeList;
            }
            else
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                data = _recipeService.List(term).RecipeList.Where(r => r.UserId == userId);
            }

            return View(new RecipeListVM { RecipeList = data });
        }

        public IActionResult Delete(int id)
        {
            var result = _recipeService.Delete(id);
            return RedirectToAction(nameof(RecipeList));

        }
    }
}