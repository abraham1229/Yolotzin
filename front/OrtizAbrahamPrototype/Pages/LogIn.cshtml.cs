using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrtizAbrahamSprint3.Data;
using OrtizAbrahamSprint3.Data.Entities;
using System.ComponentModel.DataAnnotations;


namespace OrtizAbrahamSprint3.Pages
{
    public class LogInModel : PageModel
    {
        public string Username { get; set; }
        [BindProperty]
        public string StyleNameID { get; set; }
        //Use the SelecList class from drop down list
        public SelectList listofstylesdances { get; set; }

        //Add a readonly variable with the same name as your DbContext class previously created.
        private readonly MyApplicationDbContext _myApplicationDbContext;

        //The following code injects out database context into our razor page model. 
        public LogInModel(MyApplicationDbContext myApplicationDbContext)
        {
            _myApplicationDbContext = myApplicationDbContext;
        }

        public IActionResult OnGet()
        {
            ////populate de dropdown list
            listofstylesdances = new SelectList(_myApplicationDbContext.Levels, "LevelID", "LevelName");
            return Page();
        }
    }
}
