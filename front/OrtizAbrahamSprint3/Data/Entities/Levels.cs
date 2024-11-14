using System.ComponentModel.DataAnnotations;

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
