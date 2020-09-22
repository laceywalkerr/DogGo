using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DogGo.Models;


namespace DogGo.Repositories
{
    public class IDogRepository : Controller
    {
        public IActionResult Index()
        {
            return View();

        }

        internal List<Dog> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}

