using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Models
{
    public class Dog
    {
        public int Id { get; set; }


        [Required]
        [StringLength(55)]
        [DisplayName("Dog Name")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Owner Name")]
        public int OwnerId { get; set; }

        [Required]
        [StringLength(55)]
        public string Breed { get; set; }

        [StringLength(255)]
        public string Notes { get; set; }

        [DisplayName("Picture")]
        public string ImageUrl { get; set; }


        public Owner Owner { get; set; }
    }
}
