using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using rideon.Data;
using rideon.Models;
using System;
using System.IO;
using System.Linq;

namespace rideon.Controllers
{
    public class AccountController : Controller
    {
        private readonly rideonContext _context;
        private readonly IEmailService _emailService;

        public AccountController(rideonContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // GET: Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(UsuUsuario usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario.strCorreo) || string.IsNullOrWhiteSpace(usuario.strContrasena))
            {
                ViewBag.Error = "Debes ingresar correo y contraseña.";
                return View(usuario);
            }

            var user = _context.UsuUsuarios
                .FirstOrDefault(u => u.strCorreo.Trim().ToLower() == usuario.strCorreo.Trim().ToLower()
                                  && u.strContrasena == usuario.strContrasena
                                  && u.activo);

            if (user != null)
            {
                HttpContext.Session.SetInt32("UsuarioId", user.idUsuario);
                HttpContext.Session.SetString("UsuarioNombre", user.strNombre);
                HttpContext.Session.SetInt32("TipoUsuario", user.idTipoUsuario);
                if (user.idTipoUsuario == 1)
                    return RedirectToAction("Index", "Home");
                else
                    return RedirectToAction("Index", "Usuario");
            }

            ViewBag.Error = "Correo o contraseña incorrectos o usuario inactivo.";
            return View(usuario);
        }

        public IActionResult Register()
        {
            var sexos = _context.UsuCatSexo.ToList();
            ViewBag.idSexo = new SelectList(sexos, "idSexo", "strDescripcion");
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegistroViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Concatenar todos los errores de validación
                var errores = string.Join("<br/>", ModelState.Values
                                                .SelectMany(v => v.Errors)
                                                .Select(e => e.ErrorMessage));

                TempData["Error"] = errores;

                var sexos = _context.UsuCatSexo.ToList();
                ViewBag.idSexo = new SelectList(sexos, "idSexo", "strDescripcion", model.idSexo);
                return View(model);
            }

            // Validar que las contraseñas coincidan
            if (model.strContrasena != model.ConfirmarContrasena)
            {
                TempData["Error"] = "Las contraseñas no coinciden.";
                var sexos = _context.UsuCatSexo.ToList();
                ViewBag.idSexo = new SelectList(sexos, "idSexo", "strDescripcion", model.idSexo);
                return View(model);
            }

            if (_context.UsuUsuarios.Any(u => u.strCorreo.Trim().ToLower() == model.strCorreo.Trim().ToLower()))
            {
                TempData["Error"] = "El correo ya está registrado.";
                var sexos = _context.UsuCatSexo.ToList();
                ViewBag.idSexo = new SelectList(sexos, "idSexo", "strDescripcion", model.idSexo);
                return View(model);
            }

            var random = new Random();
            var token = random.Next(100000, 999999).ToString();

            var usuario = new UsuUsuario
            {
                strNombre = model.strNombre,
                strApellidoPaterno = model.strApellidoPaterno,
                strApellidoMaterno = model.strApellidoMaterno,
                strCorreo = model.strCorreo,
                strContrasena = model.strContrasena,
                idSexo = model.idSexo,
                FechaNacimiento = model.FechaNacimiento,
                activo = false, // Inactivo hasta verificar
                idTipoUsuario = 2,
                dtFechaDeRegistro = DateTime.Now,
                strTokenRecuperacion = token,
                dtExpToken = DateTime.Now.AddMinutes(10)
            };

            _context.UsuUsuarios.Add(usuario);
            _context.SaveChanges();

            // Enviar correo con token
            string htmlBody = $"<h2>Verificación de correo</h2><p>Tu código de verificación es: <b>{token}</b></p><p>Expira en 10 minutos.</p>";
            _emailService.Send(usuario.strCorreo, "Verificación RideOn", htmlBody);

            return RedirectToAction("VerificarRegistro", new { correo = usuario.strCorreo });
        }




        // GET: Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult DescargarApp()
        {
            string apkPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "apk", "app.apk");
            string mimeType = "application/vnd.android.package-archive";
            return PhysicalFile(apkPath, mimeType, "RideOn.apk");
        }

        // ================= RECUPERAR CONTRASEÑA =================
        public IActionResult Recuperar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Recuperar(string correo)
        {
            var usuario = _context.UsuUsuarios.FirstOrDefault(u => u.strCorreo == correo);

            if (usuario == null)
            {
                TempData["Error"] = "El correo no está registrado.";
                return View();
            }

            // Generar token de 6 dígitos
            var random = new Random();
            var token = random.Next(100000, 999999).ToString();

            usuario.strTokenRecuperacion = token;
            usuario.dtExpToken = DateTime.Now.AddMinutes(10);
            _context.SaveChanges();

            // Enviar correo
            string htmlBody = $"<h2>Tu código de recuperación</h2><p>Código: <b>{token}</b></p><p>Expira en 10 minutos.</p>";
            _emailService.Send(usuario.strCorreo, "Recuperación RideOn", htmlBody);

            TempData["OK"] = "Te enviamos un código a tu correo.";
            return RedirectToAction("VerificarCodigo", new { correo = usuario.strCorreo });
        }

        public IActionResult VerificarCodigo(string correo)
        {
            ViewBag.Correo = correo;
            return View();
        }

        [HttpPost]
        public IActionResult VerificarCodigo(string correo, string token)
        {
            var usuario = _context.UsuUsuarios
                .FirstOrDefault(u => u.strCorreo == correo && u.strTokenRecuperacion == token);

            if (usuario == null || usuario.dtExpToken < DateTime.Now)
            {
                TempData["Error"] = "Código incorrecto o expirado.";
                return RedirectToAction("VerificarCodigo", new { correo });
            }

            return RedirectToAction("Restablecer", new { correo });
        }

        public IActionResult Restablecer(string correo)
        {
            if (string.IsNullOrEmpty(correo))
                return RedirectToAction("Login");

            var model = new RestablecerViewModel { Correo = correo };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Restablecer(RestablecerViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var usuario = _context.UsuUsuarios.FirstOrDefault(u => u.strCorreo == model.Correo);
            if (usuario == null)
                return Content("Error: usuario no encontrado.");

            usuario.strContrasena = model.NuevaContrasena;
            usuario.strTokenRecuperacion = null;
            usuario.dtExpToken = null;

            _context.SaveChanges();

            TempData["Mensaje"] = "Tu contraseña ha sido actualizada.";
            return RedirectToAction("Login");
        }

        public IActionResult VerificarRegistro(string correo)
        {
            ViewBag.Correo = correo;
            return View();
        }

        [HttpPost]
        public IActionResult VerificarRegistro(string correo, string token)
        {
            var usuario = _context.UsuUsuarios.FirstOrDefault(u => u.strCorreo == correo && u.strTokenRecuperacion == token);

            if (usuario == null || usuario.dtExpToken < DateTime.Now)
            {
                TempData["Error"] = "Código incorrecto o expirado.";
                return RedirectToAction("VerificarRegistro", new { correo });
            }

            usuario.activo = true;
            usuario.strTokenRecuperacion = null;
            usuario.dtExpToken = null;
            _context.SaveChanges();

            TempData["Mensaje"] = "Correo verificado correctamente. Ahora puedes iniciar sesión.";
            return RedirectToAction("Login");
        }


    }
}
