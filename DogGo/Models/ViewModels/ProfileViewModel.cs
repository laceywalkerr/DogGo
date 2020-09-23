using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DogGo.Models.ViewModels
{
    public class ProfileViewModel
    {
        //properties for each portion of the view that we need
        public Owner Owner { get; set; }
        public List<Walker> Walkers { get; set; }
        public List<Dog> Dogs { get; set; }
    }
}