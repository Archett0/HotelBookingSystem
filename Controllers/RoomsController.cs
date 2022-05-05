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
                Rooms = await rooms.ToListAsync()
            };

            return View(roomSearchViewModel);
        }

        [HttpPost]
        public string Index(string searchString, bool notUsed)
        {
            return "From [HttpPost]Index: filter on " + searchString;
        }

        // GET: Rooms/Details/5
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
