using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Clothes_2.Web.Models
{
	public class GioHang
	{
		[Key]
		public Guid Id { get; set; }

		public int SoLuong { get; set; }
		public string size { get; set; } = null!;

		[ForeignKey("SanPhamId")]
		public SanPham? SanPham { get; set; }
		[ForeignKey("SanPham")]
		public Guid? SanPhamId { get; set; }
	}
}
