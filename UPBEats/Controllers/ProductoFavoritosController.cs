using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UPBEats.Data;
using UPBEats.Models;

namespace UPBEats.Controllers
{
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
            var uPBEatsContext = _context.ProductoFavorito.Include(p => p.Producto).Include(p => p.Usuario);
            return View(await uPBEatsContext.ToListAsync());
        }

        // GET: ProductoFavoritos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productoFavorito = await _context.ProductoFavorito
                .Include(p => p.Producto)
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productoFavorito == null)
            {
                return NotFound();
            }

            return View(productoFavorito);
        }

        // GET: ProductoFavoritos/Create
        public IActionResult Create()
        {
            ViewData["ProductoId"] = new SelectList(_context.Producto, "Id", "Descripcion");
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "Id", "Apellido");
            return View();
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
            if (id == null)
            {
                return NotFound();
            }

            var productoFavorito = await _context.ProductoFavorito.FindAsync(id);
            if (productoFavorito == null)
            {
                return NotFound();
            }
            ViewData["ProductoId"] = new SelectList(_context.Producto, "Id", "Descripcion", productoFavorito.ProductoId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "Id", "Apellido", productoFavorito.UsuarioId);
            return View(productoFavorito);
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
            if (id == null)
            {
                return NotFound();
            }

            var productoFavorito = await _context.ProductoFavorito
                .Include(p => p.Producto)
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productoFavorito == null)
            {
                return NotFound();
            }

            return View(productoFavorito);
        }

        // POST: ProductoFavoritos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productoFavorito = await _context.ProductoFavorito.FindAsync(id);
            _context.ProductoFavorito.Remove(productoFavorito);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoFavoritoExists(int id)
        {
            return _context.ProductoFavorito.Any(e => e.Id == id);
        }
    }
}
