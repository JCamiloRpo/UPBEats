using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using UPBEats.Controllers;

namespace UPBEats.Models
{
    public class Producto : IValidatableObject
    {
        public int Id { get; set; }
        //El nombre del producto
        [Required(ErrorMessage = "Debe ingresar un nombre")]
        public string Nombre { get; set; }
        //La descripción del producto
        [Required(ErrorMessage = "Debe ingresar una descripción")]
        public string Descripcion { get; set; }
        //El precio del producto
        [Required(ErrorMessage = "Debe ingresar un precio")]
        [Column(TypeName = "money")]
        //[Range(0, 1000000, ErrorMessage = "El precio debe ser positivo")]
        public decimal Precio { get; set; }
        //Por el momento una foto por producto
        [Required(ErrorMessage = "Debe ingresar una foto caracteristica")]
        public byte[] Foto { get; set; }
        //El vendedor que publica el producto. Llave foranea
        [Required]
        public int UsuarioId { get; set; }

        //Activar y desactivar la disponibilidad del producto
        public bool Disponible { get; set; }

        //Relación con la tabla
        public Usuario Usuario { get; set; }
        //Campo para la imagen
        [NotMapped]
        public IFormFile FileFoto { get; set; }

        //Relacion uno a muchos
        public ICollection<ProductoFavorito> ProductoFavoritos { get; set; }

        //Reglas de validacion

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.Precio <= 0)
                yield return new ValidationResult("El precio debe ser positivo");

            if (HomeController.getUsuario.TipoRolId != 2)
                yield return new ValidationResult("Solo los vendedores pueden publicar productos");
        }
    }
}
