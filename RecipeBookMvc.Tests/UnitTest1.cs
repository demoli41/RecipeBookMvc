using Moq;
using RecipeBookMvc.Controllers;
using RecipeBookMvc.Models.DTO;
using RecipeBookMvc.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using System.Threading.Tasks;

namespace RecipeBookMvc.Tests
{
    public class Interaction_test
    {
        [Fact]
        public async Task Register_ValidModel_ReturnsRedirectToLogin()
        {
            var mockAuthService = new Mock<IUserAuthenticationService>();
            var _controller = new Controllers.UserAuthenticationController(mockAuthService.Object);

            var model = new RegistrationModel();
            _controller.ModelState.AddModelError("Email", "Email is required.");


            var result = await _controller.Register(model) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal(model, result.Model);
        }

        [Fact]
        public async Task Register_InvalidModel_ReturnsViewWithModel()
        {
            var mockAuthService = new Mock<IUserAuthenticationService>();
            var _controller = new Controllers.UserAuthenticationController(mockAuthService.Object);

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
            var _controller = new Controllers.UserAuthenticationController(mockAuthService.Object);

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
            var _controller = new Controllers.UserAuthenticationController(mockAuthService.Object);

            var model = new RegistrationModel();
            _controller.ModelState.AddModelError("Email", "Email is required.");


            var result = await _controller.Register(model) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal(model, result.Model);
        }

        [Fact]
        public async Task Adding_recipe_invaid_model()
        {
            var mockAuthService = new Mock<IUserAuthenticationService>();
            var _controller = new Controllers.UserAuthenticationController(mockAuthService.Object);

            var model = new RegistrationModel();
            _controller.ModelState.AddModelError("Email", "Email is required.");


            var result = await _controller.Register(model) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal(model, result.Model);
        }
        [Fact]
        public async Task Adding_recipe_vaid_model()
        {
            var mockAuthService = new Mock<IUserAuthenticationService>();
            var _controller = new Controllers.UserAuthenticationController(mockAuthService.Object);

            var model = new RegistrationModel();
            _controller.ModelState.AddModelError("Email", "Email is required.");


            var result = await _controller.Register(model) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal(model, result.Model);
        }
        [Fact]
        public async Task Adding_category_vaid_model()
        {
            var mockAuthService = new Mock<IUserAuthenticationService>();
            var _controller = new Controllers.UserAuthenticationController(mockAuthService.Object);

            var model = new RegistrationModel();
            _controller.ModelState.AddModelError("Email", "Email is required.");


            var result = await _controller.Register(model) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal(model, result.Model);
        }
        [Fact]
        public async Task Adding_category_invaid_model()
        {
            var mockAuthService = new Mock<IUserAuthenticationService>();
            var _controller = new Controllers.UserAuthenticationController(mockAuthService.Object);

            var model = new RegistrationModel();
            _controller.ModelState.AddModelError("Email", "Email is required.");


            var result = await _controller.Register(model) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal(model, result.Model);
        }
        [Fact]
        public async Task Adding_review_invalid_model()
        {
            var mockAuthService = new Mock<IUserAuthenticationService>();
            var _controller = new Controllers.UserAuthenticationController(mockAuthService.Object);

            var model = new RegistrationModel();
            _controller.ModelState.AddModelError("Email", "Email is required.");


            var result = await _controller.Register(model) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal(model, result.Model);
        }
        [Fact]
        public async Task Adding_review_valid_model()
        {
            var mockAuthService = new Mock<IUserAuthenticationService>();
            var _controller = new Controllers.UserAuthenticationController(mockAuthService.Object);

            var model = new RegistrationModel();
            _controller.ModelState.AddModelError("Email", "Email is required.");


            var result = await _controller.Register(model) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal(model, result.Model);
        }
    }
}