using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrtizAbrahamSprint3.Data;
using OrtizAbrahamSprint3.Data.Entities;
using System.Reflection.Metadata.Ecma335;

namespace OrtizAbrahamSprint3.Pages
{
    [Authorize]
    public class RegistrationsModel : PageModel
    {
        //Declare the class for every ID

        [BindProperty]
        public int AgeRangeID { get; set; }

        [BindProperty]
        public int LevelID { get; set; }
        [BindProperty]
        public int StyleID { get; set; }
        [BindProperty]
        public int UserID { get; set; }

        //Declare the data to insert in the table 
        [BindProperty]
        public Classes MyClasses { get; set; }

        //Declare the lists to get the data
        public SelectList listofagerange { get; set; }
        public SelectList listoflevels { get; set; }
        public SelectList listofstyles { get; set; }

        //Messages to send errors
        public string MessageAgeRange { get; set; }
        public string MessageLevel { get; set; }
        public string MessageStyle { get; set; }

        //This readonly variable needs to be the same name as your DbContext class previously created.
        private readonly MyApplicationDbContext _myApplicationDbContext;


        //The following code injects our Database context into our Razor Page Model.
        public RegistrationsModel(MyApplicationDbContext myApplicationDbContext)
        {
            _myApplicationDbContext = myApplicationDbContext;
        }

        //Lists to show class information
        public IList<AgeRange> listofagerangedata { get; set; }
        public IList<Levels> listoflevelsedata { get; set; }
        public IList<WeekDays> listofweekdata { get; set; }
        public IList<Style> listofstyledata { get; set; }
        public IList<Instructor> listofinstructordata { get; set; }

        //Classs information values
        public decimal classPrice { get; set; }
        public string classHour { get; set; }
        public string classDays { get; set; }
        public string classInstructor { get; set; }

        //Display message 
        public string displayClassInformation { get; set; }

        public void OnGet(int? age, int? level, int? style)
        {

            //Call populate dropdown function
            PopulateDropDown();

            //Check for the values to autopopulate list values (received in the argument of the funtion)
            if (age.HasValue)
            {
                AgeRangeID = age.Value;
            }
            if (level.HasValue)
            {
                LevelID = level.Value;
            }
            if (style.HasValue)
            {
                StyleID = style.Value;
            }

          
            //Display information just untill they choose the 3 options
            if (AgeRangeID != 0 && LevelID != 0 && StyleID != 0)
            {
                //Call the load information function
                LoadClassInformation();
            }
            else
            {
                displayClassInformation = "hide-information";
            }
        }

        public void OnPost()
        {
            //Display information just untill they choose the 3 options
            if (AgeRangeID != 0 && LevelID != 0 && StyleID != 0)
            {
                //Call the load information function
                LoadClassInformation();
            }
            else
            {
                displayClassInformation = "hide-information";
            }


            //Call populate dropdown function
            PopulateDropDown();
        }

        public async Task<IActionResult> OnPostEnroll()
        {
            //Declare bool to know if there was an error
            bool hasError = false;

            //Check if the user did not select all the data, if something is missing, return and ask to select data.
            if (AgeRangeID == 0)
            {
                MessageAgeRange = "Please select an age range";
                listofagerange = new SelectList(_myApplicationDbContext.AgeRange, "AgeRangeID", "RangeName");
                hasError = true;
            }
            if (LevelID == 0)
            {
                MessageLevel = "Please select a level";
                listoflevels = new SelectList(_myApplicationDbContext.Levels, "LevelID", "LevelName");
                hasError = true;
            }
            if (StyleID == 0)
            {
                MessageStyle = "Please select an sytle";
                listofstyles = new SelectList(_myApplicationDbContext.Style, "StyleID", "StyleName");
                hasError = true;
            }

            if (hasError)
            {
                return Page();
            }

            //If the user has selected everything, check for the ID and assign it to the userID forgein key
            MyClasses.UserID = int.TryParse(User.FindFirst("UserID")?.Value, out var id) ? id : 0;

            //Insert data into the database
            _myApplicationDbContext.Classes.Add(MyClasses);
            await _myApplicationDbContext.SaveChangesAsync();

            //Return to the home page
            return RedirectToPage("/Index");
        }

        //Function to populate the dropdownlist with all possible values from our database tables.
        private void PopulateDropDown()
        {
            listofagerange = new SelectList(_myApplicationDbContext.AgeRange, "AgeRangeID", "RangeName");
            listoflevels = new SelectList(_myApplicationDbContext.Levels, "LevelID", "LevelName");
            listofstyles = new SelectList(_myApplicationDbContext.Style, "StyleID", "StyleName");
        }

        // Function to load and validate class information based on selected options
        private void LoadClassInformation()
        {
            // Load all required data from the database tables
            listofagerangedata = _myApplicationDbContext.AgeRange.ToList();
            listoflevelsedata = _myApplicationDbContext.Levels.ToList();
            listofweekdata = _myApplicationDbContext.WeekDays.ToList();
            listofstyledata = _myApplicationDbContext.Style.ToList();
            listofinstructordata = _myApplicationDbContext.Instructor.ToList();

            // Check the selected age range and retrieve the price
            foreach (var item in listofagerangedata)
            {
                if (item.AgeRangeID == AgeRangeID)
                {
                    classPrice = item.Price;
                }
            }

            // Check the selected level and retrieve hours and days
            foreach (var item in listoflevelsedata)
            {
                if (item.LevelID == LevelID)
                {
                    classHour = $"{item.StartHour} - {item.EndHour}";

                    foreach (var week in listofweekdata)
                    {
                        if (week.WeekDaysID == item.WeekDaysID)
                        {
                            classDays = week.WeekDaysName;
                        }
                    }
                }
            }

            // Check the selected style and retrieve instructor information
            foreach (var style in listofstyledata)
            {
                if (style.StyleID == StyleID)
                {
                    foreach (var instructor in listofinstructordata)
                    {
                        if (instructor.StyleID == style.StyleID)
                        {
                            classInstructor = instructor.FirstNameInstructor;
                        }
                    }
                }
            }

            // Set display information status
            displayClassInformation = String.Empty;
        }
    }        
}
