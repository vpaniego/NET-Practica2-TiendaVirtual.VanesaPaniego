using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiendaVirtual_CarritoCompra.Models
{
    public class CarritoCompra : List<ArticuloCarrito>
    {
        public int Id { get; set; }
    }
}