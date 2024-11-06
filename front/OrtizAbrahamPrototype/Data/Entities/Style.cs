using System.ComponentModel.DataAnnotations;

namespace OrtizAbrahamSprint3.Data.Entities
{
    public class Style
    {
        [Key]
        public int StyleID { get; set; }
        public string StyleName { get; set; }
    }
}
