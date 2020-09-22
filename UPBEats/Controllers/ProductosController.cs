using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using UPBEats.Data;
using UPBEats.Models;

namespace UPBEats.Controllers
{
    [Authorize]
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
                if (HomeController.getUsuario.TipoRolId == 2)//Si es vendedor
                {
                    var uPBEatsContext = _context.Producto
                        .Include(p => p.Usuario)
                        .Where(u => u.UsuarioId == HomeController.getUsuario.Id); //Solo ver mis productos publicados

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
                        .Where(u => u.UsuarioId == HomeController.getUsuario.Id).ToListAsync().Result; //Solo ver mis productos favoritos

                    return View(await uPBEatsContext.ToListAsync());
                }
            }
            //Retorno a la pagina de inicio
            return RedirectToAction("Principal", "Home");
        }

        public async Task<IActionResult> Busqueda()
        {
            if (ControlIngreso())
            {
                if (HomeController.getUsuario.TipoRolId == 2)//Si es vendedor
                {
                    var uPBEatsContext = _context.Producto
                        .Include(p => p.Usuario)
                        .Where(u => u.UsuarioId == HomeController.getUsuario.Id); //Solo ver mis productos publicados

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
                        .Where(u => u.UsuarioId == HomeController.getUsuario.Id).ToListAsync().Result; //Solo ver mis productos favoritos

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
                if (HomeController.getUsuario.TipoRolId == 2)
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
        public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion,Precio,Foto,UsuarioId,Disponible")] Producto producto)
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
                if (HomeController.getUsuario.TipoRolId == 2)
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    var producto = await _context.Producto
                        .Include(p => p.Usuario)
                        .Where(u => u.UsuarioId == HomeController.getUsuario.Id) //Solo ver mis productos
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,Precio,Foto,UsuarioId,Disponible")] Producto producto)
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
                ViewData["Favorito"] = ProductoFavoritoExists(Convert.ToInt32(id), HomeController.getUsuario.Id);
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
                if (HomeController.getUsuario.TipoRolId == 2)
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    var producto = await _context.Producto
                        .Include(p => p.Usuario)
                        .Where(u => u.UsuarioId == HomeController.getUsuario.Id) //Solo ver mis productos
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

        public async Task<ActionResult> Borrar(int id)
        {
            if (ControlIngreso())
            {
                if (HomeController.getUsuario.TipoRolId == 2)
                {
                    // se obtiene el usuario logueado y el producto
                    var vendedor = await _context.Usuario
                        .FirstOrDefaultAsync(m => m.Correo == User.Identity.Name);

                    var producto = await _context.Producto.FindAsync(id);
                    _context.Producto.Remove(producto);

                    // si alguno de estos dos es null devolvemos error
                    if (producto == null || vendedor == null)
                    {
                        return NotFound();
                    }

                    // verificamos que el producto a borrar pertenezca al usuario logueado
                    if (vendedor.Id == producto.UsuarioId)
                    {
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }

                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }

                }

            }

            return RedirectToAction("Index", "Home");
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
        public static bool IsProductoFavorito(int usuarioId, int productoId)
        {
            return productoFavoritos.Any(m => m.ProductoId == productoId && m.UsuarioId == usuarioId);
        }

        public static decimal precioMax(IEnumerable<Producto> productos)
        {
            decimal max = 0;
            foreach(var i in productos)
            {
                if (i.Precio > max)
                    max = i.Precio;
            }
            return max;
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
