using System;
using System.Collections.Generic;
using System.Linq;
using DogGo.Models;
using DogGo.Repositories;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace DogGo.Controllers
{
    public class DogsController : Controller

    {

        private readonly IDogRepository _dogRepo;

        // ASP.NET will give us an instance of our Dog Repository. 
        //This is called "Dependency Injection"
        public DogsController(IDogRepository dogRepository)
        {
            _dogRepo = dogRepository;
        }
        //Gets the dogs from the Dog Table
        //Using the GetAllDogs method from the DogRepository
        //Converts it to a list
        //Passes it off to the View
        //Added GetCurrentUser HELPER FUNCTION so the index only displays users dogs
        //Added Authorize so that only the dogs that belong to the user are displayed 

        [Authorize]
        public ActionResult Index()
        {
            int ownerId = GetCurrentUserId();

            List<Dog> dogs = _dogRepo.GetDogsByOwnerId(ownerId);

            return View(dogs);
        }

        // GET: OwnersRepository/Details/5
        public ActionResult Details(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);

            if (dog == null)
            {
                return NotFound();
            }

            return View(dog);
        }

        // GET REQUEST: DogsRepository/Create
        //When the user clicks on the Create Button
        //Handing the user a blank form, a blank view for them to fill out
        public ActionResult Create()
        {
            return View();
        }

        // POST: DogsRepository/Create
        // POST: Dogs/Create
        //Allowing a user to creat a dog with their ownerId only
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Dog dog)
        {
            try
            {
                // update the dogs OwnerId to the current user's Id 
                dog.OwnerId = GetCurrentUserId();

                _dogRepo.AddDog(dog);

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(dog);
            }
        }

        // GET: Dogs/Edit/5
        //Populate Form presented to user
        [Authorize]
        public ActionResult Edit(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);
            int ownerId = GetCurrentUserId();

            if (dog == null || dog.OwnerId != ownerId)
            {
                return NotFound();
            }

            return View(dog);
        }

        // POST: DogsRepository/Edit
        // POST: Dogs/Edit
        //Allowing a user to edit a dog with their ownerId only
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Dog dog)
        {
            try
            {
                // update the dogs OwnerId to the current user's Id 
                dog.OwnerId = GetCurrentUserId();

                _dogRepo.UpdateDog(dog);

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(dog);
            }
        }


        //******************************************************************************//
        // GET REQUEST: OwnersRepository/Delete/5
        //When the user clicks on the Delete Button
        //Presenting User with populated form confirming they want to delete user

        [Authorize]
        public ActionResult Delete(int id)
        {
            int ownerId = GetCurrentUserId();

            Dog dog = _dogRepo.GetDogById(id);
            if(dog.OwnerId != ownerId)
            {
                return NotFound();
            }

            return View(dog);
        }

        // POST: Dog/Delete/5
        //If User clicks the delete button it deletes
        //If User clicks on back to list, it goes back to the Index View
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Dog dog)
        {
            try
            {
                // update the dogs OwnerId to the current user's Id 
                dog.OwnerId = GetCurrentUserId();

                _dogRepo.DeleteDog(id);

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(dog);
            }
        }


        // Getting the Id of the current logged in user and only displaying dogs that belong to them
        // HELPER FUNCTION -- getting the ID of the current user is something that will need to be done many   times
        private int GetCurrentUserId()
        {
            // Getting access to one of the claims from controller(name identifier) that we created
            // Access to the users Id
            // Must parse bc saved as string in claims
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
