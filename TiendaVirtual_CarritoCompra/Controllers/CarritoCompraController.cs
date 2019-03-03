using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaVirtual_CarritoCompra.Models;

namespace TiendaVirtual_CarritoCompra.Controllers
{
    public class CarritoCompraController : Controller
    {
        private TiendaVirtualCCEntities db = new TiendaVirtualCCEntities();        

        // GET: CarritoCompra
        public ActionResult Index()
        {
            return View(db.CarritoCompras.Find());
        }

        // GET: CarritoCompra
        public ActionResult Add(int id, CarritoCompra carrito)
        {              
            ArticuloCarrito articulo = getArticuloCarrito(id);
            carrito.Add(articulo);

            return View("Index", carrito);
        }

        public ArticuloCarrito getArticuloCarrito(int id)
        {
            Productos producto =  db.Productos.Find(id);
            return new ArticuloCarrito
            {
                Productos = producto,
                FechaAlta = DateTime.Now,
                Cantidad = 1,
                UsuarioId = getUsuarioId()
            };
            
        }

        private string getUsuarioId()
        {
            HttpSessionStateBase session = HttpContext.Session;
            if (session["KEY_USER_ID"] == null)
            {
                if (!string.IsNullOrWhiteSpace(User.Identity.Name))
                {
                    session["KEY_USER_ID"] = User.Identity.Name;
                }
                else
                {
                    // Generate a new random GUID using System.Guid class.     
                    Guid tempCartId = Guid.NewGuid();
                    session["KEY_USER_ID"] = tempCartId.ToString();
                }
            }
            return session["KEY_USER_ID"].ToString();
        }
    }
}