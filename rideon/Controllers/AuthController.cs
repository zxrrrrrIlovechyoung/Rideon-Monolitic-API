using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using rideon.Data;
using rideon.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace rideon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly rideonContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(rideonContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UsuUsuario registerDto)
        {
            var existingUser = await _context.UsuUsuarios
                .AnyAsync(u => u.strCorreo == registerDto.strCorreo);

            if (existingUser)
                return BadRequest(new { message = "El correo ya está registrado" });

            // Crear usuario
            var user = new UsuUsuario
            {
                strNombre = registerDto.strNombre,
                strApellidoPaterno = registerDto.strApellidoPaterno,
                strApellidoMaterno = registerDto.strApellidoMaterno,
                strCorreo = registerDto.strCorreo,
                strContrasena = registerDto.strContrasena,
                idSexo = registerDto.idSexo,
                idTipoUsuario = registerDto.idTipoUsuario == 0 ? 2 : registerDto.idTipoUsuario,
                activo = true,
                UsuValidacion = false,
                dtFechaDeRegistro = DateTime.Now
            };

            _context.UsuUsuarios.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Usuario registrado correctamente" });
        }

 
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsuUsuario loginDto)
        {
            var user = await _context.UsuUsuarios
                .SingleOrDefaultAsync(u => u.strCorreo == loginDto.strCorreo
                                        && u.strContrasena == loginDto.strContrasena);

            if (user == null)
                return Unauthorized(new { message = "Correo o contraseña incorrectos" });
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSecret"] ?? "TU_SECRETO_SUPER_SEGURO");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.idUsuario.ToString()),
                    new Claim(ClaimTypes.Email, user.strCorreo),
                    new Claim(ClaimTypes.Name, user.strNombre ?? "")
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            return Ok(new
            {
                token = jwt,
                user = new
                {
                    user.idUsuario,
                    user.strNombre,
                    user.strApellidoPaterno,
                    user.strApellidoMaterno,
                    user.strCorreo
                }
            });
        }
    }
}
