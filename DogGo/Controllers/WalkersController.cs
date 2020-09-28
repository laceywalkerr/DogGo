﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DogGo.Controllers
{
    public class WalkersController : Controller 
    {

      
        private readonly IDogRepository _dogRepo;
        private readonly IWalkerRepository _walkerRepo;
        private readonly IWalkRepository _walkRepo;
        /*private readonly INeighborhoodRepository _neighborhoodRepo;*/


        public WalkersController(
           
            IDogRepository dogRepository,
            IWalkerRepository walkerRepository,
            IWalkRepository walkRepository/*,
            INeighborhoodRepository neighborhoodRepository*/)
        {
            
            _dogRepo = dogRepository;
            _walkerRepo = walkerRepository;
            _walkRepo = walkRepository;
            /*_neighborhoodRepo = neighborhoodRepository;*/
        }


        // GET: WalkersController
        public ActionResult Index()
        {
            List<Walker> walkers = _walkerRepo.GetAllWalkers();
            return View(walkers);
        }

        // GET: Owners/Details/5
        public ActionResult Details(int id)
        {
            Walker walker = _walkerRepo.GetWalkerById(id);
            List<Walk> walks = _walkRepo.GetWalksByWalkerId(walker.Id);
            List<Dog> dogs = _dogRepo.GetDogsByWalkerId(walker.Id);
            /*Neighborhood neighborhood = _neighborhoodRepo.GetNeighborhoodByWalkerId(walker.Id);*/

            WalkerProfileViewModel vm = new WalkerProfileViewModel()
            {
                Dogs = dogs,
                Walks = walks,
                Walker = walker,
                /*Neighborhood = neighborhood*/
            };

            return View(vm);
        }


        // GET: WalkersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WalkersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: WalkersController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WalkersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: WalkersController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WalkersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}