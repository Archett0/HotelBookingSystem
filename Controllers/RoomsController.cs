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

namespace HotelBookingSystem.Controllers
{
    [Authorize(Roles = "Admin, User")]
    public class RoomsController : Controller   // TODO:正修改此控制器来添加搜索服务
    {
        private readonly HotelBookingSystemContext _context;

        public RoomsController(HotelBookingSystemContext context)
        {
            _context = context;
        }

        // // GET: Rooms or Rooms?searchString=
        // public async Task<IActionResult> Index(string searchString) // 增加了根据名称查找的功能
        // {
        //     var hotelBookingSystemContext = _context.Room.Include(r => r.Hotel).Include(r => r.RoomType);
        //     var rooms = from room in hotelBookingSystemContext select room;
        //     if (!String.IsNullOrEmpty(searchString))
        //     {
        //         rooms = rooms.Where(s => s.Name.Contains(searchString));
        //     }
        //     return View(await rooms.ToListAsync());
        // }

        // GET: Rooms (Advanced methods)    // TODO: 注意这里筛选功能的写法！
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(string roomType, string roomHotel, string roomName)
        {
            var hotelBookingSystemContext = _context.Room
                .Include(r => r.Hotel)
                .Include(r => r.RoomType);
            
            // using LINQ
            IQueryable<string> hotelQuery = // TODO:设置查询语句
                from r in hotelBookingSystemContext orderby r.Hotel.Name select r.Hotel.Name;

            IQueryable<string> typeQuery = // TODO:设置查询语句
                from r in hotelBookingSystemContext orderby r.RoomType.Id select r.RoomType.Name;

            var rooms = from room in hotelBookingSystemContext select room; // TODO:获取全部房间

            if (!String.IsNullOrEmpty(roomName))
            {
                rooms = rooms.Where(s => s.Name.Contains(roomName));    // TODO:简单筛选房间名称
            }

            if (!String.IsNullOrEmpty(roomType))
            {
                rooms = rooms.Where(x => x.RoomType.Name == roomType);  // TODO:筛选房间类型
            }

            if (!String.IsNullOrEmpty(roomHotel))
            {
                rooms = rooms.Where(x => x.Hotel.Name == roomHotel);    // TODO:筛选所属酒店
            }

            var roomSearchViewModel = new RoomSearchViewModel
            {
                Hotels = new SelectList(await hotelQuery.Distinct().ToListAsync()), // TODO:设置下拉菜单
                RoomTypes = new SelectList(await typeQuery.Distinct().ToListAsync()),   // TODO:设置下拉菜单
                Rooms = await rooms.OrderBy(r => r.Hotel).ThenBy(r => r.RoomType).ThenBy(r => r.Name).ToListAsync()
            };

            return View(roomSearchViewModel);
        }

        [AllowAnonymous]
        [HttpGet]   // 这个方法用来给用户主页面提供数据
        public async Task<IActionResult> DirectToCustomerIndex()
        {
            var viewModel = new CustomerReservationViewModel
            {
                RoomTypes = await _context.RoomType.ToListAsync(),
                CheckInTime = DateTime.Today,
                CheckOutTime = DateTime.Today.AddDays(1)
            };

            return View("/Views/CustomerBusinesses/CustomerIndex.cshtml", viewModel);
        }

        [AllowAnonymous]
        [HttpGet]   // 这个方法用来根据用户输入查询房间
        public async Task<IActionResult> RoomSearch(string roomType, DateTime checkInTime, DateTime checkOutTime)
        {
            // Error return
            if (checkInTime.CompareTo(checkOutTime) > 0)
            {
                ViewData["ErrorMessage"] = "Error";
                var viewModel = new CustomerReservationViewModel
                {
                    RoomTypes = await _context.RoomType.ToListAsync(),
                    CheckInTime = DateTime.Today,
                    CheckOutTime = DateTime.Today.AddDays(1)
                };
                return View("/Views/CustomerBusinesses/CustomerIndex.cshtml", viewModel);
            }

            // Get all rooms
            var hotelBookingSystemContext = _context.Room
                .Include(r => r.Hotel)
                .Include(r => r.RoomType);
            IEnumerable<Room> allRooms = await hotelBookingSystemContext.OrderBy(r => r.RoomType).ThenBy(r => r.Hotel).ToListAsync();

            // Select the rooms which are available and satisfies the type
            var clearRooms = new List<Room>();
            clearRooms.AddRange(roomType == null
                ? allRooms.Where(room => room.Status == 0)
                : allRooms.Where(room => room.RoomType.Name.Equals(roomType) && room.Status == 0));

            // Get all reservations contains the upper cleared-rooms
            var allReservations = await _context.Reservation.ToListAsync();
            var clearReservations = new List<Reservation>();
            foreach (var reservation in allReservations)
            {
                if (clearRooms.Contains(reservation.Room) && reservation.Status != 3)   // 找出所有未完成订单
                {
                    clearReservations.Add(reservation);
                }
            }

            // Match with reservation records to find time-available rooms
            var resultRooms = new List<Room>();
            foreach (var currentRoom in clearRooms)
            {
                var roomAvailable = true;
                foreach (var reservation in clearReservations)
                {
                    if (reservation.RoomId != currentRoom.Id) continue;
                    if (checkInTime.CompareTo(reservation.DateCheckIn) >= 0 &&
                        checkInTime.CompareTo(reservation.DateCheckOut) <= 0)
                    {
                        roomAvailable = false;
                        break;
                    }
                    if (checkOutTime.CompareTo(reservation.DateCheckIn) >= 0 &&
                        checkOutTime.CompareTo(reservation.DateCheckOut) <= 0)
                    {
                        roomAvailable = false;
                        break;
                    }
                    if (checkInTime.CompareTo(reservation.DateCheckIn) <= 0 &&
                        checkOutTime.CompareTo(reservation.DateCheckOut) >= 0)
                    {
                        roomAvailable = false;
                        break;
                    }
                    if (checkInTime.CompareTo(reservation.DateCheckIn) >= 0 &&
                        checkOutTime.CompareTo(reservation.DateCheckOut) <= 0)
                    {
                        roomAvailable = false;
                        break;
                    }
                }
                if (!roomAvailable)
                {
                    Console.WriteLine("Room: {0} is not available so it is removed from the ready list", currentRoom.Name);
                }
                else
                {
                    resultRooms.Add(currentRoom);
                }
            }

            var resultSingleRooms = new List<Room>();
            if (roomType != null)
            {
                var hotel1Flag = false;
                var hotel2Flag = false;
                var hotel3Flag = false;
                foreach (var room in resultRooms)
                {
                    if (room.Hotel.Name.Equals("西湖分店"))
                    {
                        if (hotel1Flag == false)
                        {
                            resultSingleRooms.Add(room);
                            hotel1Flag = true;
                        }
                    }
                    else if (room.Hotel.Name.Equals("西溪湿地分店"))
                    {
                        if (hotel2Flag == false)
                        {
                            resultSingleRooms.Add(room);
                            hotel2Flag = true;
                        }
                    }
                    else if (room.Hotel.Name.Equals("钱江新城分店"))
                    {
                        if (hotel3Flag == false)
                        {
                            resultSingleRooms.Add(room);
                            hotel3Flag = true;
                        }
                    }
                }
            }
            else
            {
                resultSingleRooms = resultRooms;
            }


            // Now we get all the rooms available, we have to send it back to the customer
            var viewResultModel = new CustomerReservationViewModel
            {
                ResultRooms = resultSingleRooms,
                CheckInTime = checkInTime,
                CheckOutTime = checkOutTime
            };

            return View("/Views/CustomerBusinesses/CustomerRoomList.cshtml", viewResultModel);
        }

        // 真正去订房才需要登录
        public async Task<IActionResult> SelectedRoomDetail(int id, DateTime checkInTime, DateTime checkOutTime, string userEmail)
        {
            var room = await _context.Room
                .Include(r => r.RoomType)
                .Include(r => r.Hotel)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (userEmail == null)
                return NotFound();

            var currentUser = await _context.Customer
                .Include(c => c.MembershipType)
                .FirstOrDefaultAsync(c => c.Email == userEmail);

            if (currentUser == null)
                return NotFound();

            var originalPrice = room.RoomType.Price;
            var hotelDiscount = room.Hotel.Discount;
            var membershipDiscount = currentUser.MembershipType.DiscountRate / 100.0;
            var totalPrice = Math.Round((originalPrice - hotelDiscount) * (1 - membershipDiscount), 2);

            var viewModel = new CustomerReservationViewModel
            {
                ThisRoom = room,
                CheckInTime = checkInTime,
                CheckOutTime = checkOutTime,
                Customer = currentUser,
                TotalPrice = totalPrice
            };

            return View("/Views/CustomerBusinesses/CustomerRoomDetails.cshtml", viewModel);
        }

        // GET: Rooms/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Room
                .Include(r => r.Hotel)
                .Include(r => r.RoomType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // GET: Rooms/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["HotelId"] = new SelectList(_context.Hotel, "Id", "Name");
            ViewData["RoomTypeId"] = new SelectList(_context.RoomType, "Id", "Name");
            return View();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,Status,Description,RoomTypeId,HotelId")] Room room)
        {
            if (ModelState.IsValid)
            {
                _context.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HotelId"] = new SelectList(_context.Hotel, "Id", "Name", room.HotelId);
            ViewData["RoomTypeId"] = new SelectList(_context.RoomType, "Id", "Name", room.RoomTypeId);
            return View(room);
        }

        // GET: Rooms/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Room.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            ViewData["HotelId"] = new SelectList(_context.Hotel, "Id", "Name", room.HotelId);
            ViewData["RoomTypeId"] = new SelectList(_context.RoomType, "Id", "Name", room.RoomTypeId);
            return View(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Status,Description,RoomTypeId,HotelId")] Room room)
        {
            if (id != room.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["HotelId"] = new SelectList(_context.Hotel, "Id", "Name", room.HotelId);
            ViewData["RoomTypeId"] = new SelectList(_context.RoomType, "Id", "Name", room.RoomTypeId);
            return View(room);
        }

        // GET: Rooms/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Room
                .Include(r => r.Hotel)
                .Include(r => r.RoomType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _context.Room.FindAsync(id);
            _context.Room.Remove(room);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomExists(int id)
        {
            return _context.Room.Any(e => e.Id == id);
        }
    }
}
