﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DogGo.Models;


namespace DogGo.Repositories
{

    
        public interface IDogRepository
        {

        List<Dog> GetAllDogs();
        Dog GetDogById(int id);
        void AddDog(Dog newDog);
        void DeleteDog(int dogId);
        void UpdateDog(Dog dog);
        List<Dog> GetDogsByOwnerId(int ownerId);
    }
    
}

