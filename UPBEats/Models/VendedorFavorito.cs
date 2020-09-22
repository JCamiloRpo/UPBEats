using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UPBEats.Controllers;

namespace UPBEats.Models
{
    public class VendedorFavorito
    {
        public int Id { get; set; }

        //El Comprador que agrega comprador favorito. Llave foranea
        [Required]
        public int CompradorId { get; set; }

        //El vendedor que se agrega a favorito. Llave foranea
        [Required]
        public int VendedorId { get; set; }

        //Relación con la tabla Usuario
        public Usuario Comprador { get; set; }

        //Relación con la tabla Usuario
        public Usuario Vendedor { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (HomeController.getUsuario.TipoRolId != 1)
                yield return new ValidationResult("Solo los compradores pueden agregar/tener favoritos");
        }
    }
}
