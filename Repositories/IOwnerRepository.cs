using DogGo.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public interface IOwnerRepository
    {
        List<Owner> GetAllOwners();
        Owner GetOwnerById(int id);

        //List<Owner> GetOwnerByIdWithDogs();

        //Method void because we are just creating this method now, not returning anything
        //Creating to be used later
       void AddOwner(Owner owner);
       void UpdateOwner(Owner owner);
       void DeleteOwner(int id);

    }
}
