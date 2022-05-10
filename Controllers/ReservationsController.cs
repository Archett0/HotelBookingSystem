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
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace HotelBookingSystem.Controllers
{
    [Authorize(Roles = "Admin, User")]
    public class ReservationsController : Controller
    {
        private readonly HotelBookingSystemContext _context;

        public ReservationsController(HotelBookingSystemContext context)
        {
            _context = context;
        }

        // // GET: Reservations
        // public async Task<IActionResult> Index()
        // {
        //     var hotelBookingSystemContext = _context.Reservation
        //         .Include(r => r.Room)
        //         .Include(r => r.Room.RoomType)
        //         .Include(r => r.Room.Hotel)
        //         .Include(r => r.Customer)
        //         .Include(r => r.Customer.MembershipType);
        //     
        //     return View(await hotelBookingSystemContext.ToListAsync());
        // }

        [Authorize(Roles = "Admin")]
        // GET: SchedulerCalendar
        public async Task<IActionResult> SchedulerCalendar()
        {
            var hotelBookingSystemContext = _context.Reservation
                .Include(r => r.Room)
                .Include(r => r.Room.RoomType)
                .Include(r => r.Room.Hotel)
                .Include(r => r.Customer)
                .Include(r => r.Customer.MembershipType);

            var reservations = from reservation in hotelBookingSystemContext select reservation;
            var reservationsList = await reservations.ToListAsync();
            var viewModel = new List<ReservationCalenderViewModel>();

            foreach (var reservation in reservationsList)
            {
                var backgroundColor = "";
                if (reservation.Room.Hotel.Name.Equals("西湖分店"))
                {
                    backgroundColor = "#1E90FF";
                }
                else if (reservation.Room.Hotel.Name.Equals("西溪湿地分店"))
                {
                    backgroundColor = "#00CD66";
                }
                else if (reservation.Room.Hotel.Name.Equals("钱江新城分店"))
                {
                    backgroundColor = "#FF6A6A";
                }

                viewModel.Add(new ReservationCalenderViewModel(
                    reservation.Id.ToString(),
                    reservation.Room.Name,
                    reservation.DateCheckIn.ToShortDateString(),
                    reservation.DateCheckOut.AddDays(1).ToShortDateString(),
                    "https://localhost:44308/Reservations/Details/"+reservation.Id,
                    backgroundColor));
            }

            var reservationListJson = JsonConvert.SerializeObject(viewModel);
            Console.WriteLine(reservationListJson);

            ViewData["Reservations"] = reservationListJson;
            return View();

        }

        [Authorize(Roles = "Admin")]
        // GET: Reservations
        public async Task<IActionResult> Index(string roomType, string roomHotel, byte reservationStatus)
        {
            var hotelBookingSystemContext = _context.Reservation
                .Include(r => r.Room)
                .Include(r => r.Room.RoomType)
                .Include(r => r.Room.Hotel)
                .Include(r => r.Customer)
                .Include(r => r.Customer.MembershipType);

            IQueryable<string> typeQueryable = 
                from r in hotelBookingSystemContext orderby r.Room.RoomType.Id select r.Room.RoomType.Name;

            IQueryable<string> hotelQueryable = 
                from r in hotelBookingSystemContext orderby r.Room.Hotel select r.Room.Hotel.Name;

            IQueryable<byte> statusQueryable =
                from r in hotelBookingSystemContext orderby r.Status select r.Status;

            var reservations = from reservation in hotelBookingSystemContext select reservation;

            if (!String.IsNullOrEmpty(roomType))
            {
                reservations = reservations.Where(r => r.Room.RoomType.Name == roomType);
            }

            if (!String.IsNullOrEmpty(roomHotel))
            {
                reservations = reservations.Where(r => r.Room.Hotel.Name == roomHotel);
            }

            if (!reservationStatus.Equals(0))
            {
                reservations = reservations.Where(r => r.Status == (reservationStatus - 1));
            }

            var viewModel = new AdminReservationFormViewModel
            {
                RoomTypes = new SelectList(await typeQueryable.Distinct().ToListAsync()),
                RoomHotels = new SelectList(await hotelQueryable.Distinct().ToListAsync()),
                ReservationStatuses = new SelectList(await statusQueryable.Distinct().ToListAsync()),
                Reservations = await reservations.OrderBy(r => r.Room).ThenBy(r => r.DateCheckIn)
                    .ThenBy(r => r.Customer).ToListAsync()
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                    .Include(r => r.Room)
                    .Include(r => r.Room.RoomType)
                    .Include(r => r.Room.Hotel)
                    .Include(r => r.Customer)
                    .Include(r => r.Customer.MembershipType)
                    .FirstOrDefaultAsync(m => m.Id == id);

            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> CustomerAllReservations(string userEmail)
        {
            var reservationsContext = _context.Reservation
                .Include(r => r.Room)
                .Include(r => r.Room.RoomType)
                .Include(r => r.Room.Hotel)
                .Include(r => r.Customer)
                .Include(r => r.Customer.MembershipType);

            var allReservations = from reservation in reservationsContext select reservation;

            allReservations = allReservations.Where(s => s.Customer.Email.Equals(userEmail));

            var userReservations = await allReservations.ToListAsync();

            return View("/Views/CustomerBusinesses/CustomerReservations.cshtml", userReservations);
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> ReservationsFilter(string userEmail, byte filterMethod)
        {
            var reservationsContext = _context.Reservation
                .Include(r => r.Room)
                .Include(r => r.Room.RoomType)
                .Include(r => r.Room.Hotel)
                .Include(r => r.Customer)
                .Include(r => r.Customer.MembershipType);

            var allReservations = from reservation in reservationsContext select reservation;

            IQueryable<byte> filterQueryable =
                from reservation in reservationsContext orderby reservation.Status select reservation.Status;

            allReservations = allReservations.Where(s => s.Customer.Email.Equals(userEmail));
            allReservations = allReservations.Where(s => s.Status == filterMethod);
            var userReservations = await allReservations.ToListAsync();

            return View("/Views/CustomerBusinesses/CustomerReservations.cshtml", userReservations);
        }

        [Authorize(Roles = "User")]
        // POST: Reservations/CustomerNewReservation 下订单模块
        public async Task<IActionResult> CustomerNewReservation(int roomId, DateTime checkInTime,
            DateTime checkOutTime, int userId, double totalPrice)
        {
            var newReservation = new Reservation
            {
                RoomId = roomId,
                CustomerId = userId,
                DateCheckIn = checkInTime,
                DateCheckOut = checkOutTime,
                TotalPrice = totalPrice
            };

            var targetUser = await _context.Customer.Where(c => c.Id == userId).FirstOrDefaultAsync();
            var targetRoom = await _context.Room.Where(c => c.Id == roomId).FirstOrDefaultAsync();
            var descriptionString = "顾客 " + targetUser.Name 
                                         + " 在 " + DateTime.Now 
                                         + " 预定了 " + targetRoom.Name
                                         + "，时段为 " + newReservation.DateCheckIn.ToShortDateString()
                                         + "-" + newReservation.DateCheckOut.ToShortDateString()
                                         + "，订单金额为 " + newReservation.TotalPrice + ";";
            Console.WriteLine(descriptionString);
            newReservation.Description = descriptionString;
            newReservation.Status = 0;

            _context.Reservation.Add(newReservation);
            await _context.SaveChangesAsync();

            // Now redirect to reservation list page
            return RedirectToAction(nameof(CustomerAllReservations),new { userEmail = targetUser.Email});
        }

        [Authorize(Roles = "Admin")]
        // GET: Reservations/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customer, "Id", "Name");
            ViewData["RoomId"] = new SelectList(_context.Room, "Id", "Name");
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,RoomId,CustomerId,DateCheckIn,DateCheckOut,Description,Status,TotalPrice")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "Id", "Name", reservation.CustomerId);
            ViewData["RoomId"] = new SelectList(_context.Room, "Id", "Name", reservation.RoomId);
            return View(reservation);
        }

        [Authorize(Roles = "User")]
        // GET: Reservations/Pay
        public async Task<IActionResult> CustomerPayReservation(int reservationId)
        {
            var targetReservation = await _context.Reservation
                .Where(r => r.Id.Equals(reservationId))
                .Include(r => r.Customer)
                .SingleOrDefaultAsync();

            if (targetReservation == null)
            {
                NotFound();
            }
            else
            {
                targetReservation.Status++;
                Console.WriteLine("Order{0} 's status has changed from {1} to {2}", targetReservation.Id,
                    targetReservation.Status - 1, targetReservation.Status);
                targetReservation.Description +=
                    Environment.NewLine +
                    "顾客 " + targetReservation.Customer.Name +
                    " 在" + DateTime.Now +
                    " 支付了订单;";
                var targetUser = targetReservation.Customer.Email;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(targetReservation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(CustomerAllReservations), new { userEmail = targetUser });
            }
            return NotFound();
        }

        [Authorize(Roles = "Admin")]
        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "Id", "Name", reservation.CustomerId);
            ViewData["RoomId"] = new SelectList(_context.Room, "Id", "Name", reservation.RoomId);

            Console.WriteLine("Executed edit method 1");
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RoomId,CustomerId,DateCheckIn,DateCheckOut,Description,Status,TotalPrice")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                Console.WriteLine("Executed edit method 2");
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "Id", "Name", reservation.CustomerId);
            ViewData["RoomId"] = new SelectList(_context.Room, "Id", "Name", reservation.RoomId);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .Include(r => r.Room)
                .Include(r => r.Room.RoomType)
                .Include(r => r.Room.Hotel)
                .Include(r => r.Customer)
                .Include(r => r.Customer.MembershipType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservation.FindAsync(id);
            _context.Reservation.Remove(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservation.Any(e => e.Id == id);
        }
    }
}
