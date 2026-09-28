using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using rideon.Data;
using rideon.Models;
using rideon.Services;

namespace rideon.Controllers
{
    public class CatMotoesController : Controller
    {
        private readonly rideonContext _context;
        private readonly CloudinaryService _cloudinaryService;

        public CatMotoesController(rideonContext context, CloudinaryService cloudinaryService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
        }
        public async Task<IActionResult> Index()
        {
            var catMotos = _context.CatMoto
                .Include(m => m.CatMarcaMoto)
                .Include(m => m.CatTipoMoto);

            return View(await catMotos.ToListAsync());
        }
        public IActionResult Create()
        {
            LoadSelectLists();
            return View(new CatMoto());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CatMoto catMoto, IFormFile foto)
        {
            ModelState.Remove("imgFoto");

            if (foto == null)
                ModelState.AddModelError("foto", "Debes seleccionar una imagen.");

            if (!ModelState.IsValid)
            {
                LoadSelectLists(catMoto);
                return View(catMoto);
            }

            string imageUrl = await _cloudinaryService.UploadImageAsync(foto);

            if (string.IsNullOrEmpty(imageUrl))
            {
                ModelState.AddModelError("foto", "Error al subir la imagen.");
                LoadSelectLists(catMoto);
                return View(catMoto);
            }

            catMoto.imgFoto = imageUrl;

            _context.Add(catMoto);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Moto registrada con éxito.";
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var catMoto = await _context.CatMoto.FindAsync(id);
            if (catMoto == null) return NotFound();

            LoadSelectLists(catMoto);
            return View(catMoto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CatMoto catMoto, IFormFile foto)
        {
            if (id != catMoto.idMoto)
                return NotFound();

            ModelState.Remove("imgFoto");

            if (!ModelState.IsValid)
            {
                LoadSelectLists(catMoto);
                return View(catMoto);
            }
            var motoStored = await _context.CatMoto.AsNoTracking()
                .FirstOrDefaultAsync(m => m.idMoto == id);

            if (motoStored == null)
                return NotFound();

            if (foto != null)
            {
                string newUrl = await _cloudinaryService.UploadImageAsync(foto);

                if (!string.IsNullOrEmpty(newUrl))
                    catMoto.imgFoto = newUrl;
            }
            else
            {
                catMoto.imgFoto = motoStored.imgFoto;
            }

            _context.Update(catMoto);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Moto actualizada.";
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var catMoto = await _context.CatMoto
                .Include(m => m.CatMarcaMoto)
                .Include(m => m.CatTipoMoto)
                .FirstOrDefaultAsync(m => m.idMoto == id);

            if (catMoto == null) return NotFound();

            return View(catMoto);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var catMoto = await _context.CatMoto.FindAsync(id);
            if (catMoto != null)
            {
                _context.CatMoto.Remove(catMoto);
                await _context.SaveChangesAsync();
            }

            TempData["Success"] = "Moto eliminada.";
            return RedirectToAction(nameof(Index));
        }
        private void LoadSelectLists(CatMoto catMoto = null)
        {
            ViewData["idMarca"] = new SelectList(
                _context.CatMarcaMoto.ToList(),
                "idMarca",
                "strMarca",
                catMoto?.idMarca
            );

            ViewData["idTipoMoto"] = new SelectList(
                _context.CatTipoMoto.ToList(),
                "idTipoMoto",
                "strTipoMoto",
                catMoto?.idTipoMoto
            );
        }
    }
}
