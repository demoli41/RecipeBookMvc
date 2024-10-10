using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RecipeBookMvc.Models.Domain;
using RecipeBookMvc.Repositories.Abstract;

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
            model.CategoryList = _categoryService.List().Select(a=>new SelectListItem { Text=a.CategoryName,Value=a.Id.ToString()});
            return View(model);
        }

        [HttpPost]

        public IActionResult Add(Recipe model)
        {
            model.CategoryList = _categoryService.List().Select(a => new SelectListItem { Text = a.CategoryName, Value = a.Id.ToString() });
            model.RecipeImage = model.ImageFile.FileName;

            if (!ModelState.IsValid)
                return View(model);
            if (model.RecipeImage != null) 
            { 
            var fileResult=this._fileService.SaveImage(model.ImageFile);
            if(fileResult.Item1==0)
            {
                TempData["msg"] = "File could not saved";
                    return View(model);
                }
            var imageName = fileResult.Item2;
            model.RecipeImage = imageName;
            }   
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
            MultiSelectList multiCategoryList=new MultiSelectList(_categoryService.List(), "Id", "CategoryName", selectedCategorys);
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
            if (model.RecipeImage != null)
            {
                var fileResult = this._fileService.SaveImage(model.ImageFile);
                if (fileResult.Item1 == 0)
                {
                    TempData["msg"] = "File could not saved";
                    return View(model);
                }
                var imageName = fileResult.Item2;
                model.RecipeImage = imageName;
            }
            var result = _recipeService.Update(model);
            if (result)
            {
                TempData["msg"] = "Recipe added successfully";
                return RedirectToAction(nameof(RecipeList));
            }
            else
            {
                TempData["msg"] = "Error on server side";
                return View(model);
            }
        }

        public IActionResult RecipeList()
        {
            var data = this._recipeService.List();
            return View(data);
        }

        public IActionResult Delete(int id)
        {
            var result = _recipeService.Delete(id);
            return RedirectToAction(nameof(RecipeList));
           
        }
    }
}
