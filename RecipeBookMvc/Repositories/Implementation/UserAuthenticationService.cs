
using Microsoft.AspNetCore.Identity;
using RecipeBookMvc.Models.Domain;
using RecipeBookMvc.Models.DTO;
using RecipeBookMvc.Repositories.Abstract;
using System.Security.Claims;

namespace RecipeBookMvc.Repositories.Implementation
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public UserAuthenticationService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;

        }

        public async Task<Status> RegisterAsync(RegistrationModel model)
        {
            var status = new Status();
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                status.StatusCode = 0;
                status.Message = "Користувач уже існує";
                return status;
            }
            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                Name = model.Name,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                status.StatusCode = 0;
                status.Message = "Невдалось зареєструвати";
                return status;
            }

            if (!await roleManager.RoleExistsAsync(model.Role))
                await roleManager.CreateAsync(new IdentityRole(model.Role));


            if (await roleManager.RoleExistsAsync(model.Role))
            {
                await userManager.AddToRoleAsync(user, model.Role);
            }

            status.StatusCode = 1;
            status.Message = "Реєстрація успішна";
            return status;
        }


        public async Task<Status> LoginAsync(LoginModel model)
        {
            var status = new Status();
            var user = await userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                status.StatusCode = 0;
                status.Message = "Неправильне ім'я";
                return status;
            }

            if (!await userManager.CheckPasswordAsync(user, model.Password))
            {
                status.StatusCode = 0;
                status.Message = "Неправильний пароль";
                return status;
            }

            var signInResult = await signInManager.PasswordSignInAsync(user, model.Password, true, true);
            if (signInResult.Succeeded)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                status.StatusCode = 1;
                status.Message = "Вхід успішний";
            }
            else if (signInResult.IsLockedOut)
            {
                status.StatusCode = 0;
                status.Message = "Користувач вийшов";
            }
            else
            {
                status.StatusCode = 0;
                status.Message = "Помилка входу";
            }

            return status;
        }

        public async Task LogoutAsync()
        {
            await signInManager.SignOutAsync();

        }

        //public async Task<Status> ChangePasswordAsync(ChangePasswordModel model, string username)
        //{
        //    var status = new Status();

        //    var user = await userManager.FindByNameAsync(username);
        //    if (user == null)
        //    {
        //        status.Message = "User does not exist";
        //        status.StatusCode = 0;
        //        return status;
        //    }
        //    var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
        //    if (result.Succeeded)
        //    {
        //        status.Message = "Password has updated successfully";
        //        status.StatusCode = 1;
        //    }
        //    else
        //    {
        //        status.Message = "Some error occcured";
        //        status.StatusCode = 0;
        //    }
        //    return status;

        //}
    }
}
