using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo2.Models
{
    public class Owner
    {
        public int Id { get; set; }

        //We're using annotations to tell the framework that certain properties are required, have a min/max length, or need to be a valid email address or phone number.
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Hmmm... You shoud really add a Name....")]
        [MaxLength(35)]
        public string Name { get; set; }

        [Required]
        [StringLength(55, MinimumLength = 5)]
        public string Address { get; set; }

        [Required]
        [DisplayName("Neighborhood")]
        public int NeighborhoodId { get; set; }

        [Phone]
        [MaxLength(11)]
        [DisplayName("Phone Number")]
        public string Phone { get; set; }
        public Neighborhood Neighborhood { get; set; }
    }
}
