using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rideon.Data;
using rideon.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace rideon.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly rideonContext _context;

        public UsuarioController(rideonContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var nombreUsuario = HttpContext.Session.GetString("UsuarioNombre");
            var idUsuario = HttpContext.Session.GetInt32("UsuarioId");

            if (string.IsNullOrEmpty(nombreUsuario) || idUsuario == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Mensaje = $"Bienvenido, {nombreUsuario}";
            var motosUsuario = _context.UsuUsuarioMotos
                .Include(um => um.CatMoto)
                    .ThenInclude(m => m.CatMarcaMoto)
                .Where(um => um.idUsuario == idUsuario)
                .ToList();

            ViewBag.CatTipos = _context.CatTipoMoto.ToList();

            ViewBag.CatMotos = _context.CatMoto
                .Include(c => c.CatMarcaMoto)
                .Include(c => c.CatTipoMoto)
                .ToList();

            if (motosUsuario.Any())
            {
                var motoActual = motosUsuario.First();
                int motoId = motoActual.idMoto ?? 0;
                int odo = motoActual.odoTotal ?? 0;
                ViewBag.Recomendaciones = _context.CatRecomendacionesMoto
                    .Where(r => r.idMoto == motoId && r.KmRecomendado <= odo)
                    .OrderBy(r => r.KmRecomendado)
                    .ToList();
            }
            else
            {
                ViewBag.Recomendaciones = new List<CatRecomendacionesMoto>();
            }


            return View(motosUsuario);
        }

        public IActionResult AddMotoPopup()
        {
            ViewBag.CatMotos = _context.CatMoto
                .Include(c => c.CatMarcaMoto)
                .Include(c => c.CatTipoMoto)
                .ToList();

            ViewBag.CatTipos = _context.CatTipoMoto.ToList();

            return PartialView("_AddMotoPopup");
        }
        [HttpPost]
        public async Task<IActionResult> AddUsuarioMoto(UsuUsuarioMoto usuarioMoto)
        {
            // validar sesión
            var idUsuario = HttpContext.Session.GetInt32("UsuarioId");
            if (idUsuario == null)
            {
                return RedirectToAction("Login", "Account");
            }
            usuarioMoto.idUsuario = idUsuario.Value;
            if (!usuarioMoto.idMoto.HasValue)
            {
                TempData["ErrorAddMoto"] = "Debes seleccionar una moto del catálogo.";
                return RedirectToAction("Index");
            }

            _context.UsuUsuarioMotos.Add(usuarioMoto);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult BuscarMotos(string q, int? tipo)
        {
            var query = _context.CatMoto
                .Include(x => x.CatMarcaMoto)
                .Include(x => x.CatTipoMoto)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                var term = q.Trim();
                query = query.Where(x =>
                    EF.Functions.Like(x.strModelo, $"%{term}%") ||
                    EF.Functions.Like(x.CatMarcaMoto.strMarca, $"%{term}%") ||
                    EF.Functions.Like(x.strCilindraje, $"%{term}%"));
            }

            if (tipo.HasValue)
            {
                query = query.Where(x => x.idTipoMoto == tipo.Value);
            }

            var result = query.Select(x => new
            {
                x.idMoto,
                x.strModelo,
                x.strCilindraje,
                x.imgFoto,
                marca = x.CatMarcaMoto.strMarca,
                tipoMoto = x.CatTipoMoto.strTipoMoto
            })
            .OrderBy(x => x.marca)
            .ThenBy(x => x.strModelo)
            .Take(200)
            .ToList();

            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> EliminarMoto(int idUsuarioMoto, string password)
        {
            var idUsuario = HttpContext.Session.GetInt32("UsuarioId");

            if (idUsuario == null)
                return Json(new { success = false, message = "Sesión no válida." });

            var usuario = await _context.UsuUsuarios
                .FirstOrDefaultAsync(u => u.idUsuario == idUsuario.Value);

            if (usuario == null)
                return Json(new { success = false, message = "Usuario no encontrado." });

            if (usuario.strContrasena != password)
                return Json(new { success = false, message = "Contraseña incorrecta." });

            var moto = await _context.UsuUsuarioMotos
                .FirstOrDefaultAsync(m => m.idUsuarioMoto == idUsuarioMoto &&
                                          m.idUsuario == idUsuario.Value);

            if (moto == null)
                return Json(new { success = false, message = "No se encontró la moto." });

            _context.UsuUsuarioMotos.Remove(moto);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Moto eliminada correctamente." });
        }






    }
}
