using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rideon.Data;
using rideon.Models;

namespace rideon.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiAccesosController : ControllerBase
    {
        private readonly rideonContext _context;

        public ApiAccesosController(rideonContext context)
        {
            _context = context;
        }

        // POST: api/ApiAccesos/Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UsuUsuario model)
        {
            if (string.IsNullOrWhiteSpace(model.strCorreo) || string.IsNullOrWhiteSpace(model.strContrasena))
                return BadRequest(new { success = false, message = "Correo y contraseña son requeridos" });

            var usuario = await _context.UsuUsuarios
                .FirstOrDefaultAsync(u => u.strCorreo == model.strCorreo);

            if (usuario == null || usuario.strContrasena != model.strContrasena)
                return Unauthorized(new { success = false, message = "Usuario o contraseña incorrecta" });

            if (!usuario.activo)
                return Unauthorized(new { success = false, message = "Cuenta inactiva" });
            usuario.dtUltimoAcceso = DateTime.Now;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                idUsuario = usuario.idUsuario,
                nombre = usuario.strNombre,
                correo = usuario.strCorreo,
                tipoUsuario = usuario.idTipoUsuario
            });
        }

        // POST: api/ApiAccesos/Registrarse
        [HttpPost("Registrarse")]
        public async Task<IActionResult> Registrarse([FromBody] UsuUsuario model)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(model.strCorreo) || string.IsNullOrWhiteSpace(model.strContrasena))
                return BadRequest(new { success = false, message = "Todos los campos obligatorios deben ser completados" });

            bool correoExiste = await _context.UsuUsuarios.AnyAsync(u => u.strCorreo == model.strCorreo);
            if (correoExiste)
                return Conflict(new { success = false, message = "El correo ya está registrado" });

            model.dtFechaDeRegistro = DateTime.Now;
            model.dtUltimaModificacion = DateTime.Now;
            model.activo = true;
            model.UsuValidacion = true;
            model.idTipoUsuario = 1; 
            _context.UsuUsuarios.Add(model);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Usuario registrado correctamente", idUsuario = model.idUsuario });
        }
    }
}
