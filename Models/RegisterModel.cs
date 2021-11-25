using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FindKočka.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage ="Enter Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Enter password")]
        [Compare("Password", ErrorMessage = "Password mismatch")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }

        [Required(ErrorMessage = "Enter phone number")]
        [Phone]
        public string Number { get; set; }
    }
}
