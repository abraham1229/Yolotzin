using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrtizAbrahamSprint3.Data;
using System.ComponentModel.DataAnnotations;

namespace OrtizAbrahamSprint3.Pages
{
    public class SignInModel : PageModel
    {
        [BindProperty]
        [RegularExpression(@"^[a-zA-Z0-9_.-]{5,20}$", ErrorMessage = "Username must be 5-20 characters long and may contain letters, numbers, underscores, and dashes.")]
        [Required(ErrorMessage = "Please enter your username")]
        public string Username { get; set; }

        [BindProperty]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Password must be at least 8 characters long and include uppercase, lowercase, number, and special character.")]
        [Required(ErrorMessage = "Please enter your password")]
        public string UserPassword { get; set; }

        //Add a readonly variable with the same name as your DbContext class previously created.
        private readonly MyApplicationDbContext _myApplicationDbContext;

        //The following code injects out database context into our razor page model. 
        public SignInModel(MyApplicationDbContext myApplicationDbContext)
        {
            _myApplicationDbContext = myApplicationDbContext;
        }

        public void OnGet()
        {
        }
    }
}
