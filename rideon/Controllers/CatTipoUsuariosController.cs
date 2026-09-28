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
    public class CatTipoUsuariosController : Controller
    {
        private readonly rideonContext _context;

        public CatTipoUsuariosController(rideonContext context)
        {
            _context = context;
        }

        // GET: CatTipoUsuarios
        public async Task<IActionResult> Index()
        {
            return View(await _context.CatTipoUsuario.ToListAsync());
        }

        // GET: CatTipoUsuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catTipoUsuario = await _context.CatTipoUsuario
                .FirstOrDefaultAsync(m => m.idTipoUsuario == id);
            if (catTipoUsuario == null)
            {
                return NotFound();
            }

            return View(catTipoUsuario);
        }

        // GET: CatTipoUsuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CatTipoUsuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("idTipoUsuario,strDescripcion")] CatTipoUsuario catTipoUsuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(catTipoUsuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(catTipoUsuario);
        }

        // GET: CatTipoUsuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catTipoUsuario = await _context.CatTipoUsuario.FindAsync(id);
            if (catTipoUsuario == null)
            {
                return NotFound();
            }
            return View(catTipoUsuario);
        }

        // POST: CatTipoUsuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("idTipoUsuario,strDescripcion")] CatTipoUsuario catTipoUsuario)
        {
            if (id != catTipoUsuario.idTipoUsuario)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(catTipoUsuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CatTipoUsuarioExists(catTipoUsuario.idTipoUsuario))
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
            return View(catTipoUsuario);
        }

        // GET: CatTipoUsuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catTipoUsuario = await _context.CatTipoUsuario
                .FirstOrDefaultAsync(m => m.idTipoUsuario == id);
            if (catTipoUsuario == null)
            {
                return NotFound();
            }

            return View(catTipoUsuario);
        }

        // POST: CatTipoUsuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var catTipoUsuario = await _context.CatTipoUsuario.FindAsync(id);
            if (catTipoUsuario != null)
            {
                _context.CatTipoUsuario.Remove(catTipoUsuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CatTipoUsuarioExists(int id)
        {
            return _context.CatTipoUsuario.Any(e => e.idTipoUsuario == id);
        }
    }
}
