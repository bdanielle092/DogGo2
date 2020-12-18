using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo2.Models
{
    public class Dog
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a name")]
        [MaxLength(35)]
        public string Name {get; set;}

        [Required]

        public int OwnerId { get; set; }

        [Required]
        [DisplayName("Owner's Name")]
        public Owner Owner { get; set; }

        [Required]
        public string Breed { get; set; }
        public string Notes { get; set; }
        public string ImageUrl { get; set; }
    
    }
}
