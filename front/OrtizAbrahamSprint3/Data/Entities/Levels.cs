using System.ComponentModel.DataAnnotations;

//To store all the possible levels and when this will be

namespace OrtizAbrahamSprint3.Data.Entities
{
    public class Levels
    {
        [Key]
        public int LevelID { get; set; }
        public string LevelName { get; set; }
        public string StartHour { get; set; }
        public string EndHour { get; set; }
        public int WeekDaysID { get; set; }
    }
}
