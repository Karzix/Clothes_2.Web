using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Clothes_2.Web.Data;
using Clothes_2.Web.Models;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Clothes_2.Web.Controllers
{
    public class ChiTietHoaDonController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChiTietHoaDonController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ChiTietHoaDon
        public async Task<IActionResult> Index()
        {
              return _context.ChiTietHoaDon != null ? 
                          View(await _context.ChiTietHoaDon.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.ChiTietHoaDon'  is null.");
        }

		//      // GET: ChiTietHoaDon/Create
		//      public IActionResult Create()
		//      {
		//          string danhSachSPTrongGioHang;
		//          string key = User.FindFirstValue(ClaimTypes.NameIdentifier);
		//          danhSachSPTrongGioHang = Request.Cookies[key];

		//          int thanhtien = 0;
		//          GioHang gh;
		//          List<GioHang> listsp = new List<GioHang>();
		//          try
		//          {
		//              listsp = JsonConvert.DeserializeObject<List<GioHang>>(danhSachSPTrongGioHang);
		//          }
		//          catch (Exception ex)
		//          {
		//              try
		//              {
		//                  gh = JsonConvert.DeserializeObject<GioHang>(danhSachSPTrongGioHang);
		//              }
		//              catch
		//              {
		//                  return View("GioHangError");
		//              }
		//              listsp.Add(gh);
		//          }
		//          foreach(var item in listsp)
		//          {
		//              thanhtien += item.SoLuong * item.SanPham.Gia;
		//          }

		//          ChiTietHoaDon chiTietHoaDon = new ChiTietHoaDon();
		//          chiTietHoaDon.GioHangs = listsp;
		//          chiTietHoaDon.ThanhTien = thanhtien;
		//          return View(chiTietHoaDon);
		//      }

		// POST: ChiTietHoaDon/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(string SoDienThoai, string DiaChi, string TenKhachHang)
		{
			string key = User.FindFirstValue(ClaimTypes.NameIdentifier);
			string danhSachSPTrongGioHang = Request.Cookies[key];
			int thanhtien = 0;
			List<GioHang> listsp = new List<GioHang>();
			GioHang gh;
			try
			{
				listsp = JsonConvert.DeserializeObject<List<GioHang>>(danhSachSPTrongGioHang);
			}
			catch (Exception ex)
			{
				try
				{
					gh = JsonConvert.DeserializeObject<GioHang>(danhSachSPTrongGioHang);
				}
				catch
				{
					return View("GioHangError");
				}
				listsp.Add(gh);
			}

			var danhSachIdGioHang = "";
			foreach (var item in listsp)
			{
				danhSachIdGioHang += " /ID: ";
				thanhtien += item.SoLuong * item.SanPham.Gia;
				danhSachIdGioHang += item.SanPhamId + " SoLuong: " + item.SoLuong + " Size: " + item.size;
			}
			foreach (var item in listsp)
			{
				var updateSLM = await _context.SanPham.Where(sp => sp.Id == item.SanPhamId).FirstOrDefaultAsync();
				updateSLM.SoLuotMua += item.SoLuong;
				_context.Update(updateSLM);
				await _context.SaveChangesAsync();
			}
			ChiTietHoaDon chiTietHoaDon = new ChiTietHoaDon
			{
				SoDienThoai = SoDienThoai,
				DiaChi = DiaChi,
				TenKhachHang = TenKhachHang,
				listGioHang = danhSachIdGioHang,
				ThanhTien = thanhtien,
				NgayXuatHoaDon = DateTime.Now
			};
			if (ModelState.IsValid)
			{
				chiTietHoaDon.Id = Guid.NewGuid();
				_context.Add(chiTietHoaDon);
				await _context.SaveChangesAsync();
			}
			string updateGioHang = JsonConvert.SerializeObject(listsp);
			CookieOptions updatecookie = new CookieOptions()
			{
				Expires = DateTime.Now.AddDays(-1)
			};
			Response.Cookies.Append(key, updateGioHang, updatecookie);
			return RedirectToAction("Index", "Home", _context.SanPham);
		}


		private bool ChiTietHoaDonExists(Guid id)
        {
          return (_context.ChiTietHoaDon?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
