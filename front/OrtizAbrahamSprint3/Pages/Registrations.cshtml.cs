using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrtizAbrahamSprint3.Data;
using OrtizAbrahamSprint3.Data.Entities;
using System.Reflection.Metadata.Ecma335;

namespace OrtizAbrahamSprint3.Pages
{
    //[Authorize]
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

        public void OnGet()
        {
            //Populate the dropdownlist with all possible chair types from our database table.
            listofagerange = new SelectList(_myApplicationDbContext.AgeRange, "AgeRangeID", "RangeName");
            listoflevels = new SelectList(_myApplicationDbContext.Levels, "LevelID", "LevelName");
            listofstyles = new SelectList(_myApplicationDbContext.Style, "StyleID", "StyleName");
        }

        public IActionResult OnPostEnroll()
        {
            if (AgeRangeID == 0)
            {
                MessageAgeRange = "Please select an age range";
                listofagerange = new SelectList(_myApplicationDbContext.AgeRange, "AgeRangeID", "RangeName");
            }

            return Page();
        }
    }
        
}
