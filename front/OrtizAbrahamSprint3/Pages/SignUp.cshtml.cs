using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrtizAbrahamSprint3.Data.Entities;
using OrtizAbrahamSprint3.Data;
using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;


namespace OrtizAbrahamSprint3.Pages
{
    public class SignUpModel : PageModel
    {
        //Tutor information
        [BindProperty]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only letters are allowed.")]
        [Required(ErrorMessage = "Please enter guardian's first name")]
        public string FirstNameGuardian { get; set; }

        [BindProperty]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only letters are allowed.")]
        [Required(ErrorMessage = "Please enter guardian's last name")]
        public string LastNameGuardian { get; set; }

        [BindProperty]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Please enter a valid email address.")]
        [Required(ErrorMessage = "Please enter guardian's email address")]
        public string EmailAddressGuardian { get; set; }

        [BindProperty]
        [RegularExpression(@"^\+?\d{0,3}[-.\s]?\(?\d{1,4}?\)?[-.\s]?\d{1,4}[-.\s]?\d{1,9}$", ErrorMessage = "Please enter a valid phone number.")]
        [Required(ErrorMessage = "Please enter guardian's phone number")]
        public string PhoneNumberGuardian { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please enter guardian's birthday")]
        public DateOnly BirthdayGuardian { get; set; }

        //User information
        [BindProperty]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only letters are allowed.")]
        [Required(ErrorMessage = "Please enter your first name")]
        public string FirstNameUser { get; set; }

        [BindProperty]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only letters are allowed.")]
        [Required(ErrorMessage = "Please enter your last name")]
        public string LastNameUser { get; set; }

        [BindProperty]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Please enter a valid email address.")]
        [Required(ErrorMessage = "Please enter your email address")]
        public string EmailAddressUser { get; set; }

        [BindProperty]
        [RegularExpression(@"^\+?\d{0,3}[-.\s]?\(?\d{1,4}?\)?[-.\s]?\d{1,4}[-.\s]?\d{1,9}$", ErrorMessage = "Please enter a valid phone number.")]
        [Required(ErrorMessage = "Please enter your phone number")]
        public string PhoneNumberUser { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please enter your birthday")]
        public DateOnly BirthdayUser { get; set; }

        [BindProperty]
        //[RegularExpression(@"^[a-zA-Z0-9_.-]{5,20}$", ErrorMessage = "Username must be 5-20 characters long and may contain letters, numbers, underscores, and dashes.")]
        //[Required(ErrorMessage = "Please enter your username")]
        public string Username { get; set; }

        [BindProperty]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Password must be at least 8 characters long and include uppercase, lowercase, number, and special character.")]
        //[Required(ErrorMessage = "Please enter your password")]
        public string UserPassword { get; set; }

        public string MessageError { get; set; }


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
        public string MessageAgeRangeGuardian { get; set; }
        public string MessageAgeRangeUser { get; set; }

        //Change the name of the class if the user is underage
        [BindProperty]
        public string ClassNameDisplayGuardian { get; set; } = "hide";

        // Method to calculate age based on the birthday
        static int CalculateAge(DateOnly birthDate)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            int age = today.Year - birthDate.Year;
            if (birthDate > today.AddYears(-age)) age--;
            return age;
        }

        public void OnGet()
        {
            //populate de dropdown list
            //listofstylesdances = new SelectList(_myApplicationDbContext.Levels, "LevelID", "LevelName");
            //return Page();
        }

        public void OnPostClearForm()
        {

            // Limpiar propiedades del formulario
            FirstNameGuardian = String.Empty;
            LastNameGuardian = String.Empty;
            EmailAddressGuardian = String.Empty;
            PhoneNumberGuardian = String.Empty;
            BirthdayGuardian = default;

            FirstNameUser = String.Empty;
            LastNameUser = String.Empty;
            EmailAddressUser = String.Empty;
            PhoneNumberUser = String.Empty;
            BirthdayUser = default;

            Username = String.Empty;
            UserPassword = String.Empty;

            // Limpiar ModelState
            ModelState.Clear();

        }


        public async Task<IActionResult> OnPostAddUser()
        {
            // Calculate age
            int ageGuardian = CalculateAge(BirthdayGuardian);
            int ageUser = CalculateAge(BirthdayUser);

            // Validate the age range 
            //If the user is not in the range
            if (ageUser < 4 || ageUser > 120)
            {
                MessageAgeRangeUser = "Users must be between 4 and 120";
                return Page();
            }
            //If the user needs a guardian
            else if (ageUser < 18 && ageGuardian > 120)
            {
                //Show guardians section
                ClassNameDisplayGuardian = String.Empty;
                //Delete guardians modal
                ModelState.Remove("FirstNameGuardian");
                ModelState.Remove("LastNameGuardian");
                ModelState.Remove("EmailAddressGuardian");
                ModelState.Remove("PhoneNumberGuardian");
                ModelState.Remove("BirthdayGuardian");
                return Page();
            }

            //Hash and salt the password entered on the form if there is not age restriction
            CreatePasswordHash(UserPassword, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new Users
            {
                FirstNameUser = FirstNameUser,
                LastNameUser = LastNameUser,
                EmailAddressUser = EmailAddressUser,
                PhoneNumberUser = PhoneNumberUser,
                BirthdayUser = BirthdayUser,
                FirstNameGuardian = FirstNameGuardian,
                LastNameGuardian = LastNameGuardian,
                EmailAddressGuardian = EmailAddressGuardian,
                PhoneNumberGuardian = PhoneNumberGuardian,
                BirthdayGuardian = BirthdayGuardian,
                Username = Username,
                UserPasswordHash = passwordHash,
                UserPasswordSalt = passwordSalt,
                UserCreationDate = DateOnly.FromDateTime(DateTime.Today)
            };


            try
            {
                _myApplicationDbContext.Users.Add(user);
                await _myApplicationDbContext.SaveChangesAsync();
                return RedirectToPage("/Index");
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
                        ModelState.AddModelError("EmailAddressUser", "This email address is already in use.");
                        hasError = true;
                    }
                    //Error for phone number alreday in use
                    if (sqlEx.Message.Contains("UQ__Users__85FB4E38A995260E"))
                    {
                        ModelState.AddModelError("PhoneNumberUser", "This phone number is already in use.");
                        hasError = true;
                    }
                    //Error for username alreday in use
                    if (sqlEx.Message.Contains("UQ__Users__536C85E4802C7F28"))
                    {
                        ModelState.AddModelError("Username", "This username is already taken.");
                        hasError = true;
                    }
                    //Another error
                    if (!hasError)
                    {
                        MessageError = "There was an authentication error";
                    }
                }
                else
                {
                    // Handle any other unexpected errors
                    MessageError = "An unexpected error occurred. Please try again.";
                }
            }

            return Page();
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            //Existing classes to hash ans salt will be used
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

        }
    }
}
