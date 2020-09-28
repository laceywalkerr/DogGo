using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace DogGo.Controllers
{
    
    public class OwnersController : Controller
    {
        //the Owner Details view needs to know more than just the owner, this accesses the other repositories
        //this lists the repos that we are currently using to call our methods
        // these are private readonly fields
        private readonly IOwnerRepository _ownerRepo;
        private readonly IDogRepository _dogRepo;
        private readonly IWalkerRepository _walkerRepo;
        private readonly INeighborhoodRepository _neighborhoodRepo;

        //this gives us access to the repositories we need to use
        // IOwnerRepositry,IDogRepository,IWalkerRepository and INeighborhoodRepository are parameters
        //whenever the controllers are created, it gives the repositories from above. This is how it connects.
        public OwnersController(
            IOwnerRepository ownerRepository,
            IDogRepository dogRepository,
            IWalkerRepository walkerRepository,
            INeighborhoodRepository neighborhoodRepository)
        {
            //feilds generated , assigning property to OwnerRepository/DogRepository/WalkerRepository/NeighborhoodRepository
            _ownerRepo = ownerRepository;
            _dogRepo = dogRepository;
            _walkerRepo = walkerRepository;
            _neighborhoodRepo = neighborhoodRepository;
        }

        //An ActionResult is a return type of a controller method, also called an action method, and serves as the base class for *Result classes. 
        //Action methods return models to views, file streams, redirect to other controllers, or whatever is necessary for the task at hand.
        // This Public Action Result is creating the Index for the Owners, then listing them.
        //Fun fact: Index is the default method
        public ActionResult Index()
        {
            //method that lists all of the owners from the owners repository
            List<Owner> owners = _ownerRepo.GetAllOwners();
            return View(owners);
        }

        // GET: Owners/Details/5
        //int id = url, getting the particular owner that we select
        public ActionResult Details(int id)
        {
            //this calls the owner by their Id
            Owner owner = _ownerRepo.GetOwnerById(id);
            //this gets the dogs that the owner's ID is associated with
            List<Dog> dogs = _dogRepo.GetDogsByOwnerId(owner.Id);
            //this gets the walkers from that neighborhood the owners are in
            List<Walker> walkers = _walkerRepo.GetWalkersInNeighborhood(owner.NeighborhoodId);

            ProfileViewModel vm = new ProfileViewModel()
            {
                //properties to be used 
                Owner = owner,
                Dogs = dogs,
                Walkers = walkers
            };
            //passes the view model object from the controller
            return View(vm);

            //after this is set, check the details in views. Make sure it's taking in the view model type so it can read the information correctly
            // In this instance, @model DogGo.Models.Owner is changed to @model DogGo.Models.ViewModels.ProfileViewModel
        }

        // GET: Owners/Create

        //setting the create method for owner
        public ActionResult Create()
        {
            //this passes the list of neighborhoods on to the views/calling this method to get a list of all the neighborhoods
            List<Neighborhood> neighborhoods = _neighborhoodRepo.GetAll();

            //creating an instance of our viewmodel, giving it those neighborhoods to populate the dropdown
            OwnerFormViewModel vm = new OwnerFormViewModel()
            {
                // properties to be used 
                Owner = new Owner(),
                Neighborhoods = neighborhoods
            };
            //passes this to the views
            return View(vm);
        }

        // POST: Owners/Create
        [HttpPost]
        [ValidateAntiForgeryToken]

        //executing the method for the create method
        public ActionResult Create(Owner owner)
        {
            try
            {
                //calling the owner repository, creating the owner
                _ownerRepo.AddOwner(owner);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                //return to view owner
                return View(owner);
            }
        }




        // GET: Owners/Delete/5
        // setting the delete method for Owners using the Id
        public ActionResult Delete(int id)
        {
            // getting the information to delete
            Owner owner = _ownerRepo.GetOwnerById(id);

            //returning the info
            return View(owner);
        }

        // POST: Owners/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]

        //  executing delete method for Owners 
        public ActionResult Delete(int id, Owner owner)
        {
            try
            {
                //calling the owner by the id for deletion
                _ownerRepo.DeleteOwner(id);
                //returning to index
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            { //returning to the owner view
                return View(owner);
            }
        }

        // GET: Owners/Edit/5
        // setting the edit method for Owners using the Id
        public ActionResult Edit(int id)
        {
            //getting and listing all the neighborhoods
            List<Neighborhood> neighborhoods = _neighborhoodRepo.GetAll();

            //updating the new edited changes, sourced from OwnerFormViewModel
            OwnerFormViewModel vm = new OwnerFormViewModel()
            {
                //properties to be used
                Owner = new Owner(),
                Neighborhoods = neighborhoods
            };
            //passes the view model object from the controller
            return View(vm);
        }

        // POST: Owners/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        //  executing edit method for Owners 
        public ActionResult Edit(int id, Owner owner)
        {

            try
            {
                //calling the owner repository, updating the owner
                _ownerRepo.UpdateOwner(owner);

                //returning to the index
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                //passes the view model object of owner to the controller
                return View(owner);
            }
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel viewModel)
        {
            Owner owner = _ownerRepo.GetOwnerByEmail(viewModel.Email);

            if (owner == null)
            {
                return Unauthorized();
            }

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, owner.Id.ToString()),
        new Claim(ClaimTypes.Email, owner.Email),
        new Claim(ClaimTypes.Role, "DogOwner"),
    };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Dogs");
        }

    }
}
