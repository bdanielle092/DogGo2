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
        private readonly IWalkRepository _walkRepos;

        public WalksController(IWalkRepository walkRepository)
        {
            _walkRepos = walkRepository;
        }

        // GET: Walks
        public ActionResult Index()
        {
            List<Walk> walks = _walkRepos.GetAllWalks();
            return View(walks);
        }

        // GET: Walks/Details/5
        public ActionResult Details(int id)
        {
            Walk walk = _walkRepos.GetWalkById(id);
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

        // GET: Walks/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Walks/Edit/5
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

        // GET: Walks/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Walks/Delete/5
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
