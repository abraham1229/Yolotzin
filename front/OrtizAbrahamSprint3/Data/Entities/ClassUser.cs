using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace OrtizAbrahamSprint3.Data.Entities
{
    public class ClassUser
    {
        [Key]
        public int ClassUserID { get; set; }
        public DateOnly EnrollmentDate { get; set; }
        public int ClassID { get; set; }
        public int UserID { get; set; }
    }
}
