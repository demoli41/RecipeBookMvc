using Microsoft.AspNetCore.Mvc;
using RecipeBookMvc.Models.DTO;
using RecipeBookMvc.Repositories.Abstract;
using System.Drawing.Text;

namespace RecipeBookMvc.Controllers
{
    public class UserAuthenticationController : Controller
    {
        private IUserAuthenticationService authService;
        public UserAuthenticationController(IUserAuthenticationService authService)
        {
            this.authService = authService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegistrationModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await authService.RegisterAsync(model);
            if (result.StatusCode == 1)
            {
                TempData["msg"] = "Реєстрація успішна";
                return RedirectToAction(nameof(Login));
            }
            else
            {
                TempData["msg"] = result.Message;
                return View(model);
            }
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await authService.LoginAsync(model);
            if (result.StatusCode == 1)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["msg"] = "Could not log in";
                return RedirectToAction(nameof(Login));
            }
        }

        public async Task<IActionResult> Logout()
        {
            await authService.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }
    }

}
