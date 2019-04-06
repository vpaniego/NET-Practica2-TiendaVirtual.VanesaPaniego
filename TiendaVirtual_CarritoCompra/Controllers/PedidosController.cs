﻿using System;
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
            decimal total = SumaTotalProductosCarrito(carrito);

            Pedidos pedido = new Pedidos
            {
                Cantidad = cantidad,
                Fecha = DateTime.Now,
                Total = ViewBag.TotalProductos,

            };

            return RedirectToAction("Index");
        }

        // GET: Pedidos/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Pedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
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