using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DogGo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DogGo.Controllers
{
    public class DogController : Controller
    {
        private readonly IDogRepository _dogRepository;
        public DogController(IDogRepository dogRepository)
        {
            _dogRepository = dogRepository;
        }
        public IActionResult Index()
        {
            List<dog>
            return View();
        }
    }
}
