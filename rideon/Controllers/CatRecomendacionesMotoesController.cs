using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using rideon.Data;
using rideon.Models;

namespace rideon.Controllers
{
    public class CatRecomendacionesMotoesController : Controller
    {
        private readonly rideonContext _context;
        private readonly ILogger<CatRecomendacionesMotoesController> _logger;

        public CatRecomendacionesMotoesController(rideonContext context, ILogger<CatRecomendacionesMotoesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ================================
        // LISTA
        // ================================
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("📄 Cargando lista de recomendaciones...");

            var lista = await _context.CatRecomendacionesMoto
                .Include(r => r.Moto)
                    .ThenInclude(m => m.CatMarcaMoto)
                .ToListAsync();

            return View(lista);
        }

        // ================================
        // DETALLES
        // ================================
        public async Task<IActionResult> Details(int? id)
        {
            _logger.LogInformation("📄 Details ID={id}", id);

            if (id == null) return NotFound();

            var recomendacion = await _context.CatRecomendacionesMoto
                .Include(r => r.Moto)
                    .ThenInclude(m => m.CatMarcaMoto)
                .FirstOrDefaultAsync(m => m.idRecomendacion == id);

            if (recomendacion == null)
            {
                _logger.LogWarning("⚠️ Recomendación {id} no encontrada", id);
                return NotFound();
            }

            return View(recomendacion);
        }

        // ================================
        // CREATE GET
        // ================================
        public IActionResult Create()
        {
            _logger.LogInformation("➡️ Entrando a Create GET");

            CargarMotosSelect();
            return View();
        }

        // ================================
        // CREATE POST
        // ================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CatRecomendacionesMoto model)
        {
            _logger.LogInformation("🔵 Entrando a Create POST");

            try
            {
                _logger.LogInformation("Datos recibidos: {model}", System.Text.Json.JsonSerializer.Serialize(model));
            }
            catch { }

            // Mostrar errores del modelo directamente
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("⚠️ ModelState inválido");

                var firstError = ModelState.Values.SelectMany(v => v.Errors)
                                                  .FirstOrDefault()?.ErrorMessage;

                ViewBag.ErrorMessage = "Error de validación: " + firstError;

                CargarMotosSelect(model.idMoto);
                return View(model);
            }

            try
            {
                _logger.LogInformation("🟢 Guardando recomendación en BD...");

                _context.Add(model);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error al guardar la recomendación");

                ViewBag.ErrorMessage = ex.Message;

                CargarMotosSelect(model.idMoto);
                return View(model);
            }
        }


        // ================================
        // EDIT GET
        // ================================
        public async Task<IActionResult> Edit(int? id)
        {
            _logger.LogInformation("✏️ Entrando a Edit GET ID={id}", id);

            if (id == null) return NotFound();

            var model = await _context.CatRecomendacionesMoto.FindAsync(id);
            if (model == null)
            {
                _logger.LogWarning("⚠️ No encontrada para editar ID={id}", id);
                return NotFound();
            }

            CargarMotosSelect(model.idMoto);
            return View(model);
        }

        // ================================
        // EDIT POST
        // ================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CatRecomendacionesMoto model)
        {
            _logger.LogInformation("✏️ Entrando a Edit POST ID={id}", id);

            if (id != model.idRecomendacion)
            {
                _logger.LogError("❌ ID no coincide en Edit. URL={urlId}, Model={modelId}", id, model.idRecomendacion);
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("⚠️ ModelState inválido en Edit");

                foreach (var item in ModelState)
                {
                    foreach (var error in item.Value.Errors)
                    {
                        _logger.LogError("❌ Error en campo {campo}: {error}",
                            item.Key, error.ErrorMessage);
                    }
                }

                CargarMotosSelect(model.idMoto);
                return View(model);
            }

            try
            {
                _logger.LogInformation("🟡 Intentando actualizar...");

                _context.Update(model);
                await _context.SaveChangesAsync();

                _logger.LogInformation("✅ Edit actualizado correctamente");
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!CatRecomendacionesMotoExists(model.idRecomendacion))
                {
                    _logger.LogError(ex, "❌ No existe al intentar editar");
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex, "❌ Error de concurrencia");
                    throw;
                }
            }
        }

        // ================================
        // DELETE GET
        // ================================
        public async Task<IActionResult> Delete(int? id)
        {
            _logger.LogInformation("🗑 Entrando a Delete GET ID={id}", id);

            if (id == null) return NotFound();

            var model = await _context.CatRecomendacionesMoto
                .Include(r => r.Moto)
                    .ThenInclude(m => m.CatMarcaMoto)
                .FirstOrDefaultAsync(m => m.idRecomendacion == id);

            if (model == null)
            {
                _logger.LogWarning("⚠️ No encontrada para eliminar ID={id}", id);
                return NotFound();
            }

            return View(model);
        }

        // ================================
        // DELETE POST
        // ================================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation("🗑 Eliminando recomendación ID={id}", id);

            var model = await _context.CatRecomendacionesMoto.FindAsync(id);
            if (model != null)
            {
                _context.CatRecomendacionesMoto.Remove(model);
                await _context.SaveChangesAsync();
            }

            _logger.LogInformation("✅ Eliminación completa");
            return RedirectToAction(nameof(Index));
        }

        // ================================
        // CHECK EXIST
        // ================================
        private bool CatRecomendacionesMotoExists(int id)
        {
            return _context.CatRecomendacionesMoto.Any(e => e.idRecomendacion == id);
        }

        // ================================
        // SELECT MOTOS
        // ================================
        private void CargarMotosSelect(int? selectedMoto = null)
        {
            _logger.LogInformation("📌 Cargando lista de motos para el Select...");

            var motos = _context.CatMoto
                .Include(m => m.CatMarcaMoto)
                .Select(m => new
                {
                    m.idMoto,
                    Texto = m.CatMarcaMoto.strMarca + " " + m.strModelo + " " + m.strCilindraje
                })
                .ToList();

            ViewBag.idMoto = new SelectList(motos, "idMoto", "Texto", selectedMoto);
        }
    }
}
