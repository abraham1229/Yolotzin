using System.ComponentModel.DataAnnotations;

namespace OrtizAbrahamSprint3.Data.Entities
{
    public class Levels
    {
        [Key]
        public int LevelID { get; set; }
        public string LevelName { get; set; }
    }
}
