using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UPBEats.Data;
using UPBEats.Models;

namespace UPBEats.Controllers
{
    [Authorize]
    public class UsuariosController : Controller
    {
        private readonly UPBEatsContext _context;
        private readonly IWebHostEnvironment _env;

        public UsuariosController(UPBEatsContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            bool ingreso = HomeController.getIngreso, registro = HomeController.getRegistro;
            //Si el usuario ya ingresó y esta registrado
            if (ingreso && registro)
            {
                var uPBEatsContext = _context.Usuario.Include(u => u.TipoRol);
                return View(await uPBEatsContext.ToListAsync());
            }
            //Si ingresó y no se ha registrado
            else if (ingreso)
            {
                HomeController.setIngreso(false);
            }
            return RedirectToAction("Principal", "Home");
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            bool ingreso = HomeController.getIngreso, registro = HomeController.getRegistro;
            //Si el usuario ya ingresó y esta registrado
            if (ingreso && registro)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var usuario = await _context.Usuario
                    .Include(u => u.TipoRol)
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (usuario == null)
                {
                    return NotFound();
                }
                return View(usuario);
            }
            //Si ingresó y no se ha registrado
            else if (ingreso)
            {
                HomeController.setIngreso(false);
            }
            return RedirectToAction("Principal", "Home");
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            bool ingreso = HomeController.getIngreso, registro = HomeController.getRegistro;
            //Si el usuario ya ingresó y no esta registrado
            if (ingreso && !registro)
            {
                ViewData["TipoRolId"] = new SelectList(_context.Rol, "Id", "Nombre");
                return View();
            }
            //Si ingresó y ya se ha registrado
            else if (ingreso)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Principal", "Home");
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Apellido,Correo,FileFoto,TipoRolId,Emprendimiento,DescEmprendimiento")] Usuario usuario)
        {
            
            if (ModelState.IsValid)
            {
                if(usuario.FileFoto != null)
                {
                    var stream = new MemoryStream();
                    await usuario.FileFoto.CopyToAsync(stream);
                    usuario.Foto = stream.ToArray();
                }
                //Si no se selecciona foto, se pone una por defecto
                else
                {
                    Stream foto = _env.WebRootFileProvider.GetFileInfo("images/user.png").CreateReadStream();
                    var stream = new MemoryStream();
                    await foto.CopyToAsync(stream);
                    usuario.Foto = stream.ToArray();
                }
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                HomeController.setRegistro(true);
                return RedirectToAction("Index", "Home");
            }

            ViewData["TipoRolId"] = new SelectList(_context.Rol, "Id", "Nombre", usuario.TipoRolId);
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            bool ingreso = HomeController.getIngreso, registro = HomeController.getRegistro;
            //Si el usuario ya ingresó y esta registrado
            if (ingreso && registro)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var usuario = await _context.Usuario.FindAsync(id);
                if (usuario == null)
                {
                    return NotFound();
                }
                ViewData["TipoRolId"] = new SelectList(_context.Rol, "Id", "Nombre", usuario.TipoRolId);
                return View(usuario);
            }
            //Si ingresó y no se ha registrado
            else if (ingreso)
            {
                HomeController.setIngreso(false);
            }
            return RedirectToAction("Principal", "Home");

        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Apellido,Correo,Foto,Emprendimiento,DescEmprendimiento")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
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
            ViewData["TipoRolId"] = new SelectList(_context.Rol, "Id", "Nombre", usuario.TipoRolId);
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            bool ingreso = HomeController.getIngreso, registro = HomeController.getRegistro;
            //Si el usuario ya ingresó y esta registrado
            if (ingreso && registro)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var usuario = await _context.Usuario
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (usuario == null)
                {
                    return NotFound();
                }

                return View(usuario);
            }
            //Si ingresó y no se ha registrado
            else if (ingreso)
            {
                HomeController.setIngreso(false);
            }
            return RedirectToAction("Principal", "Home");
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);
            _context.Usuario.Remove(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuario.Any(e => e.Id == id);
        }

    }
}
