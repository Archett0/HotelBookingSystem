using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelBookingSystem.Migrations
{
    public partial class FKCorrectionFirstStep : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TABLE Movie");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
