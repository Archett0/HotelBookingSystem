using System;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystem.Models
{
    public class Movie
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        public RoomType RoomType { get; set; }
        public byte RoomTypeId { get; set; }

        public DateTime DateCheckIn { get; set; }   // 入住日期
        public DateTime DateCheckOut { get; set; }  // 退房日期

        public byte NumberInStock { get; set; }
    }
}
