using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Clothes_2.Web.Models
{
	public class SanPham
	{
		[Key]
		public Guid Id { get; set; }
		public string? TenSanPham { get; set; }
		public string? image { get; set; }
		public int Gia { get; set; }
		public string? ThuongHieu { get; set; }
		public string? XuatSu { get; set; }
		public string? ChatLieu { get; set; }
		public int SoLuotMua { get; set; } = 0;


		[ForeignKey("LoaiSanPhamId")]
		public LoaiSanPham? LoaiSanPham { get; set; }
		[ForeignKey("LoaiSanPham")]
		public Guid? LoaiSanPhamId { get; set; }
	}
}
