using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace UPBEats.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        //El nombre del usuario
        public string Nombre { get; set; }
        //El apellido del usuario
        public string Apellido { get; set; }
        //El correo del usuario que inició sesión
        public string Correo { get; set; }
        //La foto que desea el usuario
        public byte[] Foto { get; set; }
        //Elegir un tipo de rol
        public Rol TipoRol { get; set; }
        //Se incluyen acá los siguientes campos para tener un mejor rendimiento evitando un inner join
        //El nombre del empredimiento/negocio si es vendedor
        public string Emprendimiento { get; set; }
        //La descripción de lo que hace el emprendimiento
        public string DescEmprendimiento { get; set; }
    }
}
