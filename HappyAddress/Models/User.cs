using System;

namespace HappyAddress.Models
{
    public class User
    {
        public  int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime BirthDate { get; set; }

        public string Role { get; set; } = "User";
    }
}
