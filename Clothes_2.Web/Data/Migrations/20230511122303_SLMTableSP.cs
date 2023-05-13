using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clothes_2.Web.Data.Migrations
{
    public partial class SLMTableSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SoLuotMua",
                table: "SanPham",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "TenLoaiSanPham",
                table: "LoaiSanPham",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "GioHang",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    size = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SanPhamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ChiTietHoaDonId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GioHang", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GioHang_ChiTietHoaDon_ChiTietHoaDonId",
                        column: x => x.ChiTietHoaDonId,
                        principalTable: "ChiTietHoaDon",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GioHang_SanPham_SanPhamId",
                        column: x => x.SanPhamId,
                        principalTable: "SanPham",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GioHang_ChiTietHoaDonId",
                table: "GioHang",
                column: "ChiTietHoaDonId");

            migrationBuilder.CreateIndex(
                name: "IX_GioHang_SanPhamId",
                table: "GioHang",
                column: "SanPhamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GioHang");

            migrationBuilder.DropColumn(
                name: "SoLuotMua",
                table: "SanPham");

            migrationBuilder.AlterColumn<string>(
                name: "TenLoaiSanPham",
                table: "LoaiSanPham",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
