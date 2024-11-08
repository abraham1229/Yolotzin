using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrtizAbrahamSprint3.Data.Entities;
using OrtizAbrahamSprint3.Data;
using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


//Preguntar como hacer que guarde el anio, de la misma manera
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

            // Validate the age range 
            if (age < 4 || age > 120)
            {
                MessageAgeRange = "We are sorry, you need to be older than 4 and younger than 120";
            }

            try
            {
                _myApplicationDbContext.Users.Add(MyUsers);
                await _myApplicationDbContext.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            //Check if there are errors
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2627)
                {
                    // Checks for each unique constraint and adds corresponding errors
                    bool hasError = false;

                    //Error for email adrress alreday in use
                    if (sqlEx.Message.Contains("UQ__Users__49A14740AEAD3FD2"))
                    {
                        ModelState.AddModelError("EmailAddress", "This email address is already in use.");
                        hasError = true;
                    }
                    //Error for phone number alreday in use
                    if (sqlEx.Message.Contains("UQ__Users__85FB4E38A995260E")) 
                    {
                        ModelState.AddModelError("PhoneNumber", "This phone number is already in use.");
                        hasError = true;
                    }
                    //Error for username alreday in use
                    if (sqlEx.Message.Contains("UQ__Users__536C85E48E43F32B")) 
                    {
                        ModelState.AddModelError("Username", "This username is already taken.");
                        hasError = true;
                    }
                    //Another error
                    if (!hasError)
                    {
                        ModelState.AddModelError("", "A unique constraint error occurred.");
                    }
                }
                else
                {
                    // Handle any other unexpected errors
                    ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                }
            }

            return Page();
        }
    }
}
