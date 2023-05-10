using System.ComponentModel.DataAnnotations;

namespace Clothes_2.Web.Models
{
	public class LoaiSanPham
	{
		[Key]
		public Guid Id { get; set; }
		public string? TenLoaiSanPham { get; set; }
	}
}
