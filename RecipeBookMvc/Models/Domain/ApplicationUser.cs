using Microsoft.AspNetCore.Identity;

namespace RecipeBookMvc.Models.Domain
{
    public class ApplicationUser:IdentityUser
    {
        public string Name {  get; set; }
    }
}
