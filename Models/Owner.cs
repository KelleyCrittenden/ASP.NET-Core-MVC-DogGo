using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

// Using Annotations to make fields required and validate fields, NOT ID
// Display Name Annotations change the way the field is named in forms that the User sees
namespace DogGo.Models
{
    public class Owner
    {
        public int Id { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please Enter A Name")]
        [MaxLength(35)]
        [DisplayName("Owner Name")]
        public string Name { get; set; }

        [Required]
        [StringLength(55, MinimumLength = 5)]
        public string Address { get; set; }


        [Required]
        [DisplayName("Neighborhood")]
        public int NeighborhoodId { get; set; }

        [Phone]
        [DisplayName("Phone Number")]
        public string Phone { get; set; }


        public Neighborhood Neighborhood { get; set; }
        public List<Dog> Dogs { get; set; }
    }
}
