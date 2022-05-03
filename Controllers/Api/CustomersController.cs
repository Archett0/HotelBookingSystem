using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using HotelBookingSystem.Data;
using HotelBookingSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly HotelBookingSystemContext _context;
        public CustomersController(HotelBookingSystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Customer> GetAllCustomers()
        {
            return _context.Customer.ToList();
        }

        [HttpGet(template: "{controller}/GetAll", Name = "GetAll")]
        public Customer GetCustomer(int? id)
        {
            var customer = _context.Customer.SingleOrDefault(x => x.Id == id);

            if (customer == null)
                throw new HttpRequestException();   // 和教程不同

            return customer;
        }

        [HttpPost]
        public Customer CreateCustomer(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpRequestException();   // 和教程不同
            }
            _context.Customer.Add(customer);
            _context.SaveChanges();

            return customer;
        }

        [HttpPut]
        public void UpdateCustomer(int id, Customer customer)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpRequestException();   // 和教程不同
            }

            var customerInDb = _context.Customer.SingleOrDefault(x => x.Id == id);
            if(customerInDb == null)
                throw new HttpRequestException();   // 和教程不同

            customerInDb.Name = customer.Name;
            customerInDb.Birthday = customer.Birthday;
            customerInDb.IsSubscribedToNewsLetter = customer.IsSubscribedToNewsLetter;
            customerInDb.MembershipTypeId = customer.MembershipTypeId;

            _context.SaveChanges();
        }

        [HttpDelete]
        public void DeleteCustomer(int id)
        {
            var customerInDb = _context.Customer.SingleOrDefault(x => x.Id == id);
            if (customerInDb == null)
                throw new HttpRequestException();   // 和教程不同

            _context.Customer.Remove(customerInDb);
            _context.SaveChanges();
        }
    }
}
