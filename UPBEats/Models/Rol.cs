using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UPBEats.Models
{
    public class Rol
    {
        public int Id { get; set; }
        //Será 'Comprador' o 'Vendedor'
        public string Nombre { get; set; }

        public ICollection<Usuario> Usuarios { get; set; }

    }
}
