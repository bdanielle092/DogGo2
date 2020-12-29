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
    public class WalksController : Controller
    {
        private readonly IWalkRepository _walkRepo;
        private readonly IDogRepository _dogRepo;
        private readonly IWalkerRepository _walkerRepo;
    
    
    

        public WalksController(IWalkRepository walkRepository, IDogRepository dogRepository, IWalkerRepository walkerRepository)
        {
            _walkRepo = walkRepository;
            _dogRepo = dogRepository;
            _walkerRepo = walkerRepository;
           
        }

        // GET: Walks
        public ActionResult Index()
        {
            List<Walk> walks = _walkRepo.GetAllWalks();
            return View(walks);
        }

        // GET: Walks/Details/5
        public ActionResult Details(int id)
        {
            Walk walk = _walkRepo.GetWalkById(id);
            if(walk == null)
            {
                return NotFound();
            }
            return View(walk);
        }

        // GET: Walks/Create
        public ActionResult Create(int id)
        {
            
          
           
            Walker walker = _walkerRepo.GetWalkerById(id);
            int OwnerId = GetCurrentOwner();
            List<Dog> dogs = _dogRepo.GetDogsByOwnerId(OwnerId);


            ScheduleAWalkFormViewModel sawfvm = new ScheduleAWalkFormViewModel()
            {
                OwnerId = OwnerId,
                Dogs = dogs,
                Walker = walker,
                

            };
            return View(sawfvm);
        }

        // POST: Walks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ScheduleAWalkFormViewModel sawfvm, int id)
        {
           

            try
            {
                Walker walker = _walkerRepo.GetWalkerById(id);
                int ownerId = GetCurrentOwner();
                List<Dog> dogs = _dogRepo.GetDogsByOwnerId(ownerId);
                sawfvm.Walk.WalkerId = id;
                sawfvm.Walk.WalkStatusId = 1;
                _walkRepo.AddWalk(sawfvm.Walk);

                return RedirectToAction("Details", "Owners");
            }
            catch (Exception ex)
            {
                sawfvm.ErrorMessage = "Something went wrong, Please try again";


                return View(sawfvm);
            }
        }

        // GET: Walks/Edit/5
        public ActionResult Edit(int id)
        {
            Walk walk = _walkRepo.GetWalkById(id);
            if(walk == null)
            {
                return NotFound();
            }
            return View(walk);
        }

        // POST: Walks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Walk walk)
        {
            try
            {
                _walkRepo.UpdateWalk(walk);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View(walk);
            }
        }

        // GET: Walks/Delete/5
        public ActionResult Delete(int id)
        {
            Walk walk = _walkRepo.GetWalkById(id);
            return View(walk);
        }

        // POST: Walks/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Walk walk)
        {
            try
            {
                _walkRepo.DeleteWalk(id);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View(walk);
            }
        }


        private int GetCurrentOwner()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}


