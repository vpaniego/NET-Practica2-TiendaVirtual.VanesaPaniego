using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiendaVirtual_CarritoCompra.Models
{
    using System;
    using System.Collections.Generic;

    public class CarritoCompra
    {
        public int Id { get; set; }
        public int Cantidad { get; set; }
        public string UsuarioId { get; set; }
        public System.DateTime FechaAlta { get; set; }
        public decimal PrecioTotal { get; set; }

        public virtual Productos Productos { get; set; }
    }
}