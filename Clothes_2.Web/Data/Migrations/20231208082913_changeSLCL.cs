using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clothes_2.Web.Data.Migrations
{
    public partial class changeSLCL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SoHangConLai",
                table: "SanPham",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SoHangConLai",
                table: "SanPham");
        }
    }
}
