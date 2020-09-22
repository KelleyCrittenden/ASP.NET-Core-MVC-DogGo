using System;
using System.Collections.Generic;
using DogGo.Models;
using DogGo.Repositories;

using Microsoft.AspNetCore.Mvc;
using DogGo.Models.ViewModels;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace DogGo.Controllers
{
    public class OwnersController : Controller

    {
        //Adding other repositories so that View will have access to them 
        private readonly IOwnerRepository _ownerRepo;
        private readonly IDogRepository _dogRepo;
        private readonly IWalkerRepository _walkerRepo;
        private readonly INeighborhoodRepository _neighborhoodRepo;


        //CONSTRUCTOR
        // ASP.NET will give us an instance of our Walker Repository. 
        //This is called "Dependency Injection"
        //CONSTRUCTOR : Same name as class, no return, special method
        //Runs when a new instance is created
        //Just a class
        //ASP.Net creates the new OwnersController for us, can't see it but it is happening
        //
        public OwnersController(
            IOwnerRepository ownerRepository,
            IDogRepository dogRepository,
            IWalkerRepository walkerRepository,
            INeighborhoodRepository neighborhoodRepository)
        {
            _ownerRepo = ownerRepository;
            _dogRepo = dogRepository;
            _walkerRepo = walkerRepository;
            _neighborhoodRepo = neighborhoodRepository;
        }

        //METHOD
        //Gets the owners from the Owner Table
        //Using the GetAllOwners method from the OwnerRepository
        //Converts it to a list
        //Passes it off to the View
        public IActionResult Index()
        {
            List<Owner> owners = _ownerRepo.GetAllOwners();
            return View(owners);
        }

        // GET: Owners/Details/5
        //int id comes from the URL
        public ActionResult Details(int id)
        {
            Owner owner = _ownerRepo.GetOwnerById(id);
            List<Dog> dogs = _dogRepo.GetDogsByOwnerId(owner.Id);
            //using neighbrohood id to grab all the walkers in that neighborhood
            List<Walker> walkers = _walkerRepo.GetWalkersInNeighborhood(owner.NeighborhoodId);

            ProfileViewModel vm = new ProfileViewModel()
            {
                Owner = owner,
                Dogs = dogs,
                Walkers = walkers
            };
            //vm is short for view model
            return View(vm);
        }


        //**********************************************************************//
        // GET REQUEST: OwnersRepository/Create
        //When the user clicks on the Create Button
        //Handing the user a blank form, a blank view for them to fill out
        //Grabbing the GetAllNeighbhorhoods so that this can be passed to the view and used in the dropdown menu
        public ActionResult Create()
        {
            List<Neighborhood> neighborhoods = _neighborhoodRepo.GetAllNeighborhoods();

            OwnerFormViewModel vm = new OwnerFormViewModel()
            {
                Owner = new Owner(),
                Neighborhoods = neighborhoods
            };

            return View(vm);
        }

        // POST: OwnersRepository/Create
        // POST: Owners/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Owner owner)
        {
            try
            {
                _ownerRepo.AddOwner(owner);

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(owner);
            }
        }


        //************************************************************************//
        // GET: Owners/Edit/5
        //Populate Form presented to user
        public ActionResult Edit(int id)
        {
            
            List<Neighborhood> neighborhoods = _neighborhoodRepo.GetAllNeighborhoods();

            OwnerFormViewModel vm = new OwnerFormViewModel()
            {
                Owner = _ownerRepo.GetOwnerById(id),
                Neighborhoods = neighborhoods
            };

            if (vm.Owner == null)
            {
                return NotFound();
            }

            return View(vm);
        }

        // POST: Owners/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Owner owner)
        {
            try
            {
                _ownerRepo.UpdateOwner(owner);

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(owner);
            }
        }

        //***********************************************************************//
        // GET REQUEST: OwnersRepository/Delete/5
        //When the user clicks on the Delete Button
        //Presenting User with populated form confirming they want to delete user
        public ActionResult Delete(int id)
        {
            Owner owner = _ownerRepo.GetOwnerById(id);

            return View(owner);
        }


        // POST: Owners/Delete/5
        // What happens when the user clicks the delete button
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Owner owner)
        {
            try
            {
                _ownerRepo.DeleteOwner(id);

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(owner);
            }
        }
        //*******************************************************************//

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel viewModel)
        {
            //looking up owners to see if email exist, otherwise return unathorized
            Owner owner = _ownerRepo.GetOwnerByEmail(viewModel.Email);

            if (owner == null)
            {
                //returns 401 to user if they are not found in the database
                return Unauthorized();
            }

           // If we did find a user run this
           // Claims are just properties of the user, things we want to remember
           // User is claiming to be.... claiming to have this email
           // Data we want to store about the user
           // Will always some form of identifier(owner.Id)
           // Expects claim to be in list

            var claims = new List<Claim>
            {
                //storing everything as a string
                new Claim(ClaimTypes.NameIdentifier, owner.Id.ToString()),
                new Claim(ClaimTypes.Email, owner.Email),
                new Claim(ClaimTypes.Role, "DogOwner"),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Cookies keep track of users
            // Will eventually be saved in the users browser
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            //returning Index of dogs that belong to user

            return RedirectToAction("Index", "Dogs");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
