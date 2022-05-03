using System;
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
        public IActionResult Index()    // 列表页
        {
            var movies = _context.Movie.Include(m => m.RoomType).ToList();
            return View(movies);
        }

        // GET: Movies/Details/5
        public IActionResult Details(int? id)   // 详情页
        {
            if (id == null)
            {
                return NotFound();
            }
            var movie = _context.Movie.Include(m => m.RoomType).SingleOrDefault(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }
        
            return View(movie);
        }

        // GET: Movies/New
        public IActionResult New()  // 新建页
        {
            var roomTypes = _context.RoomType.ToList();
            var viewModel = new MovieFormViewModel
            {
                Movie = new Movie(),
                RoomTypes = roomTypes
            };
            return View("MovieForm", viewModel);
        }

        // GET: Movies/Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Movie movie)  // 写入DB
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new MovieFormViewModel
                {
                    Movie = movie,
                    RoomTypes = _context.RoomType.ToList()
                };

                return View("MovieForm", viewModel);
            }
            if (movie.Id == 0)
            {
                _context.Movie.Add(movie);
            }
            else
            {
                var movieInDb = _context.Movie.Single(m => m.Id == movie.Id);
                movieInDb.Name = movie.Name;
                movieInDb.DateCheckIn = movie.DateCheckIn;
                movieInDb.DateCheckOut = movie.DateCheckOut;
                movieInDb.RoomTypeId = movie.RoomTypeId;
                movieInDb.NumberInStock = movie.NumberInStock;
            }

            _context.SaveChanges();

            return RedirectToAction("Index","Movies");
        }

        // GET: Movies/Edit
        public IActionResult Edit(int id)
        {
            var movie = _context.Movie.SingleOrDefault(m => m.Id == id);
            if (movie == null)
                return NotFound();

            var viewModel = new MovieFormViewModel
            {
                Movie = movie,
                RoomTypes = _context.RoomType.ToList()
            };

            return View("MovieForm", viewModel);
        }



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
