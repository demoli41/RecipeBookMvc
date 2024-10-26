using System.ComponentModel.DataAnnotations;

namespace RecipeBookMvc.Models.DTO
{
    public class RegistrationModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Username { get; set; }

        [Required]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*[#$^+=!*()@%&]).{6,}$", ErrorMessage = "Мінімальна довжина 6 і має містити 1 великий регістр, 1 нижній регістр, 1 спеціальний символ і 1 цифру")]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }
        public string Role { get; set; } = "User";
        //public string Role { get; set; } ;
    }
}
