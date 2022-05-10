using System.Collections.Generic;
using HotelBookingSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelBookingSystem.ViewModels
{
    public class CustomerListViewModel
    {
        public List<Customer> Customers { get; set; }

        public SelectList MembershipTypes { get; set; }

        public string MembershipType { get; set; }

        public int IsSubscribedToNewsLetter { get; set; }
    }
}
