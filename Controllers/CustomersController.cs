using System.Collections.Generic;
using System.Linq;
using HotelBookingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace HotelBookingSystem.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public CustomersController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Get: Customers
        // [Route("/customers")]
        public IActionResult Index()
        {
            var customers = GetCustomers();
            return View(customers);
        }


        // Get: Customer
        // [Route("/customer")]
        public IActionResult Details(int id)
        {
            var customer = GetCustomers().SingleOrDefault(c => c.Id == id);
            if (customer == null)
            {
                return Content("Details方法出错");
            }
            return View(customer);
        }

        public IEnumerable<Customer> GetCustomers()
        {
            return new List<Customer>
            {
                new Customer { Id = 1, Name = "成龙" },
                new Customer { Id = 2, Name = "李小年" }
            };
        }
    }
}
