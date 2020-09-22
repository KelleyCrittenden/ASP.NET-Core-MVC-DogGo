using System.Collections.Generic;
using System.Security.Claims;
using DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DogGo.Controllers
{
    public class WalkersController : Controller
    {

        private readonly IWalkerRepository _walkerRepo;
        private readonly IWalkRepository _walkRepo;

        // ASP.NET will give us an instance of our Walker Repository. 
        //This is called "Dependency Injection"
        public WalkersController(
            IWalkerRepository walkerRepository,
            IWalkRepository walkRepository)
        {
            _walkerRepo = walkerRepository;
            _walkRepo = walkRepository;

        }
        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }

        //Gets the walkers from the Walker Table
        //Using the GetAllWalkers method from the WalkerRepository
        //Converts it to a list
        //Passes it off to the View
        public IActionResult Index()
        {
            try
            {
                int ownerId = GetCurrentUserId();
                List<Walker> walkers = _walkerRepo.GetWalkersInNeighborhood(ownerId);

                return View(walkers);
            }
            catch 
             { 
                List<Walker> allWalkers = _walkerRepo.GetAllWalkers();

                return View(allWalkers);
            }

        }
        // GET: Walkers/Details/5
        public ActionResult Details(int id)
        {
            Walker walker = _walkerRepo.GetWalkerById(id);
            List<Walk> walks = _walkRepo.GetWalksbyWalkerId(walker.Id);

            WalkerProfileViewModel vm = new WalkerProfileViewModel()
            {
                Walker = walker,
                Walks = walks
            };
            //vm is short for view model
            return View(vm);
        }


    }
}


