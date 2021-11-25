using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FindKočka.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Enter email or phone number")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
