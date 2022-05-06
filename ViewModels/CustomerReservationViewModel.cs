using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HotelBookingSystem.Models;

namespace HotelBookingSystem.ViewModels
{
    public class CustomerReservationViewModel
    {
        [Display(Name = "房间类型")]
        public IEnumerable<RoomType> RoomTypes { get; set; }   // room types from the DbContext

        [Required]
        [Display(Name = "入住时间")]
        public DateTime CheckInTime { get; set; }   // time that the user check in

        [Required]
        [Display(Name = "退房时间")]
        [DateRangeValidation]
        public DateTime CheckOutTime { get; set; } // time that the user check out

        [Display(Name = "房间类型")]
        public string RoomType { get; set; }    // 搜索词

        [Display(Name = "用户Id")]
        public string UserId { get; set; }

        [Display(Name = "搜索出的所有房间")]
        public IEnumerable<Room> ResultRooms { get; set; }

        [Display(Name = "选中的一个房间")]
        public Room ThisRoom { get; set; }

        [Display(Name = "当前用户")]
        public Customer Customer { get; set; }

        [Display(Name = "折算出的总价")]
        public double TotalPrice { get; set; }
    }
}
