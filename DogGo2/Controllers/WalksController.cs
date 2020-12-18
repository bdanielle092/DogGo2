using DogGo2.Models;
using DogGo2.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo2.Controllers
{
    public class WalksController : Controller
    {
        private readonly IWalkRepository _walkRepo;

        public WalksController(IWalkRepository walkRepository)
        {
            _walkRepo = walkRepository;
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: Walks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Walk walk)
        {
            try
            {
                _walkRepo.AddWalk(walk);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View(walk);
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
    }
}
