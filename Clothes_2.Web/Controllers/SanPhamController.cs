using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Clothes_2.Web.Data;
using Clothes_2.Web.Models;

namespace Clothes_2.Web.Controllers
{
    public class SanPhamController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SanPhamController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SanPham
        public async Task<IActionResult> Index(int pageNumber , Guid? idSanPham)
        {
            var pageSize = 10;
            var applicationDbContext = _context.SanPham.Include(s => s.LoaiSanPham);
            var query = applicationDbContext.Skip(pageNumber * pageSize).Take(pageSize);
            if (idSanPham != null)
            {
            query = query.Where(sp => sp.Id == idSanPham);
            }
           
            ViewBag.MaxPage = applicationDbContext.Count() / pageSize;
            ViewBag.Page = pageNumber;
            ViewBag.TrangHienTai = pageNumber + 1;
            return View(await query.ToListAsync());
        }

        // GET: SanPham/Details/5
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

        // GET: SanPham/Create
        public IActionResult Create()
        {
            ViewData["LoaiSanPhamId"] = new SelectList(_context.LoaiSanPham, "Id", "TenLoaiSanPham");
            return View();
        }

        // POST: SanPham/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenSanPham,image,Gia,ThuongHieu,XuatSu,ChatLieu,LoaiSanPhamId")] SanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                sanPham.Id = Guid.NewGuid();
                _context.Add(sanPham);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LoaiSanPhamId"] = new SelectList(_context.LoaiSanPham, "Id", "TenLoaiSanPham", sanPham.LoaiSanPhamId);
            return View(sanPham);
        }

        // GET: SanPham/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.SanPham == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPham.FindAsync(id);
            if (sanPham == null)
            {
                return NotFound();
            }
            ViewData["LoaiSanPhamId"] = new SelectList(_context.LoaiSanPham, "Id", "TenLoaiSanPham", sanPham.LoaiSanPhamId);
            return View(sanPham);
        }

        // POST: SanPham/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,TenSanPham,image,Gia,ThuongHieu,XuatSu,ChatLieu,LoaiSanPhamId")] SanPham sanPham)
        {
            if (id != sanPham.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sanPham);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SanPhamExists(sanPham.Id))
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
            ViewData["LoaiSanPhamId"] = new SelectList(_context.LoaiSanPham, "Id", "TenLoaiSanPham", sanPham.LoaiSanPhamId);
            return View(sanPham);
        }

        // GET: SanPham/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
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

        // POST: SanPham/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.SanPham == null)
            {
                return Problem("Entity set 'ApplicationDbContext.SanPham'  is null.");
            }
            var sanPham = await _context.SanPham.FindAsync(id);
            if (sanPham != null)
            {
                _context.SanPham.Remove(sanPham);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SanPhamExists(Guid id)
        {
          return (_context.SanPham?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
