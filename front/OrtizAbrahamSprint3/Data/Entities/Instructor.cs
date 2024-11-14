using System.ComponentModel.DataAnnotations;

namespace OrtizAbrahamSprint3.Data.Entities
{
    public class Instructor
    {
        [Key]
        public int InstructorID { get; set; }
        public string FirstNameInstructor { get; set; }
        public string LastNameInstructor { get; set; }
        public string EmailAddressInstructor { get; set; }
        public string PhoneNumberInstructor { get; set; }
        public DateOnly BirthdayInstructor { get; set; }
        public int StyleID { get; set; }
    }
}
