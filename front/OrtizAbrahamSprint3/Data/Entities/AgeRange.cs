using System.ComponentModel.DataAnnotations;

//To make sure the age range and price is clear

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
