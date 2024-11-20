using System.ComponentModel.DataAnnotations;

//To store all the users and possible guardians

namespace OrtizAbrahamSprint3.Data.Entities
{
    public class Users
    {
        [Key]
        public int UserID { get; set; }
        public string FirstNameUser { get; set; }
        public string LastNameUser { get; set; }
        public string EmailAddressUser { get; set; }
        public string PhoneNumberUser { get; set; }
        public DateOnly BirthdayUser { get; set; }
        public string FirstNameGuardian { get; set; }
        public string LastNameGuardian { get; set; }
        public string EmailAddressGuardian { get; set; }
        public string PhoneNumberGuardian { get; set; }
        public DateOnly BirthdayGuardian { get; set; }
        public string Username { get; set; }
        public byte[] UserPasswordHash { get; set; }
        public byte[] UserPasswordSalt { get; set; }
        public DateOnly UserCreationDate { get; set; }
    }
}
