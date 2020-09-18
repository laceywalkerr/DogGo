using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DogGo.Models
{
    public class Owner : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
