using System;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystem.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "请输入顾客姓名")]  // Data Annotations
        [StringLength(255)] // 验证第一步
        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Display(Name = "订阅推销邮件")]
        public bool IsSubscribedToNewsLetter { get; set; }  // 是否订阅推销邮件

        [Display(Name = "会员类型")]
        public MembershipType MembershipType { get; set; }  // 保存这个用户的会员类型,有时候不需要

        [Display(Name = "会员类型")]
        public byte MembershipTypeId { get; set; }  //FK
        
        [Display(Name = "生日")]
        [Min18YearsIfAMember]   // 自定义验证第二步
        public DateTime? Birthday { get; set; } // 顾客的生日,可以不填
    }
}
