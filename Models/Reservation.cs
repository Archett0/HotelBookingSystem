using System;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystem.Models
{
    public class Reservation
    {
        public int Id { get; set; } // PK

        [Display(Name = "房间")]
        public Room Room { get; set; }  // 房间存放在这里

        [Required]
        [Display(Name = "房间")]
        public int RoomId { get; set; } // 房间Id

        [Display(Name = "顾客")]
        public Customer Customer { get; set; }  // 顾客存放在这里

        [Required]
        [Display(Name = "顾客")]
        public int CustomerId { get; set; } // 顾客Id

        [Required]
        [Display(Name = "入住日期")]
        public DateTime DateCheckIn { get; set; } // 入住日期

        [Required]
        [Display(Name = "退房日期")]
        public DateTime DateCheckOut { get; set; } // 退房日期

        [Display(Name = "预订描述")]
        public string Description { get; set; } // 本预订的描述

        [Required]
        [Display(Name = "预订状态")]
        public byte Status { get; set; }    // 本预订的状态

        [Required]
        [Display(Name = "订单金额")]
        public double TotalPrice { get; set; }  // 本预订顾客实际支付

    }
}
