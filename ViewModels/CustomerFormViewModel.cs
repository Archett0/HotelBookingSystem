using System.Collections.Generic;
using HotelBookingSystem.Models;

namespace HotelBookingSystem.ViewModels
{
    public class CustomerFormViewModel
    {
        public IEnumerable<MembershipType>  MembershipTypes { get; set; }
        public Customer Customer { get; set; }
    }
}
