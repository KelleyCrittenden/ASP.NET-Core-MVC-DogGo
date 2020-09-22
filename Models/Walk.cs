using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace DogGo.Models
{
    public class Walk
    {
        public int Id { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public DateTime Date { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        [DisplayName("Walker Name")]
        public int WalkerId { get; set; }

        [Required]
        [DisplayName("Dog Walked")]
        public int DogId { get; set; }


        public Walker Walker { get; set; }
        public Dog Dog { get; set; }
        public Owner Owner { get; set; }
    }
}
