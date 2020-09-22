using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UPBEats.Controllers;

namespace UPBEats.Models
{
    public class ProductoFavorito : IValidatableObject
    {
        public int Id { get; set; }

        //El Comprador que agrega producto favorito. Llave foranea
        [Required]
        public int UsuarioId { get; set; }

        //El produto que se agrega a favorito. Llave foranea
        [Required]
        public int ProductoId { get; set; }

        //Relación con la tabla Usuario
        public Usuario Usuario { get; set; }

        //Relación con la tabla Producto
        public Producto Producto { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (HomeController.getUsuario.TipoRolId != 1)
                yield return new ValidationResult("Solo los compradores pueden agregar/tener favoritos");
        }
    }
}
