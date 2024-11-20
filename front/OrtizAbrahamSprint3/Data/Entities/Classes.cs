using System.ComponentModel.DataAnnotations;

//To store the class the user has selected

namespace OrtizAbrahamSprint3.Data.Entities
{
    public class Classes
    {
        [Key]
        public int ClassID { get; set; }
        public int AgeRangeID { get; set; }
        public int LevelID { get; set; }
        public int StyleID { get; set; }
        public int UserID { get; set; }
    }
}
