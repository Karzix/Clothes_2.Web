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

        // GET: ChiTietHoaDon/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.ChiTietHoaDon == null)
            {
                return NotFound();
            }

            var chiTietHoaDon = await _context.ChiTietHoaDon
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chiTietHoaDon == null)
            {
                return NotFound();
            }

            return View(chiTietHoaDon);
        }

        // GET: ChiTietHoaDon/Create
        public IActionResult Create(int SoLuong, string Size, Guid id)
        {
            string danhSachSPTrongGioHang;

            if (id == null)
            {
                string key = User.FindFirstValue(ClaimTypes.NameIdentifier);
                danhSachSPTrongGioHang = Request.Cookies[key];
            }
            else
            {
                GioHang gh2 = new GioHang
                {
                    Id = Guid.NewGuid(),
                    size = Size,
                    SanPhamId = id,
                    SoLuong = SoLuong,
                    SanPham = _context.SanPham.Where(sp => sp.Id == id).FirstOrDefault()
                };
                danhSachSPTrongGioHang = JsonConvert.SerializeObject(gh2);
            }
            int thanhtien = 0;
            GioHang gh;
            List<GioHang> listsp = new List<GioHang>();
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

            ChiTietHoaDon chiTietHoaDon = new ChiTietHoaDon();
            chiTietHoaDon.GioHangs = listsp;
            return View(chiTietHoaDon);
        }

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

		// GET: ChiTietHoaDon/Edit/5
		public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.ChiTietHoaDon == null)
            {
                return NotFound();
            }

            var chiTietHoaDon = await _context.ChiTietHoaDon.FindAsync(id);
            if (chiTietHoaDon == null)
            {
                return NotFound();
            }
            return View(chiTietHoaDon);
        }

        // POST: ChiTietHoaDon/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,TenKhachHang,SoDienThoai,DiaChi,ThanhTien,NgayXuatHoaDon")] ChiTietHoaDon chiTietHoaDon)
        {
            if (id != chiTietHoaDon.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chiTietHoaDon);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChiTietHoaDonExists(chiTietHoaDon.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(chiTietHoaDon);
        }

        // GET: ChiTietHoaDon/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.ChiTietHoaDon == null)
            {
                return NotFound();
            }

            var chiTietHoaDon = await _context.ChiTietHoaDon
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chiTietHoaDon == null)
            {
                return NotFound();
            }

            return View(chiTietHoaDon);
        }

        // POST: ChiTietHoaDon/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.ChiTietHoaDon == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ChiTietHoaDon'  is null.");
            }
            var chiTietHoaDon = await _context.ChiTietHoaDon.FindAsync(id);
            if (chiTietHoaDon != null)
            {
                _context.ChiTietHoaDon.Remove(chiTietHoaDon);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChiTietHoaDonExists(Guid id)
        {
          return (_context.ChiTietHoaDon?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
