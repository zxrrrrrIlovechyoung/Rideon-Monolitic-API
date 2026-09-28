using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using rideon.Data;
using rideon.Models;

namespace rideon.Controllers
{
    public class CatMarcaMotoesController : Controller
    {
        private readonly rideonContext _context;

        public CatMarcaMotoesController(rideonContext context)
        {
            _context = context;
        }

        // GET: CatMarcaMotoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.CatMarcaMoto.ToListAsync());
        }

        // GET: CatMarcaMotoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var catMarcaMoto = await _context.CatMarcaMoto
                .FirstOrDefaultAsync(m => m.idMarca == id);

            if (catMarcaMoto == null)
                return NotFound();

            return View(catMarcaMoto);
        }

        // GET: CatMarcaMotoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CatMarcaMotoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("idMarca,strMarca")] CatMarcaMoto catMarcaMoto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(catMarcaMoto);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Marca registrada con éxito.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {         
                    string error = ex.InnerException?.Message ?? ex.Message;
                    TempData["Error"] = $"❌ Error de base de datos: {error}";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"❌ Error inesperado: {ex.Message}";
                }
            }

            return View(catMarcaMoto);
        }

        // GET: CatMarcaMotoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var mantenimiento = await _context.CatMantenimiento
                .Include(m => m.Moto)            
                .ThenInclude(m => m.CatMarcaMoto)    
                .Include(m => m.Moto.CatTipoMoto) 
                .FirstOrDefaultAsync(m => m.idCatMantenimiento == id);

            if (mantenimiento == null)
                return NotFound();

            return View(mantenimiento);
        }


        // POST: CatMarcaMotoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("idMarca,strMarca")] CatMarcaMoto catMarcaMoto)
        {
            if (id != catMarcaMoto.idMarca)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(catMarcaMoto);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Marca actualizada con éxito.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    string error = ex.InnerException?.Message ?? ex.Message;
                    TempData["Error"] = $"❌ Error de base de datos: {error}";
                    return View(catMarcaMoto);
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"❌ Error inesperado: {ex.Message}";
                    return View(catMarcaMoto);
                }
            }

            return View(catMarcaMoto);
        }

        // GET: CatMarcaMotoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var catMarcaMoto = await _context.CatMarcaMoto
                .FirstOrDefaultAsync(m => m.idMarca == id);

            if (catMarcaMoto == null)
                return NotFound();

            return View(catMarcaMoto);
        }

        // POST: CatMarcaMotoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var catMarcaMoto = await _context.CatMarcaMoto.FindAsync(id);

                if (catMarcaMoto != null)
                    _context.CatMarcaMoto.Remove(catMarcaMoto);

                await _context.SaveChangesAsync();
                TempData["Success"] = "Marca eliminada correctamente.";
            }
            catch (DbUpdateException ex)
            {
                string error = ex.InnerException?.Message ?? ex.Message;
                TempData["Error"] = $"❌ No se puede eliminar esta marca: {error}";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"❌ Error inesperado: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CatMarcaMotoExists(int id)
        {
            return _context.CatMarcaMoto.Any(e => e.idMarca == id);
        }
    }
}
