using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Clothes_2.Web.Models;

namespace Clothes_2.Web.Data
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}
		public DbSet<Clothes_2.Web.Models.LoaiSanPham>? LoaiSanPham { get; set; }
		public DbSet<Clothes_2.Web.Models.SanPham>? SanPham { get; set; }
		public DbSet<Clothes_2.Web.Models.ChiTietHoaDon>? ChiTietHoaDon { get; set; }
		public DbSet<Clothes_2.Web.Models.GioHang>? GioHang { get; set; }
	}
}