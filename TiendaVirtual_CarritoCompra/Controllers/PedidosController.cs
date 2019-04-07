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
            return View(db.Pedidos.ToList());
        }

        // GET: Pedidos/Tramitar
        public ActionResult Tramitar()
        {
            List<CarritoCompra> carrito = (List<CarritoCompra>)HttpContext.Session["CARRITO"];

            int cantidad = carrito.Count;
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

        // GET: Pedidos/Continue/5
        public ActionResult Continue(int? id)
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
            Facturas factura = new Facturas {
                Importe = pedidos.Total,
                UsuarioId = pedidos.UsuarioId,
                Pedido = pedidos
            };
            pedidos.Facturas = factura;
            db.SaveChanges();
            return View(pedidos);
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

        private Facturas GenerarFacura(decimal importe, string userId)
        {
            return new Facturas {
                Importe = importe,
                UsuarioId = userId
            };
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
