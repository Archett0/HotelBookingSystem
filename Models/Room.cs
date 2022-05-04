using System;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystem.Models
{
    public class Room
    {
        public int Id { get; set; } // PK

        [Required] 
        [StringLength(255)]
        [Display(Name = "房间号")]
        public string Name { get; set; }    // 房间号

        [Required]
        [Display(Name = "房间状态")]
        public byte Status { get; set; }    // 房间状态

        [Display(Name = "房间描述")]
        public string Description { get; set; } // 房间描述

        [Display(Name = "房间类型")]
        public RoomType RoomType { get; set; }  // 类型存放在这里
        
        [Required]
        [Display(Name = "房间类型")]
        public byte RoomTypeId { get; set; }    // 类型Id

        [Display(Name = "所属酒店")]
        public Hotel Hotel { get; set; }    // 所属酒店
        
        [Required]
        [Display(Name = "所属酒店")]
        public int HotelId { get; set; }    // 所属酒店Id
    }
}