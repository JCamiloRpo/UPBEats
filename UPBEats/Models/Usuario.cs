using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace UPBEats.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        //El nombre del usuario
        [Required(ErrorMessage = "Debe ingresar un nombre")]
        public string Nombre { get; set; }
        //El apellido del usuario
        [Required(ErrorMessage = "Debe ingresar un apellido")]
        public string Apellido { get; set; }
        //El correo del usuario que inició sesión
        [Required(ErrorMessage = "Debe ingresar el correo")]
        public string Correo { get; set; }
        //La foto que desea el usuario
        public byte[] Foto { get; set; }
        //Elegir un tipo de rol
        [Display(Name = "Rol")]
        [Required(ErrorMessage = "Debe seleccionar un rol")]
        public int TipoRolId { get; set; }
        //Se incluyen acá los siguientes campos para tener un mejor rendimiento evitando un inner join
        //El nombre del empredimiento/negocio si es vendedor
        public string Emprendimiento { get; set; }
        //La descripción de lo que hace el emprendimiento
        [Display(Name = "Descripción del emprendimiento")]
        public string DescEmprendimiento { get; set; }

        //Llave foranea
        public Rol TipoRol { get; set; }

        //Relacion uno a muchos
        public ICollection<Producto> Productos { get; set; }
        public ICollection<ProductoFavorito> ProductoFavoritos { get; set; }

        //Campo para la imagen de perfil
        [NotMapped]
        public IFormFile FileFoto { get; set; }

        //Propiedad de sólo lectura para obtener nombre completo del usuario
        [NotMapped]
        public string NombreCompleto
        {
            get
            {
                return Nombre + " " + Apellido;
            }
        }

        [NotMapped]
        public int NumeroProductos
        {
            get
            {
                if (Productos == null)
                {
                    return 0;
                }
                else
                {
                    return Productos.Count;
                }
            }
        }
    }
}
