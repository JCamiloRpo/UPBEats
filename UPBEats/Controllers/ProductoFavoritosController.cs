using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UPBEats.Data;
using UPBEats.Models;

namespace UPBEats.Controllers
{
    [Authorize]
    public class ProductoFavoritosController : Controller
    {
        private readonly UPBEatsContext _context;

        public ProductoFavoritosController(UPBEatsContext context)
        {
            _context = context;
        }

        // GET: ProductoFavoritos
        public async Task<IActionResult> Index()
        {
            if (ControlIngreso())
            {
                var uPBEatsContext = _context.ProductoFavorito
                .Include(p => p.Producto)
                .Include(p => p.Usuario)
                .Where(u => u.UsuarioId == HomeController.getUsuario.Id); //Solo ver mis productos favoritos

                return View(await uPBEatsContext.ToListAsync());
            }
            //Retorno a la pagina de inicio
            return RedirectToAction("Principal", "Home");
        }

        // GET: ProductoFavoritos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (ControlIngreso())
            {
                if (id == null)
                {
                    return NotFound();
                }

                var productoFavorito = await _context.ProductoFavorito
                    .Include(p => p.Producto)
                    .Include(p => p.Usuario)
                    .Where(u => u.UsuarioId == HomeController.getUsuario.Id) //Solo ver mis productos favoritos
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (productoFavorito == null)
                {
                    return NotFound();
                }

                return View(productoFavorito);
            }
            //Retorno a la pagina de inicio
            return RedirectToAction("Principal", "Home");
        }

        // GET: ProductoFavoritos/Create
        public IActionResult Create()
        {
            if (ControlIngreso())
            {
                ViewData["ProductoId"] = new SelectList(_context.Producto, "Id", "Descripcion");
                ViewData["UsuarioId"] = new SelectList(_context.Usuario, "Id", "Apellido");
                return View();
            }
            //Retorno a la pagina de inicio
            return RedirectToAction("Principal", "Home");
        }

        // POST: ProductoFavoritos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UsuarioId,ProductoId")] ProductoFavorito productoFavorito)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productoFavorito);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductoId"] = new SelectList(_context.Producto, "Id", "Descripcion", productoFavorito.ProductoId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "Id", "Apellido", productoFavorito.UsuarioId);
            return View(productoFavorito);
        }

        // GET: ProductoFavoritos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (ControlIngreso())
            {
                if (id == null)
                {
                    return NotFound();
                }

                var productoFavorito = await _context.ProductoFavorito
                    .Include(p => p.Producto)
                    .Include(p => p.Usuario)
                    .Where(u => u.UsuarioId == HomeController.getUsuario.Id) //Solo ver mis productos favoritos
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (productoFavorito == null)
                {
                    return NotFound();
                }
                ViewData["ProductoId"] = new SelectList(_context.Producto, "Id", "Nombre", productoFavorito.ProductoId);
                ViewData["UsuarioId"] = new SelectList(_context.Usuario, "Id", "Nombre", productoFavorito.UsuarioId);
                return View(productoFavorito);
            }
            //Retorno a la pagina de inicio
            return RedirectToAction("Principal", "Home");
        }

        // POST: ProductoFavoritos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UsuarioId,ProductoId")] ProductoFavorito productoFavorito)
        {
            if (id != productoFavorito.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productoFavorito);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoFavoritoExists(productoFavorito.Id))
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
            ViewData["ProductoId"] = new SelectList(_context.Producto, "Id", "Descripcion", productoFavorito.ProductoId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "Id", "Apellido", productoFavorito.UsuarioId);
            return View(productoFavorito);
        }

        // GET: ProductoFavoritos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (ControlIngreso())
            {
                if (id == null)
                {
                    return NotFound();
                }

                var productoFavorito = await _context.ProductoFavorito
                    .Include(p => p.Producto)
                    .Include(p => p.Usuario)
                    .Where(u => u.UsuarioId == HomeController.getUsuario.Id) //Solo ver mis productos favoritos
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (productoFavorito == null)
                {
                    return NotFound();
                }

                return View(productoFavorito);
            }
            //Retorno a la pagina de inicio
            return RedirectToAction("Principal", "Home");
        }

        // POST: ProductoFavoritos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productoFavorito = await _context.ProductoFavorito.FindAsync(id);
            _context.ProductoFavorito.Remove(productoFavorito);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "ProductoFavoritos");
        }

        /**
         * Params Id, UsuarioId, ProductoId - Son los datos de la tabla ProductoFavorito
         * Return La vista con la lista de todos los productos (NO deberia redirigir)
         * El metodo es llamado para agregar/eliminar de los favoritos el producto al cual se le dió click al boton respectivo
         */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Favorito([Bind("Id,UsuarioId,ProductoId")] ProductoFavorito productoFavorito)
        {
            var id = await _context.ProductoFavorito.FirstOrDefaultAsync(m => m.ProductoId == productoFavorito.ProductoId && m.UsuarioId == productoFavorito.UsuarioId);
            if (id != null)
            {
                _context.ProductoFavorito.Remove(id);
            }
            else
            {
                _context.Add(productoFavorito);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Productos");
        }

        private bool ProductoFavoritoExists(int id)
        {
            return _context.ProductoFavorito.Any(e => e.Id == id);
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
