using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DogGo.Models.ViewModels
{
    public class WalkerProfileViewModel : Controller
    {

        public Walker Walker { get; set; }
        public List<Walker> Walkers { get; set; }
        public Dog Dog { get; set; }
        public List<Dog> Dogs { get; set; }

        public Walk Walk { get; set; }
        public List<Walk> Walks { get; set; }

        public Neighborhood Neighborhood { get; set; }
    }
}

