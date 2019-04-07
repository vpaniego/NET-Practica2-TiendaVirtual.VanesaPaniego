using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TiendaVirtual_CarritoCompra.Models;

namespace TiendaVirtual_CarritoCompra.Controllers
{
    public class PedidosController : Controller
    {
        private TiendaVirtualCCEntities db = new TiendaVirtualCCEntities();

        // GET: Pedidos
        public ActionResult Index()
        {
            string userId = HttpContext.Session["KEY_USER_ID"].ToString();
            var query = from p in db.Pedidos
                            where p.UsuarioId == userId
                            && p.Facturas == null
                            select p;

            List<Pedidos> pedidos = new List<Pedidos> {
                query.FirstOrDefault<Pedidos>()
            };

            if(pedidos!=null && !pedidos.Contains(null))
            {
                return View(pedidos);
            } else
            {
                return View(new List<Pedidos>());
            }            
        }

        // GET: Pedidos/Tramitar
        public ActionResult Tramitar()
        {
            List<CarritoCompra> carrito = (List<CarritoCompra>)HttpContext.Session["CARRITO"];

            int cantidad = SumaTotalCantidadCarrito(carrito);
            decimal totalProductos = SumaTotalProductosCarrito(carrito);
            string userId = HttpContext.Session["KEY_USER_ID"].ToString();

            Pedidos pedido = new Pedidos
            {
                Cantidad = cantidad,
                Fecha = DateTime.Now,
                Total = totalProductos,
                UsuarioId = userId
            };

            db.Pedidos.Add(pedido);
            db.SaveChanges();

            return RedirectToAction("Index");
        } 

        // GET: Pedidos/Cancel/5
        public ActionResult Cancel(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedidos pedidos = db.Pedidos.Find(id);
            if (pedidos == null)
            {
                return HttpNotFound();
            }
            return View(pedidos);
        }

        // POST: Pedidos/Cancel/5
        [HttpPost, ActionName("Cancel")]
        [ValidateAntiForgeryToken]
        public ActionResult CancelConfirmed(int id)
        {            
            Pedidos pedidos = db.Pedidos.Find(id);
            db.Pedidos.Remove(pedidos);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private decimal SumaTotalProductosCarrito(List<CarritoCompra> carritoCompra)
        {            
            decimal totalSuma = 0;            
            for (int i = 0; carritoCompra != null && i < carritoCompra.Count; i++)
            {
                totalSuma = totalSuma + carritoCompra[i].PrecioTotal;
            }
            return totalSuma;
        }

        private int SumaTotalCantidadCarrito(List<CarritoCompra> carritoCompra)
        {
            int totalCantidad = 0;
            for (int i = 0; carritoCompra != null && i < carritoCompra.Count; i++)
            {
                totalCantidad = totalCantidad + carritoCompra[i].Cantidad;
            }
            return totalCantidad;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
