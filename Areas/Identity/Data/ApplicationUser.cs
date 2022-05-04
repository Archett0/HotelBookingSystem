using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using HotelBookingSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace HotelBookingSystem.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string FirstName { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string LastName { get; set; }

        [PersonalData]
        public bool IsAdmin { get; set; }    // 是否为管理员,0=customer,1=admin
        
        public bool IsSubscribedToNewsLetter { get; set; }  // 是否订阅推销邮件
        
        public byte MembershipTypeId { get; set; }  //会员类型Id

        public DateTime Birthday { get; set; } // 顾客的生日

    }
}
