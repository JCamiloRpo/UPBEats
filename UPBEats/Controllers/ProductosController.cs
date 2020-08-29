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
        public static List<ProductoFavorito> productoFavoritos;

        public ProductosController(UPBEatsContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Productos
        public async Task<IActionResult> Index()
        {
            if (ControlIngreso())
            {
                if (UserIsSeller())//Si es vendedor
                {
                    var uPBEatsContext = _context.Producto
                        .Include(p => p.Usuario)
                        .Where(u => u.UsuarioId == HomeController.getIdUsuario); //Solo ver mis productos publicados

                    return View(await uPBEatsContext.ToListAsync());
                }
                else //Si es comprador
                {
                    var uPBEatsContext = _context.Producto
                        .Include(p => p.Usuario);

                    //Lista de productos favoritos del usuario para poder personalizar la vista del boton de favorito
                    productoFavoritos = _context.ProductoFavorito
                        .Include(p => p.Producto)
                        .Include(p => p.Usuario)
                        .Where(u => u.UsuarioId == HomeController.getIdUsuario).ToListAsync().Result; //Solo ver mis productos favoritos

                    return View(await uPBEatsContext.ToListAsync());
                }
            }
            //Retorno a la pagina de inicio
            return RedirectToAction("Principal", "Home");
        }

        // GET: Productos/Create
        public IActionResult Create()
        {
            if (ControlIngreso())
            {
                if (UserIsSeller())
                {
                    ViewData["UsuarioId"] = new SelectList(_context.Usuario, "Id", "Apellido");
                    return View(new Producto());
                }
            }
            //Retorno a la pagina de inicio
            return RedirectToAction("Principal", "Home");
        }

        // POST: Productos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion,Precio,Foto,UsuarioId")] Producto producto)
        {

            if (ModelState.IsValid)
            {
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

        // GET: Productos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (ControlIngreso())
            {
                if (UserIsSeller())
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    var producto = await _context.Producto
                        .Include(p => p.Usuario)
                        .Where(u => u.UsuarioId == HomeController.getIdUsuario) //Solo ver mis productos
                        .FirstOrDefaultAsync(m => m.Id == id);
                    if (producto == null)
                    {
                        return NotFound();
                    }
                    return View(producto);
                }
            }
            //Retorno a la pagina de inicio
            return RedirectToAction("Principal", "Home");
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

        // GET: Productos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (ControlIngreso())
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
                ViewData["Favorito"] = ProductoFavoritoExists(Convert.ToInt32(id), HomeController.getIdUsuario);
                return View(producto);
            }
            //Retorno a la pagina de inicio
            return RedirectToAction("Principal", "Home");
        }

        // GET: Productos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (ControlIngreso())
            {
                if (UserIsSeller())
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    var producto = await _context.Producto
                        .Include(p => p.Usuario)
                        .Where(u => u.UsuarioId == HomeController.getIdUsuario) //Solo ver mis productos
                        .FirstOrDefaultAsync(m => m.Id == id);
                    if (producto == null)
                    {
                        return NotFound();
                    }

                    return View(producto);
                }
            }
            //Retorno a la pagina de inicio
            return RedirectToAction("Principal", "Home");  
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

        /**
         * Params productoId, usuarioId
         * Return N/A
         * Se realiza la consulta de que si el usuario tiene ese producto en favoritos utilizado en Deteails
         */
        private bool ProductoFavoritoExists(int productoId, int usuarioId)
        {
            return _context.ProductoFavorito.Any(m => m.ProductoId == productoId && m.UsuarioId == usuarioId);
        }

        /**
         * Params productoId, usuarioId
         * Return N/A
         * Se realiza la consulta de que si el usuario tiene ese producto en favoritos utilizado en Index desde el html
         */
        public static bool IsProductoFavorito(int productoId, int usuarioId)
        {
            return productoFavoritos.Any(m => m.ProductoId == productoId && m.UsuarioId == usuarioId);
        }

        private bool UserIsSeller()
        {
            return HomeController.getUsuarioTipoRolId == 2;
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
    }
}
