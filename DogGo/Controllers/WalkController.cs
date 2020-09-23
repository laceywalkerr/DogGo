using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DogGo.Controllers
{
    public class WalkController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
