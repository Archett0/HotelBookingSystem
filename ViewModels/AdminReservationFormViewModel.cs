using System.Collections.Generic;
using HotelBookingSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelBookingSystem.ViewModels
{
    public class AdminReservationFormViewModel
    {
        public List<Reservation> Reservations { get; set; }

        public SelectList RoomTypes { get; set; }

        public SelectList RoomHotels { get; set; }

        public SelectList ReservationStatuses { get; set; }

        public string RoomType { get; set; }
        public string RoomHotel { get; set; }

        public byte ReservationStatus { get; set; }
    }
}
