﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FindKočka.Models
{
    public class Owner
    {
        public int Id { get; set; }
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public ICollection<Cat> CatsId { get; set; }

    }
}