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
            //method that lists all of the owners from the owners repository
            List<Walk> walks = _walkRepo.GetAllWalks();
            return View(walks);
        }

        // this is getting the walk details
        //int id = url, getting the particular owner that we select
        public ActionResult Details(int id)
        {
            //this calls the walk by the Id
            Walk walk = _walkRepo.GetWalkById(id);
            return View(walk);
        }

    }
}