using System.Collections.Generic;
using HotelBookingSystem.Models;

namespace HotelBookingSystem.ViewModels
{
    public class MovieFormViewModel
    {
        public IEnumerable<RoomType> RoomTypes { get; set; }
        public Movie Movie { get; set; }

        public string Title => Movie != null ? "修改电影信息" : "新增电影";
    }
}
