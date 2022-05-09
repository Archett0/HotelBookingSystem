using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystem.Models
{
    public class ProjectRole
    {
        public int Id { get; set; }
     
        [Required]
        public string RoleName { get; set; }
    }
}
