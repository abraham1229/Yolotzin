using System.ComponentModel.DataAnnotations;

namespace OrtizAbrahamSprint3.Data.Entities
{
    public class Classes
    {
        [Key]
        public int ClassID { get; set; }
        public decimal Price { get; set; }
        public string ClassHourStart { get; set; }
        public string ClassHourFinish { get; set; }
        public int AgeRangeID { get; set; }
        public int LevelID { get; set; }
        public int StyleID { get; set; }
        public int WeekDaysID { get; set; }
    }
}
