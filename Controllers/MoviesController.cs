﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelBookingSystem.Data;
using HotelBookingSystem.Models;
using HotelBookingSystem.ViewModels;

namespace HotelBookingSystem.Controllers
{
    public class MoviesController : Controller
    {
        private readonly HotelBookingSystemContext _context;

        public MoviesController(HotelBookingSystemContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            // return View(await _context.Movie.ToListAsync());
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


        // // GET: Movies/Details/5
        // public async Task<IActionResult> Details(int? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     var movie = await _context.Movie
        //         .FirstOrDefaultAsync(m => m.Id == id);
        //     if (movie == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     return View(movie);
        // }
        //
        // // GET: Movies/Create
        // public IActionResult Create()
        // {
        //     return View();
        // }
        //
        // // POST: Movies/Create
        // // To protect from overposting attacks, enable the specific properties you want to bind to.
        // // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Create([Bind("Id,Name")] Movie movie)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         _context.Add(movie);
        //         await _context.SaveChangesAsync();
        //         return RedirectToAction(nameof(Index));
        //     }
        //     return View(movie);
        // }
        //
        // // GET: Movies/Edit/5
        // public async Task<IActionResult> Edit(int? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     var movie = await _context.Movie.FindAsync(id);
        //     if (movie == null)
        //     {
        //         return NotFound();
        //     }
        //     return View(movie);
        // }
        //
        // // POST: Movies/Edit/5
        // // To protect from overposting attacks, enable the specific properties you want to bind to.
        // // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Movie movie)
        // {
        //     if (id != movie.Id)
        //     {
        //         return NotFound();
        //     }
        //
        //     if (ModelState.IsValid)
        //     {
        //         try
        //         {
        //             _context.Update(movie);
        //             await _context.SaveChangesAsync();
        //         }
        //         catch (DbUpdateConcurrencyException)
        //         {
        //             if (!MovieExists(movie.Id))
        //             {
        //                 return NotFound();
        //             }
        //             else
        //             {
        //                 throw;
        //             }
        //         }
        //         return RedirectToAction(nameof(Index));
        //     }
        //     return View(movie);
        // }
        //
        // // GET: Movies/Delete/5
        // public async Task<IActionResult> Delete(int? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     var movie = await _context.Movie
        //         .FirstOrDefaultAsync(m => m.Id == id);
        //     if (movie == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     return View(movie);
        // }
        //
        // // POST: Movies/Delete/5
        // [HttpPost, ActionName("Delete")]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> DeleteConfirmed(int id)
        // {
        //     var movie = await _context.Movie.FindAsync(id);
        //     _context.Movie.Remove(movie);
        //     await _context.SaveChangesAsync();
        //     return RedirectToAction(nameof(Index));
        // }
        //
        // private bool MovieExists(int id)
        // {
        //     return _context.Movie.Any(e => e.Id == id);
        // }
    }
}
