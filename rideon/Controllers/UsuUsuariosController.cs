using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using rideon.Data;
using rideon.Models;

namespace rideon.Controllers
{
    public class UsuUsuariosController : Controller
    {
        private readonly rideonContext _context;

        public UsuUsuariosController(rideonContext context)
        {
            _context = context;
        }

        // GET: UsuUsuarios
        public async Task<IActionResult> Index()
        {
            var usuarios = await _context.UsuUsuarios
                .Include(u => u.CatTipoUsuario)
                .Include(u => u.UsuCatSexo)
                .Include(u => u.UsuUsuarioMotos)
                    .ThenInclude(rel => rel.CatMoto)
                        .ThenInclude(m => m.CatMarcaMoto)   
                .ToListAsync();

            return View(usuarios);
        }

        // GET: UsuUsuarios/Create
        public IActionResult Create()
        {
            ViewData["idSexo"] = new SelectList(_context.UsuCatSexo, "idSexo", "strDescripcion");
            return View();
        }

        // POST: UsuUsuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsuUsuario usuUsuario, int[] SelectedMotos)
        {
            if (string.IsNullOrEmpty(usuUsuario.strContrasena))
            {
                ModelState.AddModelError("strContrasena", "La contraseña es obligatoria al crear un usuario.");
            }

            if (ModelState.IsValid)
            {
                usuUsuario.idTipoUsuario = 2;
                usuUsuario.dtFechaDeRegistro = DateTime.Now;
                usuUsuario.strFotoPerfil = "default.png";
                usuUsuario.activo = false;
                usuUsuario.UsuValidacion = false;

                _context.Add(usuUsuario);
                await _context.SaveChangesAsync();

                if (SelectedMotos != null && SelectedMotos.Any())
                {
                    foreach (var motoId in SelectedMotos)
                    {
                        _context.UsuUsuarioMotos.Add(new UsuUsuarioMoto
                        {
                            idUsuario = usuUsuario.idUsuario,
                            idMoto = motoId 
                        });
                    }
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["idSexo"] = new SelectList(_context.UsuCatSexo, "idSexo", "strDescripcion", usuUsuario.idSexo);
            return View(usuUsuario);
        }

        // GET: UsuUsuarios/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var usuario = await _context.UsuUsuarios
                .Include(u => u.UsuUsuarioMotos)
                    .ThenInclude(um => um.CatMoto)
                        .ThenInclude(m => m.CatMarcaMoto) 
                .FirstOrDefaultAsync(u => u.idUsuario == id);

            if (usuario == null)
            {
                return NotFound();
            }

            // Prepara las motos seleccionadas
            ViewBag.SelectedMotos = usuario.UsuUsuarioMotos
                .Select(m => new
                {
                    id = m.idMoto,   
                    text = m.CatMoto.CatMarcaMoto.strMarca + " " + m.CatMoto.strModelo
                })
                .ToList();

            ViewData["idSexo"] = new SelectList(_context.UsuCatSexo, "idSexo", "strDescripcion", usuario.idSexo);
            ViewData["idTipoUsuario"] = new SelectList(_context.CatTipoUsuario, "idTipoUsuario", "strDescripcion", usuario.idTipoUsuario);

            return View(usuario);
        }

        // POST: UsuUsuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UsuUsuario usuUsuario, int[] SelectedMotos)
        {
            if (id != usuUsuario.idUsuario) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var usuarioDb = await _context.UsuUsuarios
                        .Include(u => u.UsuUsuarioMotos)
                        .FirstOrDefaultAsync(u => u.idUsuario == id);

                    if (usuarioDb == null) return NotFound();
                    usuarioDb.strNombre = usuUsuario.strNombre;
                    usuarioDb.strApellidoPaterno = usuUsuario.strApellidoPaterno;
                    usuarioDb.strApellidoMaterno = usuUsuario.strApellidoMaterno;
                    usuarioDb.strCorreo = usuUsuario.strCorreo;
                    usuarioDb.strTelefono = usuUsuario.strTelefono;
                    usuarioDb.idSexo = usuUsuario.idSexo;
                    usuarioDb.idTipoUsuario = usuUsuario.idTipoUsuario;
                    usuarioDb.dtUltimaModificacion = DateTime.Now;

                    if (!string.IsNullOrEmpty(usuUsuario.strContrasena))
                    {
                        usuarioDb.strContrasena = usuUsuario.strContrasena;
                    }

                    _context.Update(usuarioDb);
                    await _context.SaveChangesAsync();
                    var existentes = _context.UsuUsuarioMotos.Where(rel => rel.idUsuario == id);
                    _context.UsuUsuarioMotos.RemoveRange(existentes);

                    if (SelectedMotos != null && SelectedMotos.Any())
                    {
                        foreach (var motoId in SelectedMotos)
                        {
                            _context.UsuUsuarioMotos.Add(new UsuUsuarioMoto
                            {
                                idUsuario = usuarioDb.idUsuario,
                                idMoto = motoId  
                            });
                        }
                    }

                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error al editar usuario: {ex.Message}");
                }
            }

            ViewData["idSexo"] = new SelectList(_context.UsuCatSexo, "idSexo", "strDescripcion", usuUsuario.idSexo);
            ViewData["idTipoUsuario"] = new SelectList(_context.CatTipoUsuario, "idTipoUsuario", "strDescripcion", usuUsuario.idTipoUsuario);

            ViewBag.SelectedMotos = _context.UsuUsuarioMotos
                .Where(x => x.idUsuario == usuUsuario.idUsuario)
                .Include(x => x.CatMoto)
                    .ThenInclude(m => m.CatMarcaMoto)  
                .Select(x => new
                {
                    id = x.idMoto,   
                    text = x.CatMoto.CatMarcaMoto.strMarca + " " + x.CatMoto.strModelo
                })
                .ToList();

            return View(usuUsuario);
        }

     
        [HttpGet]
        public async Task<IActionResult> SearchMotos(string term)
        {
            var motos = await _context.CatMoto
                .Include(m => m.CatMarcaMoto)  
                .Where(m => string.IsNullOrEmpty(term)
                         || m.CatMarcaMoto.strMarca.Contains(term)
                         || m.strModelo.Contains(term)
                         || m.strCilindraje.Contains(term))
                .Select(m => new
                {
                    idMoto = m.idMoto,   
                    text = m.CatMarcaMoto.strMarca + " " + m.strModelo + " " + m.strCilindraje
                })
                .Take(20)
                .ToListAsync();

            return Json(motos);
        }
    }
}
