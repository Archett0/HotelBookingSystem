using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using HotelBookingSystem.Data;
using HotelBookingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Controllers
{
    public class CustomersController : Controller
    {
        private readonly HotelBookingSystemContext _context;

        public CustomersController(HotelBookingSystemContext context)
        {
            _context = context;
            // _context.Dispose();  // 要加这句话吗？加上会报错
        }

        // Get: Customers
        public async Task<IActionResult> Index()
        {
            // var customers = GetCustomers();
            var customers = _context.Customer.Include(c => c.MembershipType).ToList();  // 若不加ToList则在执行遍历时才去查询DB,加入Include是执行贪婪加载
            return View(customers);
            // return View(await _context.Customer.ToListAsync());
        }

        // Get: Customer
        public IActionResult Details(int id)
        {
            // var customer = GetCustomers().SingleOrDefault(c => c.Id == id);
            var customer = _context.Customer.SingleOrDefault(c => c.Id == id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // public IEnumerable<Customer> GetCustomers() // 使用_context.Customer后本方法被废弃
        // {
        //     return new List<Customer>
        //     {
        //         new Customer { Id = 1, Name = "成龙" },
        //         new Customer { Id = 2, Name = "李小年" }
        //     };
        // }


        // // GET: Customers/Details/5
        // public async Task<IActionResult> Details(int? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     var customer = await _context.Customer
        //         .FirstOrDefaultAsync(m => m.Id == id);
        //     if (customer == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     return View(customer);
        // }
        //
        // // GET: Customers/Create
        // public IActionResult Create()
        // {
        //     return View();
        // }
        //
        // // POST: Customers/Create
        // // To protect from overposting attacks, enable the specific properties you want to bind to.
        // // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Create([Bind("Id,Name")] Customer customer)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         _context.Add(customer);
        //         await _context.SaveChangesAsync();
        //         return RedirectToAction(nameof(Index));
        //     }
        //     return View(customer);
        // }
        //
        // // GET: Customers/Edit/5
        // public async Task<IActionResult> Edit(int? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     var customer = await _context.Customer.FindAsync(id);
        //     if (customer == null)
        //     {
        //         return NotFound();
        //     }
        //     return View(customer);
        // }
        //
        // // POST: Customers/Edit/5
        // // To protect from overposting attacks, enable the specific properties you want to bind to.
        // // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Customer customer)
        // {
        //     if (id != customer.Id)
        //     {
        //         return NotFound();
        //     }
        //
        //     if (ModelState.IsValid)
        //     {
        //         try
        //         {
        //             _context.Update(customer);
        //             await _context.SaveChangesAsync();
        //         }
        //         catch (DbUpdateConcurrencyException)
        //         {
        //             if (!CustomerExists(customer.Id))
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
        //     return View(customer);
        // }
        //
        // // GET: Customers/Delete/5
        // public async Task<IActionResult> Delete(int? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     var customer = await _context.Customer
        //         .FirstOrDefaultAsync(m => m.Id == id);
        //     if (customer == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     return View(customer);
        // }
        //
        // // POST: Customers/Delete/5
        // [HttpPost, ActionName("Delete")]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> DeleteConfirmed(int id)
        // {
        //     var customer = await _context.Customer.FindAsync(id);
        //     _context.Customer.Remove(customer);
        //     await _context.SaveChangesAsync();
        //     return RedirectToAction(nameof(Index));
        // }
        //
        // private bool CustomerExists(int id)
        // {
        //     return _context.Customer.Any(e => e.Id == id);
        // }
    }
}
