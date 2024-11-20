using System.ComponentModel.DataAnnotations;

//To store all the possible styles

namespace OrtizAbrahamSprint3.Data.Entities
{
    public class Style
    {
        [Key]
        public int StyleID { get; set; }
        public string StyleName { get; set; }
    }
}
