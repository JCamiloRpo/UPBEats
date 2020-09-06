﻿using System;
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
        public static List<Usuario> vendedores;
        public static List<Producto> productosVendedor;
        private static int numProductos = -1;

        public static int getNumProductos { get => numProductos; }
        public static void setNumProductos(int val)
        {
            numProductos = val;
        }

        public UsuariosController(UPBEatsContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            if (ControlIngreso())
            {
                //Codigo independiente de cada metodo
                var uPBEatsContext = _context.Usuario.Include(u => u.TipoRol);
                return View(await uPBEatsContext.ToListAsync());
            }
            //Retorno a la pagina de inicio
            return RedirectToAction("Principal", "Home");
        }

        // GET: Vendedores
        public async Task<IActionResult> ListaVendedores()
        {
            if (ControlIngreso())
            {
                if (HomeController.getUsuarioTipoRolId == 1)//Si es vendedor
                {
                    // TODO
                }
                else //Si es comprador
                {
                    var uPBEatsContext = _context.Usuario
                        .Include(p => p.TipoRol)
                        .Where(u => u.TipoRolId == 2);

                    //Lista de productos favoritos del usuario para poder personalizar la vista del boton de favorito
                    /*vendedores = _context.Usuario
                        .Include(p => p.Nombre)
                        .Include(p => p.Apellido)
                        .Where(u => u.TipoRolId == 2).ToListAsync().Result; //Solo ver mis productos favoritos*/

                    return View(await uPBEatsContext.ToListAsync());
                }
            }
            //Retorno a la pagina de inicio
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
                if (usuario.FileFoto != null)
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
            if (ControlIngreso())
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
            return RedirectToAction("Principal", "Home");
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (ControlIngreso())
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
                ViewData["Resultado"] = " ";
                return View(usuario);
            }
            return RedirectToAction("Principal", "Home");
        }

        // POST: Usuarios/Details/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(int id, [Bind("Id,Nombre,Apellido,Correo,Foto,FileFoto,TipoRolId,Emprendimiento,DescEmprendimiento")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (usuario.FileFoto != null)
                {
                    var stream = new MemoryStream();
                    await usuario.FileFoto.CopyToAsync(stream);
                    usuario.Foto = stream.ToArray();
                }
                //Si no se selecciona foto, se deja la que tenia

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
                return Details(id).Result;
            }
            return Details(id).Result;
        }

        // POST: Usuarios/DetalleVendedor/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        public async Task<IActionResult> DetalleVendedor(int? id)
        {
            if (ControlIngreso())
            {
                if (id == null)
                {
                    return NotFound();
                }

                var vendedor = await _context.Usuario
                    .Include(u => u.TipoRol)
                    .FirstOrDefaultAsync(m => m.Id == id);

                productosVendedor = _context.Producto
                    .Where(m => m.UsuarioId == id).ToList();

                NumProductos(vendedor.Id);

                if (vendedor == null)
                {
                    return NotFound();
                }
                ViewData["Resultado"] = " ";
                return View(vendedor);
            }
            return RedirectToAction("Principal", "Home");
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (ControlIngreso())
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

        /**
         * Params N/A
         * Salida true si se permite el ingreso
         * Se controla el ingreso por medio las variables booleanas que especifican si el usuario está en el sisteme y si ya está registrado
         */
        private bool ControlIngreso()
        {
            bool ingreso = HomeController.getIngreso, registro = HomeController.getRegistro;
            //Si el usuario ya ingresó y esta registrado
            if (ingreso && registro)
            {
                return true;
            }
            //Si ingresó y no se ha registrado
            else if (ingreso)
            {
                HomeController.setIngreso(false);
            }
            return false;
        }
        private void NumProductos(int userid)
        {
            var usuario = _context.Usuario.FirstOrDefault(m => m.Id == userid);
            if (usuario != null)
            {
                if (usuario.TipoRol.Nombre.Equals("Vendedor"))
                {
                    int numProductos = _context.Producto.Count(m => m.UsuarioId == userid);
                    setNumProductos(numProductos);
                }
            }
        }

    }
}
