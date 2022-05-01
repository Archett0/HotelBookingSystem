using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelBookingSystem.Migrations
{
    public partial class PopulateMembershipTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO MembershipType(Id, SignUpFee, DurationInMonths, DiscountRate) VALUES (1, 0, 0, 0)");
            migrationBuilder.Sql("INSERT INTO MembershipType(Id, SignUpFee, DurationInMonths, DiscountRate) VALUES (2, 100, 6, 10)");
            migrationBuilder.Sql("INSERT INTO MembershipType(Id, SignUpFee, DurationInMonths, DiscountRate) VALUES (3, 300, 12, 20)");
            migrationBuilder.Sql("INSERT INTO MembershipType(Id, SignUpFee, DurationInMonths, DiscountRate) VALUES (4, 1000, 24, 30)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
