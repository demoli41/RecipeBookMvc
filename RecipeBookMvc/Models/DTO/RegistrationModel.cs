using System.ComponentModel.DataAnnotations;

namespace RecipeBookMvc.Models.DTO
{
    public class RegistrationModel
    {
        [Required(ErrorMessage ="Name required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email required")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Login required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password required")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*[#$^+=!*()@%&]).{6,}$", ErrorMessage = "Minimum length is 6 and must contain 1 upper case, 1 lower case, 1 special character and 1 number")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Password confirm required")]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }
        //public string Role { get; set; } = "User";
        public string Role { get; set; }
    }
}
