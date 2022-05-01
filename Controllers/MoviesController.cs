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
        // private readonly ILogger<HomeController> _logger;
        // public MoviesController(ILogger<HomeController> logger)
        // {
        //     _logger = logger;
        // }

        // Get: Movies
        public IActionResult Index()
        {
            var movies = GetMovies();
            return View(movies);
        }

        private IEnumerable<Movie> GetMovies()
        {
            return new List<Movie>
            {
                new Movie { Id = 1, Name = "Interstellar" },
                new Movie { Id = 2, Name = "Inception" }
            };
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
        }
    }
}
