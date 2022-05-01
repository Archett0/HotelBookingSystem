using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using HotelBookingSystem.Models;
using HotelBookingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public MoviesController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Get: Movies
        public IActionResult Index(int? pageIndex, string sortBy)
        {
            if (!pageIndex.HasValue)
                pageIndex = 1;
            if (string.IsNullOrWhiteSpace(sortBy))
                sortBy = "Name";

            return Content(String.Format("pageIndex={0}&sortBy={1}", pageIndex, sortBy));
        }


        //Get: Movies/Random
        public IActionResult Random()
        {
            var movie = new Movie() { Name = "Shrek!" };
            var customers = new List<Customer>
            {
                new Customer { Name = "Customer 1" },
                new Customer { Name = "Customer 2" }
            };

            var viewModel = new RandomMovieViewModel
            {
                Movie = movie,
                Customers = customers
            };

            return View(viewModel); 

            // return Content("Hello World!");
            // return RedirectToAction("Index", "Home", new {page = 1, sortBy = "name"});
        }

        public IActionResult Edit(int id)
        {
            return Content("id=" + id);
        }

        
        // [Route("movies/released/{year}/{month:range(1,12)}")] // working and this is the priority
        public IActionResult ByReleaseDate(int year, int month)
        {
            return Content(year + "/" + month);
        }
    }
}
