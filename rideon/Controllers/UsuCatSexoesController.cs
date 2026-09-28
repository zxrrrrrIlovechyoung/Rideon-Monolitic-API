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
    public class UsuCatSexoesController : Controller
    {
        private readonly rideonContext _context;

        public UsuCatSexoesController(rideonContext context)
        {
            _context = context;
        }

        // GET: UsuCatSexoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.UsuCatSexo.ToListAsync());
        }

        // GET: UsuCatSexoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuCatSexo = await _context.UsuCatSexo
                .FirstOrDefaultAsync(m => m.idSexo == id);
            if (usuCatSexo == null)
            {
                return NotFound();
            }

            return View(usuCatSexo);
        }

        // GET: UsuCatSexoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UsuCatSexoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("idSexo,strDescripcion")] UsuCatSexo usuCatSexo)
        {
            Console.WriteLine(">>> MÉTODO CREATE POST EJECUTÁNDOSE...");

         
            if (ModelState.IsValid)
            {
                try
                {
                   
                    _context.Add(usuCatSexo);
                    await _context.SaveChangesAsync();

                    Console.WriteLine(">>> REGISTRO GUARDADO CORRECTAMENTE");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                   
                    Console.WriteLine(">>> ERROR AL GUARDAR: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine(">>> MODELSTATE NO ES VÁLIDO");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(" - Error: " + error.ErrorMessage);
                }
            }
            return View(usuCatSexo);
        }





        // GET: UsuCatSexoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuCatSexo = await _context.UsuCatSexo.FindAsync(id);
            if (usuCatSexo == null)
            {
                return NotFound();
            }
            return View(usuCatSexo);
        }

        // POST: UsuCatSexoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("idSexo,strDescripcion")] UsuCatSexo usuCatSexo)
        {
            if (id != usuCatSexo.idSexo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuCatSexo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuCatSexoExists(usuCatSexo.idSexo))
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
            return View(usuCatSexo);
        }

        // GET: UsuCatSexoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuCatSexo = await _context.UsuCatSexo
                .FirstOrDefaultAsync(m => m.idSexo == id);
            if (usuCatSexo == null)
            {
                return NotFound();
            }

            return View(usuCatSexo);
        }

        // POST: UsuCatSexoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuCatSexo = await _context.UsuCatSexo.FindAsync(id);
            if (usuCatSexo != null)
            {
                _context.UsuCatSexo.Remove(usuCatSexo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuCatSexoExists(int id)
        {
            return _context.UsuCatSexo.Any(e => e.idSexo == id);
        }
    }
}
