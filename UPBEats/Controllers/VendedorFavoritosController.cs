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
    public class VendedorFavoritosController : Controller
    {
        private readonly UPBEatsContext _context;

        public VendedorFavoritosController(UPBEatsContext context)
        {
            _context = context;
        }

        // GET: VendedorFavoritos
        public async Task<IActionResult> Index()
        {
            var uPBEatsContext = _context.VendedorFavorito.Include(v => v.Comprador).Include(v => v.Vendedor);
            return View(await uPBEatsContext.ToListAsync());
        }

        // GET: VendedorFavoritos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendedorFavorito = await _context.VendedorFavorito
                .Include(v => v.Comprador)
                .Include(v => v.Vendedor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vendedorFavorito == null)
            {
                return NotFound();
            }

            return View(vendedorFavorito);
        }

        // GET: VendedorFavoritos/Create
        public IActionResult Create()
        {
            ViewData["CompradorId"] = new SelectList(_context.Usuario, "Id", "Apellido");
            ViewData["VendedorId"] = new SelectList(_context.Usuario, "Id", "Apellido");
            return View();
        }

        // POST: VendedorFavoritos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CompradorId,VendedorId")] VendedorFavorito vendedorFavorito)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vendedorFavorito);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompradorId"] = new SelectList(_context.Usuario, "Id", "Apellido", vendedorFavorito.CompradorId);
            ViewData["VendedorId"] = new SelectList(_context.Usuario, "Id", "Apellido", vendedorFavorito.VendedorId);
            return View(vendedorFavorito);
        }

        // GET: VendedorFavoritos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendedorFavorito = await _context.VendedorFavorito.FindAsync(id);
            if (vendedorFavorito == null)
            {
                return NotFound();
            }
            ViewData["CompradorId"] = new SelectList(_context.Usuario, "Id", "Apellido", vendedorFavorito.CompradorId);
            ViewData["VendedorId"] = new SelectList(_context.Usuario, "Id", "Apellido", vendedorFavorito.VendedorId);
            return View(vendedorFavorito);
        }

        // POST: VendedorFavoritos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CompradorId,VendedorId")] VendedorFavorito vendedorFavorito)
        {
            if (id != vendedorFavorito.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vendedorFavorito);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VendedorFavoritoExists(vendedorFavorito.Id))
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
            ViewData["CompradorId"] = new SelectList(_context.Usuario, "Id", "Apellido", vendedorFavorito.CompradorId);
            ViewData["VendedorId"] = new SelectList(_context.Usuario, "Id", "Apellido", vendedorFavorito.VendedorId);
            return View(vendedorFavorito);
        }

        // GET: VendedorFavoritos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendedorFavorito = await _context.VendedorFavorito
                .Include(v => v.Comprador)
                .Include(v => v.Vendedor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vendedorFavorito == null)
            {
                return NotFound();
            }

            return View(vendedorFavorito);
        }

        // POST: VendedorFavoritos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vendedorFavorito = await _context.VendedorFavorito.FindAsync(id);
            _context.VendedorFavorito.Remove(vendedorFavorito);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VendedorFavoritoExists(int id)
        {
            return _context.VendedorFavorito.Any(e => e.Id == id);
        }

        /**
         * Params Id, CompradorId, VendedorId - Son los datos de la tabla ProductoFavorito
         * El metodo es llamado para agregar/eliminar de los favoritos el vendedor al cual se le dió click
         */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public void Favorito([Bind("Id,CompradorId,VendedorId")] VendedorFavorito vendedorFavorito)
        {
            var id = _context.VendedorFavorito.FirstOrDefaultAsync(m => m.VendedorId == vendedorFavorito.VendedorId && m.CompradorId == vendedorFavorito.CompradorId).Result;
            if (id != null)
            {
                _context.VendedorFavorito.Remove(id);
            }
            else
            {
                _context.Add(vendedorFavorito);
            }
            _context.SaveChanges();
        }
    }
}
