using DogGo2.Models;
using DogGo2.Models.ViewModels;
using DogGo2.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DogGo2.Controllers
{
    public class WalkersController : Controller
    {
        private readonly IWalkerRepository _walkerRepo;
        private readonly IWalkRepository _walkRepo;

        // ASP.NET will give us an instance of our Walker Repository. This is called "Dependency Injection"
        public WalkersController(IWalkerRepository walkerRepository, IWalkRepository walkRepository)
        {
            _walkerRepo = walkerRepository;
            _walkRepo = walkRepository;
        }
        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }

        //In the context of ASP.NET, each of the public methods in the controllers is considered an Action. When our application receives incoming HTTP requests, The ASP.NET framework is smart enough to know which controller Action to invoke.

        // GET: WalkersConstrollers
        //This code will get all the walkers in the Walker table, convert it to a List and pass it off to the view.
        public ActionResult Index(int id)
        {
           //if they are log in then only let them see the walkers that are in their neighborhood
           //otherwise let them see all the walkers
            try
            {
                int ownerId = GetCurrentUserId();
                List<Walker> walkers = _walkerRepo.GetWalkersInNeighborhood(ownerId);

                return View(walkers);
            }
            catch
            {
                List<Walker> walkers = _walkerRepo.GetAllWalkers();
                return View(walkers);
            }
        }

        //Notice that this method accepts an id parameter. When the ASP.NET framework invokes this method for us, it will take whatever value is in the url and pass it to the Details method. For example, if the url is walkers/details/2, the framework will invoke the Details method and pass in the value 2. The code looks in the database for a walker with the id of 2. If it finds one, it will return it to the view. If it doesn't the user will be given a 404 Not Found page.
        // GET: WalkersConstrollers/Details/5
        public ActionResult Details(int id)
        {
            Walker walker = _walkerRepo.GetWalkerById(id);
            List<Walk> walks = _walkRepo.GetWalksByWalkerId(walker.Id);

            WalkerProfileViewModel vm = new WalkerProfileViewModel()
            {
                Walker = walker,
                Walks = walks
            };

            return View(vm);
        }

        // GET: WalkersConstrollers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WalkersConstrollers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Walker walker)
        {
            try
            {
                _walkerRepo.AddWalker(walker);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View(walker);
            }
        }

        // GET: WalkersConstrollers/Edit/5
        public ActionResult Edit(int id)
        {
            Walker walker = _walkerRepo.GetWalkerById(id);
            if(walker == null)
            {
                return NotFound();
            }
            return View(walker);
        }

        // POST: WalkersConstrollers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Walker walker)
        {
            try
            {
                _walkerRepo.UpdateWalker(walker); 
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View(walker);
            }
        }

        // GET: WalkersConstrollers/Delete/5
        public ActionResult Delete(int id)
        {
            Walker walker = _walkerRepo.GetWalkerById(id);
            return View(walker);
        }

        // POST: WalkersConstrollers/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Walker walker)
        {
            try
            {
                _walkerRepo.DeleteWalker(id);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View(walker);
            }
        }
    }
}
