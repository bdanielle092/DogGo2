using DogGo2.Models;
using DogGo2.Models.ViewModels;
using DogGo2.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DogGo2.Controllers
{


    public class OwnersController : Controller
    {
        //this is dependency injection. It loosen coupling and makes it easier to test things. You know its dependency injection because it private interfaces followed by a constructor with a bunch of interfaces. Benefits is it reduces tight coupling and makes taking a lot easier.
        private readonly IOwnerRepository _ownerRepo;
        private readonly IDogRepository _dogRepo;
        private readonly IWalkerRepository _walkerRepo;
        private readonly INeighborhoodRepository _neighborhoodRepo;

        public OwnersController(IOwnerRepository ownerRepository, IDogRepository dogRepository, IWalkerRepository walkerRepository, INeighborhoodRepository neighborhoodRepository)
        {
            _ownerRepo = ownerRepository;
            _dogRepo = dogRepository;
            _walkerRepo = walkerRepository;
            _neighborhoodRepo = neighborhoodRepository;
        }

        // GET: OwnersController
        public ActionResult Index()
        {
            List<Owner> owners = _ownerRepo.GetAllOwners();
            return View(owners);
        }

        // GET: OwnersController/Details/5
        public ActionResult Details(int id)
        {
            //Bring in the owner and list of dogs and walkers
            Owner owner = _ownerRepo.GetOwnerById(id);
            List<Dog> dogs = _dogRepo.GetDogsByOwnerId(owner.Id);
            List<Walker> walkers = _walkerRepo.GetWalkersInNeighborhood(owner.NeighborhoodId);
           

            //creating an Oject that holds owner and the list of dogs and walkers
            ProfileViewModel vm = new ProfileViewModel
            {
                Owner = owner,
                Dogs = dogs,
                Walkers = walkers

            };
            //returning the view model
            return View(vm);
        }

        //the sever is handing the user a blank html form. The user is getting the form
        // GET: OwnersController/Create
        public ActionResult Create()
        {
            //bringing in the list of neighborhoods
            List<Neighborhood> neighborhoods = _neighborhoodRepo.GetAll();

            //creating an obect to hold owner and the list of neighborhoods
            OwnerFormViewModel vm = new OwnerFormViewModel()
            {
                Owner = new Owner(),
                Neighborhoods = neighborhoods,

            };
            //returning the view model
            return View(vm);
        }

        //The user is giving the serve the form back. The user is submitting the form 
        // POST: OwnersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OwnerFormViewModel viewModel)
        {
            //redirect user back to the index if the owner is added, passing in the viewModel for owner
            //otherwise it will stay on the create view and an error message will show
            try
            {
                _ownerRepo.AddOwner(viewModel.Owner);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                //error message if an exception is thrown
                viewModel.ErrorMessage = "Woop! Something wnet wrong while saving this owner";
                //getting the list of neighborhoods again 
                viewModel.Neighborhoods = _neighborhoodRepo.GetAll();
                //retruning the viewModel for owner
                return View(viewModel);
            }

        }


        // GET: OwnersController/Edit/5
        public ActionResult Edit(int id)
        {
            //getting the owner and a list of neighborhoods
            Owner owner = _ownerRepo.GetOwnerById(id);
            List<Neighborhood> neighborhoods = _neighborhoodRepo.GetAll();

            //creating an object to hold the owner and list of neighborhoods
            OwnerFormViewModel vm = new OwnerFormViewModel()
            {
                Neighborhoods = neighborhoods,

                Owner = owner

            };
            //return the view model
            return View(vm);
        }

        // POST: OwnersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, OwnerFormViewModel viewModel)
        {
            try
            {
                //if the owner is update the user will be redirect to the index 
                //otherwise the user will stay on the update page and an error message will show
                _ownerRepo.UpdateOwner(viewModel.Owner);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                //error message
                viewModel.ErrorMessage = "Woop! Something went wrong while saving this owner";
                //getting the list of Neighborhoods again 
                viewModel.Neighborhoods = _neighborhoodRepo.GetAll();
                //returning the view model
                return View(viewModel);
            }
        }

        // GET: OwnersController/Delete/5
        public ActionResult Delete(int id)
        {
            //the user can delete the owner by there Id
            Owner owner = _ownerRepo.GetOwnerById(id);
            return View(owner);
        }

        // POST: OwnersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Owner owner)
        {
            //if owner is deleted the user will be redirected to the index page
            //otherwise the user will stay on the delete page to fix the error
            try
            {
                _ownerRepo.DeleteOwner(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
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

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, owner.Id.ToString()),
                new Claim(ClaimTypes.Email, owner.Email),
                new Claim(ClaimTypes.Role, "DogOwner"),
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Dogs");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Owners");

        }
    }
}
