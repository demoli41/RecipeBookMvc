using System.ComponentModel.DataAnnotations;

namespace RecipeBookMvc.Models.DTO
{
    public class RegistrationModel
    {
        [Required(ErrorMessage ="Ім'я є обов'язковим")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Пошта є обов'язковою")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Логін є обов'язковим")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Пароль є обов'язковим")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*[#$^+=!*()@%&]).{6,}$", ErrorMessage = "Мінімальна довжина 6 і має містити 1 великий регістр, 1 нижній регістр, 1 спеціальний символ і 1 цифру")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Підтвердження є обов'язковим")]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }
        public string Role { get; set; } = "User";
        //public string Role { get; set; }
    }
}
