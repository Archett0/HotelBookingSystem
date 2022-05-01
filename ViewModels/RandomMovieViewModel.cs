using System.Collections.Generic;
using HotelBookingSystem.Models;

namespace HotelBookingSystem.ViewModels
{
    public class RandomMovieViewModel
    {
        public Movie Movie { get; set; }
        public List<Customer> Customers { get; set; }
    }
}
