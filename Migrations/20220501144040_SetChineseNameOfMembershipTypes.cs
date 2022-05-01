using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelBookingSystem.Migrations
{
    public partial class SetChineseNameOfMembershipTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE MembershipType SET Name = N'白银会员' WHERE Id = 1");
            migrationBuilder.Sql("UPDATE MembershipType SET Name = N'黄金会员' WHERE Id = 2");
            migrationBuilder.Sql("UPDATE MembershipType SET Name = N'铂金会员' WHERE Id = 3");
            migrationBuilder.Sql("UPDATE MembershipType SET Name = N'钻石会员' WHERE Id = 4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
