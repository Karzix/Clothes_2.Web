using System.ComponentModel.DataAnnotations;

namespace Clothes_2.Web.Models
{
	public class ChiTietHoaDon
	{
		[Key]
		public Guid Id { get; set; }

		public string? TenKhachHang { get; set; }
		public string? SoDienThoai { get; set; }
		public string? DiaChi { get; set; }
		public int ThanhTien { get; set; }
		public DateTime NgayXuatHoaDon { get; set; }

		public string? listGioHang { get; set; }
		public List<GioHang>? GioHangs { get; set;}
	}
}
