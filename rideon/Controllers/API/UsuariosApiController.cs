using Microsoft.AspNetCore.Mvc;
using rideon.Data;
using rideon.Models;
using System;
using System.Linq;

namespace rideon.Controllers.Api

{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosApiController : ControllerBase
    {
        private readonly rideonContext _context;
        private readonly IEmailService _emailService; 

        public UsuariosApiController(rideonContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpGet("ping")]
        public IActionResult Ping() => Ok("API viva");

        [HttpPost("register")]
        public IActionResult Register([FromBody] UsuUsuario usuario)
        {
            if (_context.UsuUsuarios.Any(u => u.strCorreo == usuario.strCorreo))
                return BadRequest(new { success = false, message = "Correo ya registrado" });

            usuario.dtFechaDeRegistro = DateTime.Now;
            usuario.activo = true;

            _context.UsuUsuarios.Add(usuario);
            _context.SaveChanges();

            return Ok(new { success = true, message = "Usuario registrado correctamente" });
        }

        // POST: api/UsuariosApi/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var usuario = _context.UsuUsuarios
                .FirstOrDefault(u => u.strCorreo == request.Correo && u.strContrasena == request.Contrasena && u.activo);

            if (usuario == null)
                return Unauthorized(new { success = false, message = "Correo o contraseña incorrectos" });

            usuario.dtUltimoAcceso = DateTime.Now;
            _context.SaveChanges();

            return Ok(new { success = true, usuarioId = usuario.idUsuario, nombre = usuario.strNombre });
        }

        // POST: api/UsuariosApi/forgot-password
        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var usuario = _context.UsuUsuarios.FirstOrDefault(u => u.strCorreo == request.Correo);
            if (usuario == null)
                return NotFound(new { success = false, message = "Correo no registrado" });

            var random = new Random();
            var token = random.Next(100000, 999999).ToString();

            usuario.strTokenRecuperacion = token;
            usuario.dtExpToken = DateTime.Now.AddMinutes(10);
            _context.SaveChanges();

            string usuarioNombre = usuario.strNombre ?? "Usuario";
            string tokenHtml = $"<b style='color:#ff4d4d;'>{token}</b>";

            string logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/logo.jpg");
            if (!System.IO.File.Exists(logoPath))
                throw new FileNotFoundException("No se encontró el logo en la ruta: " + logoPath);
            string logoBase64 = Convert.ToBase64String(System.IO.File.ReadAllBytes(logoPath));
            string htmlBody = $@"
<html>
<head>
  <style>
    body {{
      font-family: Arial, sans-serif;
      background-color: #f4f4f4;
      color: #333333;
      margin: 0;
      padding: 0;
    }}
    .container {{
      max-width: 600px;
      margin: 20px auto;
      background-color: #ffffff;
      padding: 20px;
      border-radius: 8px;
      box-shadow: 0 2px 5px rgba(0,0,0,0.1);
    }}
    .header {{
      text-align: center;
      padding-bottom: 20px;
    }}
    .content {{
      font-size: 16px;
      line-height: 1.5;
    }}
    .footer {{
      font-size: 12px;
      color: #999999;
      text-align: center;
      margin-top: 20px;
    }}
  </style>
</head>
<body>
  <div class='container'>
    <div class='header'>
      <img src='data:image/jpeg;base64,{logoBase64}' alt='RideOn Logo' style='width:100px;'/>
      <h2>Recuperación de contraseña</h2>
    </div>
    <div class='content'>
      <p>Hola {usuarioNombre},</p>
      <p>Hemos recibido una solicitud para restablecer tu contraseña en <b>RideOn</b>.</p>
      <p>Tu código de recuperación es:</p>
      <p style='text-align:center; font-size:24px; color:#ff4d4d;'><b>{token}</b></p>
      <p>Este código expira en <b>10 minutos</b>.</p>
      <p>Si no solicitaste este cambio, puedes ignorar este correo.</p>
    </div>
    <div class='footer'>
      <p>© 2025 RideOn. Todos los derechos reservados.</p>
      <p>Este correo contiene información confidencial y es para uso exclusivo del destinatario.</p>
    </div>
  </div>
</body>
</html>
";
            _emailService.Send(
                usuario.strCorreo,
                "Recuperación de contraseña - RideOn",
                htmlBody
            );

            return Ok(new { success = true, message = "Código enviado al correo" });
        }



        // POST: api/UsuariosApi/reset-password
        [HttpPost("reset-password")]
        public IActionResult ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var usuario = _context.UsuUsuarios
                .FirstOrDefault(u => u.strCorreo == request.Correo && u.strTokenRecuperacion == request.Token);

            if (usuario == null || usuario.dtExpToken < DateTime.Now)
                return BadRequest(new { success = false, message = "Token inválido o expirado" });

            usuario.strContrasena = request.NuevaContrasena;
            usuario.strTokenRecuperacion = null;
            usuario.dtExpToken = null;
            _context.SaveChanges();

            return Ok(new { success = true, message = "Contraseña actualizada correctamente" });
        }
        [HttpGet("getSexos")]
        public IActionResult GetSexo()
        {
            var sexos = _context.UsuCatSexo
                .Select(s => new { s.idSexo, s.strDescripcion }) 
                .ToList();

            return Ok(sexos);
        }
        [HttpGet("getUsuarios")]
        public IActionResult GetUsuarios()
        {
            var usuarios = _context.UsuUsuarios
                .Select(u => new {
                    u.idUsuario,
                    u.strNombre,
                    u.strApellidoPaterno,
                    u.strApellidoMaterno,
                    u.strCorreo,
                    u.idSexo,
                    u.activo,
                    u.dtFechaDeRegistro
                })
                .ToList();

            return Ok(usuarios);
        }



    }

    // Clases auxiliares para requests
    public class LoginRequest
    {
        public string Correo { get; set; }
        public string Contrasena { get; set; }
    }

    public class ForgotPasswordRequest
    {
        public string Correo { get; set; }
    }

    public class ResetPasswordRequest
    {
        public string Correo { get; set; }
        public string Token { get; set; }
        public string NuevaContrasena { get; set; }
    }

}
