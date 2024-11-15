using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OrtizAbrahamSprint3.Pages
{
    public class SignOutModel : PageModel
    {
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostCancel()
        {
            return RedirectToPage("/Index");
        }

        public async Task<IActionResult> OnPostSignOut()
        {
            //Sign the user out by removing the authentication cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            //Redirect to the home page
            return RedirectToPage("/Index");
        }
    }
}
