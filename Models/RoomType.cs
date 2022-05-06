using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystem.Models
{
    public class RoomType
    {
        public byte Id { get; set; } // PK
        
        [Required]
        [StringLength(255)]
        public string Name { get; set; }    // 房间类型

        [Display(Name = "房间标准价格")]
        public double Price { get; set; }   // 房间价格

        [Display(Name = "房间图片")]
        public string PictureUrl { get; set; }  // 房间示例图片

    }
}
