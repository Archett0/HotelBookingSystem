using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystem.Models
{
    public class RoomType
    {
        public int Id { get; set; } // PK
        
        [Required]
        [StringLength(255)]
        public string Name { get; set; }    // 房间类型
    }
}
