using Moq;
using RecipeBookMvc.Controllers;
using RecipeBookMvc.Models.DTO;
using RecipeBookMvc.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using System.Threading.Tasks;

namespace RecipeBookMvc.Tests
{
    public class UserAuthenticationControllerTests
    {
        [Fact]
        public async Task Register_ValidModel_ReturnsRedirectToLogin()
        {

            var mockAuthService = new Mock<IUserAuthenticationService>();
            var controller = new UserAuthenticationController(mockAuthService.Object);

            var validModel = new RegistrationModel
            {
                Name = "Test User",
                Email = "test@example.com",
                Username = "testuser",
                Password = "Password123!",
                PasswordConfirm = "Password123!",
                Role = "User"
            };

            mockAuthService
                .Setup(service => service.RegisterAsync(validModel))
                .ReturnsAsync(new Status { StatusCode = 1, Message = "Реєстрація успішна" });

            var result = await controller.Register(validModel) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Login", result.ActionName);
        }

        [Fact]
        public async Task Register_InvalidModel_ReturnsViewWithModel()
        {
            var mockAuthService = new Mock<IUserAuthenticationService>();
            var _controller = new UserAuthenticationController(mockAuthService.Object);

            var model = new RegistrationModel();
            _controller.ModelState.AddModelError("Email", "Email is required.");


            var result = await _controller.Register(model) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal(model, result.Model);
        }
        [Fact]
        public async Task Login_ValidCredentials_ReturnsRedirectToHomeIndex()
        {
            var mockAuthService = new Mock<IUserAuthenticationService>();
            var _controller = new UserAuthenticationController(mockAuthService.Object);

            var model = new LoginModel { Username = "testuser", Password = "P@ssw0rd" };
            mockAuthService.Setup(service => service.LoginAsync(model))
                            .ReturnsAsync(new Status { StatusCode = 1 });

            var result = await _controller.Login(model) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Home", result.ControllerName);
        }
        [Fact]
        public async Task Login_InvalidCredentials_ReturnsLoginViewWithMessage()
        {
            var mockAuthService = new Mock<IUserAuthenticationService>();
            var _controller = new UserAuthenticationController(mockAuthService.Object);

            var model = new LoginModel { Username = "testuser", Password = "wrongpassword" };
            mockAuthService.Setup(service => service.LoginAsync(model))
                            .ReturnsAsync(new Status { StatusCode = 0, Message = "Invalid login" });

            var result = await _controller.Login(model) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal("Login", result.ViewName);
            Assert.Equal("Invalid login", _controller.TempData["msg"]);
        }
    }
}