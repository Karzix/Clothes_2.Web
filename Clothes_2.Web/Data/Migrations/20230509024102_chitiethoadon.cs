using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clothes_2.Web.Data.Migrations
{
    public partial class chitiethoadon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "listGioHang",
                table: "ChiTietHoaDon",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "listGioHang",
                table: "ChiTietHoaDon");
        }
    }
}
