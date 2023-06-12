using System.Diagnostics;
using System.Drawing;
using System.Net.Mail;
using System.Net;
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

        public async Task<IActionResult> Index(int pageNumber)
		{
			var pageSize = 12;
			var applicationDbContext = _context.SanPham.Include(s => s.LoaiSanPham);
			var query = applicationDbContext.Skip(pageNumber * pageSize).Take(pageSize);
			ViewBag.MaxPage = applicationDbContext.Count() / pageSize;
			ViewBag.Page = pageNumber;
			ViewBag.TrangHienTai = pageNumber + 1;
			return _context.SanPham != null ?
                         View(await query.ToListAsync()) :
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

        public async Task<IActionResult> addCookie(int SoLuong, string Size, Guid IdSanPham)
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
                var kiemtragiohang = listsp.Where(sp=>sp.SanPhamId == IdSanPham && sp.size==Size).FirstOrDefault();
                if(kiemtragiohang == null)
                {
                    GioHang themgiohang = new GioHang
                    {
                        Id = Guid.NewGuid(),
                        SoLuong = SoLuong,
                        size = Size,
                        SanPhamId = IdSanPham,
                        SanPham = _context.SanPham.Where(sp => sp.Id == IdSanPham).FirstOrDefault()
                    };
                    listsp.Add(themgiohang);
                }
                else
                {
                    listsp.Where(sp => sp.SanPhamId == IdSanPham && sp.size == Size).FirstOrDefault().SoLuong += SoLuong;
                }
                
                string updateGioHang = JsonConvert.SerializeObject(listsp);
                CookieOptions updatecookie = new CookieOptions()
                {
                    Expires = DateTime.Now.AddDays(7)
                };
                Response.Cookies.Append(key, updateGioHang, updatecookie);
            }
            return View("Details",await _context.SanPham.Where(sp=>sp.Id==IdSanPham).FirstOrDefaultAsync());
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
                    return View("GioHang", listsp);
				}
                listsp.Add(gh);
            }
            int tongtien = 0;
            foreach(var item in listsp)
            {
                tongtien += item.SoLuong * item.SanPham.Gia;
            }
            ViewBag.TongTien=tongtien;
            return View("GioHang", listsp);
        }
        public IActionResult GioiThieu()
        {
            return View("GioiThieu");
        }
        public async Task<IActionResult> TimKiemTheoLoaiSP(Guid? id, string search, string asc, string sort)
        {
            ViewBag.timkiem = search;
            var applicationDbContext = _context.SanPham.Include(s => s.LoaiSanPham).AsQueryable();
            if (id != null)
            {
                applicationDbContext = _context.SanPham.Where(sp => sp.LoaiSanPhamId == id);
            }
            if (search != null)
            {
                applicationDbContext = _context.SanPham.Where(sp => sp.TenSanPham.Contains(search));
            }
            ViewBag.asc = asc;
            ViewBag.sort = sort;
            if (sort != null)
            {
                switch (sort)
                {
                    case "Gia":
                        {
                            if (asc == "True")
                            { applicationDbContext = applicationDbContext.OrderByDescending(sp => sp.Gia); }
                            else
                            { applicationDbContext = applicationDbContext.OrderBy(sp => sp.Gia); }
                            break;
                        }
                    case "SoLuotMua":
                        {
                            if (asc == "True")
                            { applicationDbContext = applicationDbContext.OrderByDescending(sp => sp.SoLuotMua); }
                            else
                            { applicationDbContext = applicationDbContext.OrderBy(sp => sp.SoLuotMua); }
                            break;
                        }
                    default:
                        break;
                }
            } 
            return View("TimKiemTheoLoaiSP",await applicationDbContext.ToListAsync());
        }


        public IActionResult DeleteGioHang(Guid id)
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
            listsp.Remove(listsp.Where(sp=>sp.Id==id).FirstOrDefault());
			string updateGioHang = JsonConvert.SerializeObject(listsp);
			//CookieOptions remove = new CookieOptions()
			//{
			//	Expires = DateTime.Now.AddDays(-1)
			//};
			//Response.Cookies.Append(key, updateGioHang, remove);
            CookieOptions updatecookie = new CookieOptions()
            {
                Expires = DateTime.Now.AddDays(7)
            };
            Response.Cookies.Append(key, updateGioHang, updatecookie);
            return View("GioHang", listsp);

		}
        public IActionResult Contact()
        {
            return View("Contact");
        }
        public IActionResult Blog()
        {
            return View("Blog");
        }
        //[HttpPost]
        //public ActionResult Contact(string ten, string sdt, string noidung)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Tạo một đối tượng MailMessage để chứa thông tin về email
        //        MailMessage mail = new MailMessage();
        //        mail.From = new MailAddress("anhkietlop10a3hvs@gmail.com");
        //        mail.To.Add("anhkietlop10a3hvs@gmail.com");
        //        mail.Subject = "liên hệ từ " + ten;
        //        mail.Body = "Tên: " + ten + "\nEmail: " + sdt + "\nNội dung: " + noidung;

        //        // Tạo một đối tượng SmtpClient để gửi email
        //        SmtpClient smtp = new SmtpClient();
        //        smtp.Host = "smtp.gmail.com"; // Địa chỉ máy chủ SMTP của bạn
        //        smtp.Port = 587; // Cổng SMTP của bạn
        //        smtp.Credentials = new NetworkCredential("anhkietlop10a3hvs@gmail.com", "kietvip1");
        //        smtp.EnableSsl = true;
        //        smtp.Send(mail);

        //        ////ViewBag.Message = "Cảm ơn bạn đã gửi khảo sát!";
        //    }
        //    return View("Contact"); 
        //}

    }
}