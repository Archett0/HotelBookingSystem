using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HotelBookingSystem.Models;

namespace HotelBookingSystem.Data
{
    public class HotelBookingSystemContext : DbContext
    {
        public HotelBookingSystemContext(DbContextOptions<HotelBookingSystemContext> options)
            : base(options)
        {
        }

        public DbSet<HotelBookingSystem.Models.Movie> Movie { get; set; }

        public DbSet<HotelBookingSystem.Models.Customer> Customer { get; set; }

        public DbSet<HotelBookingSystem.Models.Room> Room { get; set; }

        public DbSet<HotelBookingSystem.Models.MembershipType> MembershipType { get; set; }
    }
}