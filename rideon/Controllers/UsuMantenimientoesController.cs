using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using rideon.Data;
using rideon.Models;

namespace rideon.Controllers
{
    public class UsuMantenimientoesController : Controller
    {
        private readonly rideonContext _context;

        public UsuMantenimientoesController(rideonContext context)
        {
            _context = context;
        }

        // GET: UsuMantenimientoes
        public async Task<IActionResult> Index()
        {
            var mantenimientos = _context.UsuMantenimiento
                .Include(u => u.CatMantenimiento)
                .Include(u => u.Usuario)
                .Include(u => u.Moto); 

            return View(await mantenimientos.ToListAsync());
        }

        // GET: UsuMantenimientoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var usuMantenimiento = await _context.UsuMantenimiento
                .Include(u => u.CatMantenimiento)
                .Include(u => u.Usuario)
                .Include(u => u.Moto)
                .FirstOrDefaultAsync(m => m.idMantenimientoUsuario == id);

            if (usuMantenimiento == null) return NotFound();

            return View(usuMantenimiento);
        }

        // GET: UsuMantenimientoes/Create
        public IActionResult Create()
        {
            ViewData["idCatMantenimiento"] = new SelectList(_context.CatMantenimiento, "idCatMantenimiento", "Nombre");
            ViewData["idMotoUsuario"] = new SelectList(_context.CatMoto, "idMotoUsuario", "strMarca");
            ViewData["idUsuario"] = new SelectList(_context.UsuUsuarios, "idUsuario", "strNombre");
            return View();
        }

        // POST: UsuMantenimientoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("idMantenimientoUsuario,idUsuario,idMotoUsuario,idCatMantenimiento,dtFecha,strNotas")] UsuMantenimiento usuMantenimiento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuMantenimiento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["idCatMantenimiento"] = new SelectList(_context.CatMantenimiento, "idCatMantenimiento", "Nombre", usuMantenimiento.idCatMantenimiento);
            ViewData["idMotoUsuario"] = new SelectList(_context.CatMoto, "idMotoUsuario", "strMarca", usuMantenimiento.idMotoUsuario);
            ViewData["idUsuario"] = new SelectList(_context.UsuUsuarios, "idUsuario", "strNombre", usuMantenimiento.idUsuario);
            return View(usuMantenimiento);
        }

        // GET: UsuMantenimientoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var usuMantenimiento = await _context.UsuMantenimiento.FindAsync(id);
            if (usuMantenimiento == null) return NotFound();

            ViewData["idCatMantenimiento"] = new SelectList(_context.CatMantenimiento, "idCatMantenimiento", "Nombre", usuMantenimiento.idCatMantenimiento);
            ViewData["idMotoUsuario"] = new SelectList(_context.CatMoto, "idMotoUsuario", "strMarca", usuMantenimiento.idMotoUsuario);
            ViewData["idUsuario"] = new SelectList(_context.UsuUsuarios, "idUsuario", "strNombre", usuMantenimiento.idUsuario);
            return View(usuMantenimiento);
        }

        // POST: UsuMantenimientoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("idMantenimientoUsuario,idUsuario,idMotoUsuario,idCatMantenimiento,dtFecha,strNotas")] UsuMantenimiento usuMantenimiento)
        {
            if (id != usuMantenimiento.idMantenimientoUsuario) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuMantenimiento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuMantenimientoExists(usuMantenimiento.idMantenimientoUsuario))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["idCatMantenimiento"] = new SelectList(_context.CatMantenimiento, "idCatMantenimiento", "Nombre", usuMantenimiento.idCatMantenimiento);
            ViewData["idMotoUsuario"] = new SelectList(_context.CatMoto, "idMotoUsuario", "strMarca", usuMantenimiento.idMotoUsuario);
            ViewData["idUsuario"] = new SelectList(_context.UsuUsuarios, "idUsuario", "strNombre", usuMantenimiento.idUsuario);
            return View(usuMantenimiento);
        }

        // GET: UsuMantenimientoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var usuMantenimiento = await _context.UsuMantenimiento
                .Include(u => u.CatMantenimiento)
                .Include(u => u.Usuario)
                .Include(u => u.Moto)
                .FirstOrDefaultAsync(m => m.idMantenimientoUsuario == id);

            if (usuMantenimiento == null) return NotFound();

            return View(usuMantenimiento);
        }

        // POST: UsuMantenimientoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuMantenimiento = await _context.UsuMantenimiento.FindAsync(id);
            if (usuMantenimiento != null)
            {
                _context.UsuMantenimiento.Remove(usuMantenimiento);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool UsuMantenimientoExists(int id)
        {
            return _context.UsuMantenimiento.Any(e => e.idMantenimientoUsuario == id);
        }
    }
}
