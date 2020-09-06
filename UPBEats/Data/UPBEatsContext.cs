using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UPBEats.Models;

namespace UPBEats.Data
{
    public class UPBEatsContext : DbContext
    {
        public UPBEatsContext(DbContextOptions<UPBEatsContext> options):base(options)
        {
        }

        //Se agregan todos los modelos
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Rol> Rol { get; set; }
        public DbSet<Producto> Producto { get; set; }

        public DbSet<ProductoFavorito> ProductoFavorito { get; set; }

        public DbSet<VendedorFavorito> VendedorFavorito { get; set; }
    }
}
