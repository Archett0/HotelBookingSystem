using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystem.Models
{
    public class Hotel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "酒店名称")]
        public string Name { get; set; }

        [Display(Name = "酒店描述")]
        public string Description { get; set; }

        [Display(Name = "酒店位置")]
        public string Location { get; set; }

        [Display(Name = "酒店电话")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "酒店当前折扣")]
        public double Discount { get; set; }
    }
}
