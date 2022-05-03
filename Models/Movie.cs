using System;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystem.Models
{
    public class Movie
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(255)]
        [Display(Name = "电影名称")]
        public string Name { get; set; }

        public RoomType RoomType { get; set; }

        [Display(Name = "房间类型")]
        [Required]
        public byte RoomTypeId { get; set; }

        [Display(Name = "入住日期")]
        [Required]
        public DateTime DateCheckIn { get; set; }   // 入住日期

        [Display(Name = "退房日期")]
        [Required]
        public DateTime DateCheckOut { get; set; }  // 退房日期

        [Display(Name = "剩余数量")]
        [Range(0, 200)]
        public byte NumberInStock { get; set; }
    }
}
