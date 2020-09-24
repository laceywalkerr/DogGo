using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DogGo.Models;
using DogGo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DogGo.Controllers
{
    public class WalksController : Controller
    {
        private readonly IWalkRepository _walkRepo;
        public WalksController(IWalkRepository walkRepository)
        {
            _walkRepo = walkRepository;
        }


        // GET: WalksController
        public ActionResult Index()
        {
         /*   List<Walk> walks = _walkRepo.GetAllWalks();*/
            return View(/*walks*/);
        }


        public ActionResult Details(int id)
        {
            /*Walk walk = _walkRepo.GetWalkByDetails(id);*/

            return View(/*walk*/);
        }

    }
}