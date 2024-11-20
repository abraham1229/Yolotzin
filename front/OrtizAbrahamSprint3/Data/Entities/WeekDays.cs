using System.ComponentModel.DataAnnotations;

//To store the days options for every class

namespace OrtizAbrahamSprint3.Data.Entities
{
    public class WeekDays
    {
        [Key]
        public int WeekDaysID { get; set; }
        public string WeekDaysName { get; set; }
    }
}
