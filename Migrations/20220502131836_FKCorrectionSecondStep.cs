using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelBookingSystem.Migrations
{
    public partial class FKCorrectionSecondStep : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TABLE Room");
            migrationBuilder.Sql("DROP TABLE RoomType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
