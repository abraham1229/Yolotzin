using System.ComponentModel.DataAnnotations;

namespace OrtizAbrahamSprint3.Data.Entities
{
    public class AgeRange
    {
        [Key]
        public int AgeRangeID { get; set; }
        public int MinimumAge { get; set; }
        public int MaximumAge { get; set; }
        public string RangeName { get; set; }
        public decimal Price { get; set; }
    }
}
