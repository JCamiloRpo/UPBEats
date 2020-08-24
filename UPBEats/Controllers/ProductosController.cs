using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.CompilerServices;
using UPBEats.Data;
using UPBEats.Models;

namespace UPBEats.Controllers
{
    public class ProductosController : Controller
    {
        private readonly UPBEatsContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductosController(UPBEatsContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Productos
        public async Task<IActionResult> Index()
        {
            if (UserIsSeller())
            {
                var uPBEatsContext = _context.Producto.Include(p => p.Usuario);
                return View(await uPBEatsContext.ToListAsync());
            }

            else
            {
                return RedirectToAction("Principal", "Home");
            }
        }

        // GET: Productos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (UserIsSeller())
            {
                if (id == null)
                {
                    return NotFound();
                }

                var producto = await _context.Producto
                    .Include(p => p.Usuario)
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (producto == null)
                {
                    return NotFound();
                }

                return View(producto);
            }

            else
            {
                return RedirectToAction("Principal", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(int id, [Bind("Id,Nombre,Descripcion,Precio,FileFoto,UsuarioId")] Producto producto)
        {
            if (id != producto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (producto.FileFoto != null)
                {
                    var stream = new MemoryStream();
                    await producto.FileFoto.CopyToAsync(stream);
                    producto.Foto = stream.ToArray();
                }
                //Si no se selecciona foto, se deja la que tenia

                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.Id))
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

        // GET: Productos/Create
        public IActionResult Create()
        {
            if (UserIsSeller())
            {
                bool ingreso = HomeController.getIngreso, registro = HomeController.getRegistro;

                if (ingreso)
                {
                    ViewData["UsuarioId"] = new SelectList(_context.Usuario, "Id", "Apellido");
                    return View(new Producto());
                }

                else
                {
                    return RedirectToAction("Principal", "Home");
                }
            }

            else
            {
                return RedirectToAction("Principal", "Home");
            }
        }

        // POST: Productos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion,Precio,FileFoto,UsuarioId")] Producto producto)
        {
            // Transformamos la foto pasada por el usuario
            var stream = new MemoryStream();
            await producto.FileFoto.CopyToAsync(stream);
            producto.Foto = stream.ToArray();

            // Encontramos el usuario por su Id
            Usuario user = await _context.Usuario.FindAsync(HomeController.getIdUsuario);

            // Asignamos el Usuario al producto
            producto.Usuario = user;

            if (ModelState.IsValid)
            {
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["UsuarioId"] = new SelectList(_context.Usuario, "Id", "Apellido", producto.UsuarioId);
            return View(producto);
        }

        // GET: Productos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (UserIsSeller())
            {
                if (id == null)
                {
                    return NotFound();
                }

                var producto = await _context.Producto.FindAsync(id);
                if (producto == null)
                {
                    return NotFound();
                }
                ViewData["UsuarioId"] = new SelectList(_context.Usuario, "Id", "Apellido", producto.UsuarioId);
                return View(producto);
            }

            else
            {
                return RedirectToAction("Principal", "Home");
            }
        }

        // POST: Productos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,Precio,Foto,UsuarioId")] Producto producto)
        {
            if (id != producto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.Id))
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
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "Id", "Apellido", producto.UsuarioId);
            return View(producto);
        }

        // GET: Productos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (UserIsSeller())
            {
                if (id == null)
                {
                    return NotFound();
                }

                var producto = await _context.Producto
                    .Include(p => p.Usuario)
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (producto == null)
                {
                    return NotFound();
                }

                return View(producto);
            }

            else
            {
                return RedirectToAction("Principal", "Home");
            }    
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.Producto.FindAsync(id);
            _context.Producto.Remove(producto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoExists(int id)
        {
            return _context.Producto.Any(e => e.Id == id);
        }

        private bool UserIsSeller()
        {
            Usuario usuario = _context.Usuario.Find(HomeController.getIdUsuario);
            if (User.Identity.IsAuthenticated && usuario != null)
            {
                if (usuario.TipoRolId == 2)
                    return true;
                else
                    return false;
            }

            else
            {
                return false;
            }
        }
    }
}
