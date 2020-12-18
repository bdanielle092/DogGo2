using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo2.Models
{
    public class Walker
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a name")]
        [MaxLength(35)]
        public string Name { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        public int NeighborhoodId { get; set; }

        [DisplayName("")]
        public string ImageUrl { get; set; }
        public Neighborhood Neighborhood { get; set; }
      
    }
}
