using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace FindKočka.Models
{
    public class Cat
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter cat name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter cat age")]
        public int Age { get; set; }
        public int OwnerId { get; set; }
        public string OwnerName { get; set; }
        public string OwnerEmail { get; set; }
        public string OwnerNumber { get; set; }
        public string Description { get; set; }
        public string ImageName { get; set; }

        [Required(ErrorMessage = "Add photo with your cat")]
        [NotMapped]
        public IFormFile Image { get; set; }


    }
}
