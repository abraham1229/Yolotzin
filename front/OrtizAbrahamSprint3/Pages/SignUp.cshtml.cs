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
        //Guardian information
        [BindProperty]
        [RegularExpression(@"^[a-zA-Z ]{1,30}$", ErrorMessage = "Only letters are allowed.")]
        [Required(ErrorMessage = "Please enter guardian's first name")]
        public string FirstNameGuardian { get; set; }

        [BindProperty]
        [RegularExpression(@"^[a-zA-Z ]{1,30}$", ErrorMessage = "Only letters are allowed.")]
        [Required(ErrorMessage = "Please enter guardian's last name")]
        public string LastNameGuardian { get; set; }

        [BindProperty]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Please enter a valid email address.")]
        [Required(ErrorMessage = "Please enter guardian's email address")]
        public string EmailAddressGuardian { get; set; }

        [BindProperty]
        [RegularExpression(@"^\+?\d{8,30}$", ErrorMessage = "Please enter a valid phone number.")]
        [Required(ErrorMessage = "Please enter guardian's phone number")]
        public string PhoneNumberGuardian { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please enter guardian's birthday")]
        public DateOnly BirthdayGuardian { get; set; }

        //User information
        [BindProperty]
        [RegularExpression(@"^[a-zA-Z ]{1,30}$", ErrorMessage = "Only letters are allowed.")]
        [Required(ErrorMessage = "Please enter your first name")]
        public string FirstNameUser { get; set; }

        [BindProperty]
        [RegularExpression(@"^[a-zA-Z ]{1,30}$", ErrorMessage = "Only letters are allowed.")]
        [Required(ErrorMessage = "Please enter your last name")]
        public string LastNameUser { get; set; }

        [BindProperty]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Please enter a valid email address.")]
        [Required(ErrorMessage = "Please enter your email address")]
        public string EmailAddressUser { get; set; }

        [BindProperty]
        [RegularExpression(@"^\+?\d{10,13}$", ErrorMessage = "Please enter a valid phone number.")]
        [Required(ErrorMessage = "Please enter your phone number")]
        public string PhoneNumberUser { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please enter your birthday")]
        public DateOnly BirthdayUser { get; set; }

        [BindProperty]
        [RegularExpression(@"^[a-zA-Z0-9_.-]{5,50}$", ErrorMessage = "Username must be 5-50 characters long and may contain letters, numbers, underscores, and dashes.")]
        [Required(ErrorMessage = "Please enter your username")]
        public string Username { get; set; }

        [BindProperty]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W)[A-Za-z\d\W]{8,}$", ErrorMessage = "Password must be at least 8 characters long and include uppercase, lowercase, number, and special character.")]
        [Required(ErrorMessage = "Please enter your password")]
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

        }

        //OnClick for the clear button
        public void OnPostClearForm()
        {
            // Clean form properties
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

            // Clean ModelState
            ModelState.Clear();
        }

        //OnClick for the Sign Up button
        public async Task<IActionResult> OnPostAddUser()
        {
            // Calculate age
            int ageGuardian = CalculateAge(BirthdayGuardian);
            int ageUser = CalculateAge(BirthdayUser);

            // Validate the age range 
            //If the user is not between the range
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
            //If guardian is under age
            else if (ageUser < 18 && ageGuardian < 120)
            {
                ClassNameDisplayGuardian = String.Empty;
                if (ageGuardian < 18)
                {
                    MessageAgeRangeGuardian = "Guardians must be older than 18";
                    return Page();
                }
            }

            //Hash and salt the password entered on the form if there is not age restriction
            if (UserPassword == null)
            {
                return Page();
            }

            CreatePasswordHash(UserPassword, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new Users();

            //Assign the data for the guardian if the user is underage
            if (ageUser < 18)
            {
                user = new Users
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
            }

            //Assign none to the guardian if the user is older than 18
            else
            {
                user = new Users
                {
                    FirstNameUser = FirstNameUser,
                    LastNameUser = LastNameUser,
                    EmailAddressUser = EmailAddressUser,
                    PhoneNumberUser = PhoneNumberUser,
                    BirthdayUser = BirthdayUser,
                    FirstNameGuardian = "none",
                    LastNameGuardian = "none",
                    EmailAddressGuardian = "none",
                    PhoneNumberGuardian = "none",
                    BirthdayGuardian = new DateOnly(2001, 1, 1),
                    Username = Username,
                    UserPasswordHash = passwordHash,
                    UserPasswordSalt = passwordSalt,
                    UserCreationDate = DateOnly.FromDateTime(DateTime.Today)
                };
            }


            //Inject data to the database
            try
            {
                _myApplicationDbContext.Users.Add(user);
                await _myApplicationDbContext.SaveChangesAsync();
                return RedirectToPage("/SignIn");
            }

            //Check if there are errors
            catch (Exception)
            {
                MessageError = "An unexpected error occurred";

                return Page();
            }
            



        }
        
        //Function to create thw hash ans salt for the password
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
