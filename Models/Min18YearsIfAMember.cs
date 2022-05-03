using System;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystem.Models
{
    public class Min18YearsIfAMember: ValidationAttribute   // 自定义验证第一步
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var customer = (Customer)validationContext.ObjectInstance;

            if (customer.MembershipTypeId == MembershipType.Unknown ||
                customer.MembershipTypeId == MembershipType.SilverMember)   // 用户未选择会员类型或选择不付费会员(白银会员)则不进行验证
            {
                return ValidationResult.Success;
            }

            if (customer.Birthday == null)  // 用户没有填写生日
            {
                return new ValidationResult("需要填写生日");
            }

            var age = DateTime.Today.Year - customer.Birthday.Value.Year;

            return (age >= 18)  // 超过18岁才可以办理付费会员
                ? ValidationResult.Success 
                : new ValidationResult("顾客未成年,不可办理会员");
        }
    }
}
