using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FindKočka.Models
{
    public class Cat
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int OwnerId { get; set; }
        public string Description { get; set; }
        public string ImageName { get; set; }
        
        [NotMapped]
        public IFormFile Image { get; set; }


    }
}
