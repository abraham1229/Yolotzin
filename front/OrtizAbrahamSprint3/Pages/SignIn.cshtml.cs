using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrtizAbrahamSprint3.Data;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace OrtizAbrahamSprint3.Pages
{
    public class SignInModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string UserPassword { get; set; }

        public string Message { get; set; }

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

        public async Task<IActionResult> OnPostSignIn() 
        {
            //Create a variable for the user record
            var user = await _myApplicationDbContext.Users.FirstOrDefaultAsync(u => u.Username == Username);

            //Check if the user variable is n ull or if it is not verified with a valid password

            if (user == null || !VerifyPasswordHash(UserPassword, user.UserPasswordHash, user.UserPasswordSalt) )
            {
                Message = "Invalid login credentials, please try again";
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");

                return Page();
            }

            //If there is not previous return the credentials are valid
            //Create a claim to identify the user
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FirstNameUser),
                new Claim("UserID", user.UserID.ToString()) //Custom claim
           
            };

            // Create the authenticated cookie
            var claimsIdentity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, //keep the user logged in even after the browser closes.\
                ExpiresUtc = DateTime.UtcNow.AddMinutes(1)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return RedirectToPage("/Index");
        }

        public bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computesHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                //true if is the same password and false if not
                return computesHash.SequenceEqual(storedHash);
            }
            
        }
    }
}
