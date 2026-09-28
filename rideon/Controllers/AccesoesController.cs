using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using rideon.Data;
using rideon.Models;

namespace rideon.Controllers
{
    public class AccesoesController : Controller
    {
        private readonly rideonContext _context;

        public AccesoesController(rideonContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (TempData["RegistroExitoso"] != null)
            {
                ViewBag.Mensaje = TempData["RegistroExitoso"].ToString();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UsuUsuario model)
        {
            if (string.IsNullOrWhiteSpace(model.strCorreo) || string.IsNullOrWhiteSpace(model.strContrasena))
            {
                ViewBag.Error = "Por favor, completa todos los campos.";
                return View(model);
            }

            var usuario = await _context.UsuUsuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.strCorreo == model.strCorreo);

            if (usuario == null || usuario.strContrasena != model.strContrasena)
            {
                ViewBag.Error = "Usuario no encontrado o contraseña incorrecta.";
                return View(model);
            }

            if (!usuario.activo)
            {
                ViewBag.Error = "Tu cuenta está inactiva. Contacta al administrador.";
                return View(model);
            }

            var usuarioUpdate = await _context.UsuUsuarios.FindAsync(usuario.idUsuario);
            if (usuarioUpdate != null)
            {
                usuarioUpdate.dtUltimoAcceso = DateTime.Now;
                await _context.SaveChangesAsync();
            }

            HttpContext.Session.SetString("UsuarioLogueado", usuario.strNombre);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Registrarse()
        {
            ViewBag.idSexo = new SelectList(_context.UsuCatSexo, "idSexo", "strDescripcion");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registrarse(UsuUsuario model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Mensaje = "Por favor completa todos los campos obligatorios.";
                ViewBag.idSexo = new SelectList(_context.UsuCatSexo, "idSexo", "strDescripcion", model.idSexo);
                return View(model);
            }

            bool correoExiste = await _context.UsuUsuarios.AnyAsync(u => u.strCorreo == model.strCorreo);
            if (correoExiste)
            {
                ViewBag.Mensaje = "El correo ya está registrado.";
                ViewBag.idSexo = new SelectList(_context.UsuCatSexo, "idSexo", "strDescripcion", model.idSexo);
                return View(model);
            }

            model.dtFechaDeRegistro = DateTime.Now;
            model.dtUltimaModificacion = DateTime.Now;
            model.activo = true;
            model.UsuValidacion = true;
            model.idTipoUsuario = 1;

            _context.UsuUsuarios.Add(model);
            await _context.SaveChangesAsync();

            TempData["RegistroExitoso"] = "Tu cuenta ha sido creada correctamente. Ahora puedes iniciar sesión.";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UsuarioLogueado");
            return RedirectToAction("Login");
        }
    }
}
