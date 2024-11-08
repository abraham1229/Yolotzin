    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using OrtizAbrahamSprint3.Data;

    namespace OrtizAbrahamSprint3.Pages
    {
        public class RegistrationsModel : PageModel
        {
            //Declare the class for every ID
            [BindProperty]
            public int InstructorID { get; set; }
            [BindProperty]
            public int StyleID { get; set; }
            [BindProperty]
            public int LevelID { get; set; }
            [BindProperty]
            public int AgeRangeID { get; set; }
            [BindProperty]
            public int WeekDaysID { get; set; }

            //Declare the lists to get the data
            public SelectList listofinstructors { get; set; }
            public SelectList listofstyles { get; set; }
            public SelectList listoflevels { get; set; }
            public SelectList listofagerange { get; set; }
            public SelectList listofweekdays { get; set; }

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
                listofinstructors = new SelectList(_myApplicationDbContext.Instructor, "InstructorID", "FirstName");
                listofstyles = new SelectList(_myApplicationDbContext.Style, "StyleID", "StyleName");
                listoflevels = new SelectList(_myApplicationDbContext.Levels, "LevelID", "LevelName");
                listofagerange = new SelectList(_myApplicationDbContext.AgeRange, "AgeRangeID", "RangeName");
                listofweekdays = new SelectList(_myApplicationDbContext.WeekDays, "WeekDaysID", "WeekDaysName");
            }
        }
    }
