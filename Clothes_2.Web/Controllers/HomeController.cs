﻿using System.Diagnostics;
using System.Security.Claims;
using Clothes_2.Web.Data;
using Clothes_2.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Clothes_2.Web.Controllers
{
	public class HomeController : Controller
	{
		//private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
		{
            return _context.SanPham != null ?
                         View(await _context.SanPham.ToListAsync()) :
                         Problem("Entity set 'ApplicationDbContext.LoaiSanPham'  is null.");
        }

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

        // GET: SanPhams/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.SanPham == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPham
                .Include(s => s.LoaiSanPham)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }

        public IActionResult addCookie(int SoLuong, string Size, Guid IdSanPham)
        {
            string key = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (key == null)
            {
                return View("~/Areas/Identity/Pages/Account/Login");
            }
            var readcookie = Request.Cookies[key];
            if (readcookie == null)
            {
                GioHang addCookie = new GioHang
                {
                    Id = Guid.NewGuid(),
                    SoLuong = SoLuong,
                    size = Size,
                    SanPhamId = IdSanPham,
                    SanPham = _context.SanPham.Where(sp => sp.Id == IdSanPham).FirstOrDefault()
                };
                string json = JsonConvert.SerializeObject(addCookie);

                CookieOptions cookie = new CookieOptions()
                {
                    Expires = DateTime.Now.AddDays(7)
                };
                Response.Cookies.Append(key, json, cookie);
            }
            else
            {
                List<GioHang> listsp = new List<GioHang>();
                GioHang gh;
                try
                {
                    listsp = JsonConvert.DeserializeObject<List<GioHang>>(readcookie);
                }
                catch (Exception ex)
                {
                    gh = JsonConvert.DeserializeObject<GioHang>(readcookie);
                    listsp.Add(gh);
                }
                foreach (var item in listsp)
                {
                    if (item.SanPham == null)
                    {
                        listsp.Remove(item);
                    }
                }

                GioHang themgiohang = new GioHang
                {
                    Id = Guid.NewGuid(),
                    SoLuong = SoLuong,
                    size = Size,
                    SanPhamId = IdSanPham,
                    SanPham = _context.SanPham.Where(sp => sp.Id == IdSanPham).FirstOrDefault()
                };
                listsp.Add(themgiohang);
                string updateGioHang = JsonConvert.SerializeObject(listsp);
                CookieOptions updatecookie = new CookieOptions()
                {
                    Expires = DateTime.Now.AddDays(7)
                };
                Response.Cookies.Append(key, updateGioHang, updatecookie);
            }

            return View("Index", _context.SanPham);
        }

        public IActionResult ReadCookie()
        {
            string key = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var readcookie = Request.Cookies[key];
            List<GioHang> listsp = new List<GioHang>();
            GioHang gh;
            try
            {
                listsp = JsonConvert.DeserializeObject<List<GioHang>>(readcookie);
            }
            catch (Exception ex)
            {
                try
                {
                    gh = JsonConvert.DeserializeObject<GioHang>(readcookie);
                }
                catch
                {
                    return View("GioHangError");
                }
                listsp.Add(gh);
            }

            return View("GioHang", listsp);
        }
        public IActionResult GioiThieu()
        {
            return View("GioiThieu");
        }
        public async Task<IActionResult> TimKiemTheoLoaiSP(Guid id)
        {
            var _application =await _context.SanPham.Where(sp=>sp.LoaiSanPhamId==id).ToListAsync();

            return View("TimKiemTheoLoaiSP", _application);
        }
    }
}