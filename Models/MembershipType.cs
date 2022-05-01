namespace HotelBookingSystem.Models
{
    public class MembershipType
    {
        public byte Id { get; set; }    // 主键
        public short SignUpFee { get; set; }    // 会员注册费0-1000
        public byte DurationInMonths { get; set; }  // 会员有效期0-36
        public byte DiscountRate { get; set; }  // 折扣百分比0-100
        public string Name { get; set; }
    }
}
