using DogGo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Repositories
{
    public interface IOwnerRepository
    {
        void AddOwner(Owner owner);
        void DeleteOwner(int ownerId);
        Owner GetOwnerById(int id);
        void UpdateOwner(Owner owner);
        List<Owner> GetAll();
        List<Owner> GetAllOwners();
    }
}
