using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rideon.Data;
using rideon.Models;

namespace rideon.Controllers
{
    public class CatMantenimientoesController : Controller
    {
        private readonly rideonContext _context;

        public CatMantenimientoesController(rideonContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var mantenimientos = await _context.CatMantenimiento
                .Include(m => m.Moto)
                    .ThenInclude(m => m.CatMarcaMoto)
                .ToListAsync();

            return View(mantenimientos);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var mantenimiento = await _context.CatMantenimiento
                .Include(m => m.Moto)
                    .ThenInclude(m => m.CatMarcaMoto)
                .FirstOrDefaultAsync(m => m.idCatMantenimiento == id);

            if (mantenimiento == null)
                return NotFound();

            return View(mantenimiento);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CatMantenimiento catMantenimiento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(catMantenimiento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(catMantenimiento);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var mantenimiento = await _context.CatMantenimiento
                .Include(m => m.Moto)
                    .ThenInclude(m => m.CatMarcaMoto)
                .FirstOrDefaultAsync(m => m.idCatMantenimiento == id);

            if (mantenimiento == null)
                return NotFound();

            return View(mantenimiento);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CatMantenimiento catMantenimiento)
        {
            if (id != catMantenimiento.idCatMantenimiento)
                return NotFound();

            if (!ModelState.IsValid)
                return View(catMantenimiento);

            try
            {
                _context.Update(catMantenimiento);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.CatMantenimiento.Any(e => e.idCatMantenimiento == id))
                    return NotFound();

                throw;
            }

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var mantenimiento = await _context.CatMantenimiento
                .Include(m => m.Moto)
                    .ThenInclude(m => m.CatMarcaMoto)
                .FirstOrDefaultAsync(m => m.idCatMantenimiento == id);

            if (mantenimiento == null)
                return NotFound();

            return View(mantenimiento);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mantenimiento = await _context.CatMantenimiento.FindAsync(id);

            if (mantenimiento != null)
            {
                _context.CatMantenimiento.Remove(mantenimiento);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> SearchMotosCatalogo(string term)
        {
            var motos = await _context.CatMoto
                .Include(m => m.CatMarcaMoto)
                .Where(m =>
                    string.IsNullOrEmpty(term)
                    || m.CatMarcaMoto.strMarca.Contains(term)
                    || m.strModelo.Contains(term)
                    || m.strCilindraje.Contains(term)
                )
                .Select(m => new
                {
                    id = m.idMoto,
                    text = m.CatMarcaMoto.strMarca + " " + m.strModelo + " (" + m.strCilindraje + ")"
                })
                .Take(20)
                .ToListAsync();

            return Json(motos);
        }
    }
}
