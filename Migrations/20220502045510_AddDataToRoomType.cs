using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelBookingSystem.Migrations
{
    public partial class AddDataToRoomType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO RoomType (Name) VALUES (N'单人间')");
            migrationBuilder.Sql("INSERT INTO RoomType (Name) VALUES (N'大床房')");
            migrationBuilder.Sql("INSERT INTO RoomType (Name) VALUES (N'双床房')");
            migrationBuilder.Sql("INSERT INTO RoomType (Name) VALUES (N'亲子房')");
            migrationBuilder.Sql("INSERT INTO RoomType (Name) VALUES (N'商务套房')");
            migrationBuilder.Sql("INSERT INTO RoomType (Name) VALUES (N'总统套房')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
