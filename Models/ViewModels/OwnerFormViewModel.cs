//The Purpose of this File is to create a View Model that contains the Owners along with a list of their corresponding neighborhoods
//When Creating an Owner
//Allows user to use drop down menu as list rather than manually entering an ID

using System.Collections.Generic;

namespace DogGo.Models.ViewModels
{
    public class OwnerFormViewModel
    {
        public Owner Owner { get; set; }
        public List<Neighborhood> Neighborhoods { get; set; }
    }
}