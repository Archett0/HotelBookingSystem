using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using HotelBookingSystem.Data;
using HotelBookingSystem.Models;
using HotelBookingSystem.ViewModels;
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
        public async Task<IActionResult> Index()    // 列表页
        {
            // var customers = GetCustomers();
            var customers = _context.Customer.Include(c => c.MembershipType).ToList();  // 若不加ToList则在执行遍历时才去查询DB,加入Include是执行贪婪加载
            return View(customers);
            // return View(await _context.Customer.ToListAsync());
        }

        // Get: Customer
        public IActionResult Details(int id)    // 详情页
        {
            // var customer = GetCustomers().SingleOrDefault(c => c.Id == id);
            var customer = _context.Customer.Include(c => c.MembershipType).SingleOrDefault(c => c.Id == id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // GET: Customers/New
        public IActionResult New()  // 新建页，用来给表单提供数据
        {
            var membershipTypes = _context.MembershipType.ToList();
            var viewModel = new CustomerFormViewModel
            {
                MembershipTypes = membershipTypes
            };
            return View("CustomerForm", viewModel);
        }

        // GET: Customers/Save
        [HttpPost]
        public IActionResult Save(Customer customer) // 写入DB,使用Model binding
        {
            if (customer.Id == 0)   // 新顾客
            {
                _context.Customer.Add(customer);
            }
            else
            {
                var customerInDb = _context.Customer.Single(c => c.Id == customer.Id);  // 拿出数据库中的顾客信息
                // TryUpdateModelAsync(customerInDb); // Data Breach DO NOT USE
                customerInDb.Name = customer.Name;  // 手动设置props
                customerInDb.Birthday = customer.Birthday;
                customerInDb.MembershipTypeId = customer.MembershipTypeId;
                customerInDb.IsSubscribedToNewsLetter = customer.IsSubscribedToNewsLetter;
            }

            _context.SaveChanges(); // 根据所有数据的变化进行更改(在Transaction中进行)

            return RedirectToAction("Index","Customers");   // 返回给本Controller的IndexAction
        }

        public IActionResult Edit(int id)   // 修改页,给修改页提供数据
        {
            var customer = _context.Customer.SingleOrDefault(c => c.Id == id);
            if (customer == null)
                return NotFound();

            var viewModel = new CustomerFormViewModel
            {
                Customer = customer,
                MembershipTypes = _context.MembershipType.ToList()
            };
            return View("CustomerForm", viewModel);
        }


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
