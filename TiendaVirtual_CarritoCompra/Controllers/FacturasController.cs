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
    public class FacturasController : Controller
    {
        private TiendaVirtualCCEntities db = new TiendaVirtualCCEntities();

        // GET: Facturas
        public ActionResult Index()
        {
            string userId = HttpContext.Session["KEY_USER_ID"].ToString();

            var query = from fc in db.Facturas
                        where fc.UsuarioId == userId
                        select fc;

            List<Facturas> facturas = new List<Facturas> {
                query.FirstOrDefault<Facturas>()
            };

            HttpContext.Session["CARRITO"] = null;

            return View(facturas);
        }

        // GET: Facturas/GenerateBill/5
        public ActionResult GenerateBill(int? id)
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
            Facturas factura = new Facturas
            {
                Importe = pedidos.Total,
                UsuarioId = pedidos.UsuarioId,
                Pedido = pedidos
            };
            db.Facturas.Add(factura);
            pedidos.Facturas = factura;
            db.SaveChanges();

            return RedirectToAction("Index");
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
