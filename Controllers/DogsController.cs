using System;
using System.Collections.Generic;
using System.Linq;
using DogGo.Models;
using DogGo.Repositories;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Index()
        {
            List<Dog> dogs = _dogRepo.GetAllDogs();

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Dog dog)
        {
            try
            {
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
        public ActionResult Edit(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);

            if (dog == null)
            {
                return NotFound();
            }

            return View(dog);
        }

        // POST: Dogs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Dog dog)
        {
            try
            {
                _dogRepo.UpdateDog(dog);

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(dog);
            }
        }
        // GET REQUEST: OwnersRepository/Delete/5
        //When the user clicks on the Delete Button
        //Presenting User with populated form confirming they want to delete user
        public ActionResult Delete(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);

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
                _dogRepo.DeleteDog(id);

                return RedirectToAction("Index");
            }
            catch (Exception )
            {
                return View(dog);
            }
        }
    }
}
