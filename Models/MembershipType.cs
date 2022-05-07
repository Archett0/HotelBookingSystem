using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystem.Models
{
    public class MembershipType
    {
        public byte Id { get; set; }    // 主键
        public short SignUpFee { get; set; }    // 会员注册费0-1000
        public byte DurationInMonths { get; set; }  // 会员有效期0-36

        [Display(Name = "享受的折扣百分比")]
        public byte DiscountRate { get; set; }  // 折扣百分比0-100

        [Display(Name = "会员类型")]
        public string Name { get; set; }

        public static readonly byte Unknown = 0;    // 用户没有选择会员类型,和下面的属性一样被用于表单验证
        public static readonly byte SilverMember = 1; // 不可以成为付费会员
    }
}
