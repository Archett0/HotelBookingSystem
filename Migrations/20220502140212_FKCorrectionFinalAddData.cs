using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelBookingSystem.Migrations
{
    public partial class FKCorrectionFinalAddData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [dbo].[Movie] ([Name], [DateCheckIn], [DateCheckOut], [NumberInStock], [RoomTypeId]) VALUES (N'星际穿越', N'2017-05-06 00:00:00', N'2017-05-08 00:00:00', 10, 18)");
            migrationBuilder.Sql("INSERT INTO [dbo].[Movie] ([Name], [DateCheckIn], [DateCheckOut], [NumberInStock], [RoomTypeId]) VALUES (N'盗梦空间', N'2017-06-08 00:00:00', N'2017-06-09 00:00:00', 20, 17)");
            migrationBuilder.Sql("INSERT INTO [dbo].[Movie] ([Name], [DateCheckIn], [DateCheckOut], [NumberInStock], [RoomTypeId]) VALUES (N'大侦探福尔摩斯', N'2019-08-09 00:00:00', N'2019-08-10 00:00:00', 100, 15)");
            migrationBuilder.Sql("INSERT INTO [dbo].[Movie] ([Name], [DateCheckIn], [DateCheckOut], [NumberInStock], [RoomTypeId]) VALUES (N'信条', N'2020-09-01 00:00:00', N'2020-09-10 00:00:00', 5, 18)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
