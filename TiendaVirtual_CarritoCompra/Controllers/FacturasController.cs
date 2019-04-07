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
            return View(db.Facturas.ToList());
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
