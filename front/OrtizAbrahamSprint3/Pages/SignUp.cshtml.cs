using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrtizAbrahamSprint3.Data.Entities;
using OrtizAbrahamSprint3.Data;
using System.ComponentModel.DataAnnotations;

namespace OrtizAbrahamSprint3.Pages
{
    public class SignUpModel : PageModel
    {
        [BindProperty]
        [RegularExpression(@"^[a-zA-Z'-]+$", ErrorMessage = "Only letters, hyphens, and apostrophes are allowed.")]
        [Required(ErrorMessage = "Please enter your first name")]
        public string FirstName { get; set; }

        [RegularExpression(@"^[a-zA-Z'-]+$", ErrorMessage = "Only letters, hyphens, and apostrophes are allowed.")]
        [BindProperty]
        [Required(ErrorMessage = "Please enter your last name")]
        public string LastName { get; set; }

        [BindProperty]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Please enter a valid email address.")]
        [Required(ErrorMessage = "Please enter your email address")]
        public string EmailAddress { get; set; }

        [BindProperty]
        [RegularExpression(@"^\+?\d{0,3}[-.\s]?\(?\d{1,4}?\)?[-.\s]?\d{1,4}[-.\s]?\d{1,9}$", ErrorMessage = "Please enter a valid phone number.")]
        [Required(ErrorMessage = "Please enter your phone number")]
        public string PhoneNumber { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please enter your birthday")]
        public DateOnly Birthday { get; set; }

        [BindProperty]
        [RegularExpression(@"^[a-zA-Z0-9_.-]{5,20}$", ErrorMessage = "Username must be 5-20 characters long and may contain letters, numbers, underscores, and dashes.")]
        [Required(ErrorMessage = "Please enter your username")]
        public string Username { get; set; }
        [BindProperty]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Password must be at least 8 characters long and include uppercase, lowercase, number, and special character.")]
        [Required(ErrorMessage = "Please enter your password")]
        public string UserPassword { get; set; }

        [BindProperty]
        public string StyleNameID { get; set; }

        [BindProperty]
        public Users MyUsers { get; set; }

        //Use the SelecList class from drop down list
        public SelectList listofstylesdances { get; set; }

        //Add a readonly variable with the same name as your DbContext class previously created.
        private readonly MyApplicationDbContext _myApplicationDbContext;

        //The following code injects out database context into our razor page model. 
        public SignUpModel(MyApplicationDbContext myApplicationDbContext)
        {
            _myApplicationDbContext = myApplicationDbContext;
        }

        //Display error message for age
        public string MessageAgeRange { get; set; }

        // Method to calculate age based on the birthday
        private int CalculateAge(DateOnly birthDate)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            int age = today.Year - birthDate.Year;
            if (birthDate > today.AddYears(-age)) age--;
            return age;
        }

        //public IActionResult OnGet()
        //{
        //    //populate de dropdown list
        //    listofstylesdances = new SelectList(_myApplicationDbContext.Levels, "LevelID", "LevelName");
        //    return Page();
        //}

        public async Task<IActionResult> OnPostAddUser()
        {
            // Calculate age
            int age = CalculateAge(Birthday);
            if (age < 4 || age > 120)
            {
                MessageAgeRange = "The age must be between 4 and 120";
                return Page();
            }
            _myApplicationDbContext.Users.Add(MyUsers);
            await _myApplicationDbContext.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
