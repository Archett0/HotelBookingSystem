using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystem.Models
{
    public class Customer
    {
        public int Id { get; set; }
        [Required]  // Data Annotations
        [StringLength(255)]
        public string Name { get; set; }
        public bool IsSubscribedToNewsLetter { get; set; }  // 是否订阅推销邮件
        public MembershipType MembershipType { get; set; }  // 保存这个用户的会员类型,有时候不需要
        public byte MembershipTypeId { get; set; }  //FK
    }
}
