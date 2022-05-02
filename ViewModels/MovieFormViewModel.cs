using System.Collections.Generic;
using HotelBookingSystem.Models;

namespace HotelBookingSystem.ViewModels
{
    public class MovieFormViewModel
    {
        public IEnumerable<RoomType> RoomTypes { get; set; }
        public Movie Movie { get; set; }

        public string Title
        {
            get
            {
                if (Movie != null && Movie.Id != null)
                {
                    return "修改电影信息";
                }
                else
                {
                    return "新增电影";
                }
            }
        }
    }
}
