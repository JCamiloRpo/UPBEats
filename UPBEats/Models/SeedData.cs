using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UPBEats.Data;

namespace UPBEats.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new UPBEatsContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<UPBEatsContext>>()))
            {
                // Buscando cualquier rol
                if (context.Rol.Any())
                    return;   // Ya se inicializó la BD

                //Roles agregar
                context.Rol.AddRange(
                    new Rol { Nombre = "Comprador" },
                    new Rol { Nombre = "Vendedor" }
                );
                context.SaveChanges();
            }
        }
    }
}
