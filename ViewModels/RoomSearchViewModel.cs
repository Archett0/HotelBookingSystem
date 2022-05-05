using System.Collections.Generic;
using HotelBookingSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelBookingSystem.ViewModels
{
    public class RoomSearchViewModel
    {
        public List<Room> Rooms { get; set; }   // 存放要显示的Rooms

        public SelectList RoomTypes { get; set; }   // 这里并不是直接从Model中拿数据，而是DB搜索结果填充在这里

        public SelectList Hotels { get; set; }   // 这里并不是直接从Model中拿数据，而是DB搜索结果填充在这里

        public string RoomType { get; set; }    // 搜索词

        public string RoomHotel { get; set; }   // 搜索词

        public string RoomName { get; set; }    // 搜索词
    }
}
