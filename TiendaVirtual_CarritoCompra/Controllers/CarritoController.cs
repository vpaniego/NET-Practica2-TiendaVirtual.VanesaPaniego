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
    public class CarritoController : Controller
    {
        private TiendaVirtualCCEntities db = new TiendaVirtualCCEntities();

        // GET: Carrito
        public ActionResult Index()
        {

            ViewBag.TotalProductos = SumaTotalProductosCarrito();
            return View((List<Carrito>)HttpContext.Session["CARRITO"]);
        }

        // GET: Carrito/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<Carrito> carritoCompra = (List<Carrito>)HttpContext.Session["CARRITO"];
            int index = ExisteProductoEnCarrito(id);
            Carrito carrito = carritoCompra[index];

            if (carrito == null)
            {
                return HttpNotFound();
            }
            return View(carrito);
        }

        // POST: Carrito/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            List<Carrito> carritoCompra = (List<Carrito>)HttpContext.Session["CARRITO"];
            int index = ExisteProductoEnCarrito(id);
            Carrito carrito = carritoCompra[index];
            carritoCompra.Remove(carrito);
            if (carritoCompra.Any())
            {
                HttpContext.Session["CARRITO"] = carritoCompra;
            }
            else
            {
                HttpContext.Session["CARRITO"] = null;
            }

            return RedirectToAction("Index");
        }

        // GET: Carrito
        public ActionResult Add(int id)
        {
            HttpSessionStateBase session = HttpContext.Session;

            string usuarioId = GetUsuarioId();
            Carrito carrito = GetCarrito(id, usuarioId);

            if (session["CARRITO"] == null)
            {
                List<Carrito> carritoCompra = new List<Carrito>();
                carritoCompra.Add(carrito);
                session["CARRITO"] = carritoCompra;
            }
            else
            {
                List<Carrito> carritoCompra = (List<Carrito>)session["CARRITO"];
                int index = ExisteProductoEnCarrito(id);
                if (index != -1)
                {
                    carritoCompra[index].Cantidad++;
                    carritoCompra[index].PrecioTotal = carritoCompra[index].Cantidad * carritoCompra[index].Productos.PrecioUnidad;
                }
                else
                {
                    carritoCompra.Add(carrito);
                }
                session["CARRITO"] = carritoCompra;
            }
            return RedirectToAction("Index");
        }

        public Carrito GetCarrito(int id, string usuarioId)
        {
            Random random = new Random();
            int idCarrito = random.Next(1, 1000);

            Productos producto = db.Productos.SingleOrDefault(
           p => p.Id == id);
            return new Carrito
            {
                Productos = producto,
                Id = idCarrito,
                FechaAlta = DateTime.Now,
                Cantidad = 1,
                UsuarioId = usuarioId,
                PrecioTotal = producto.PrecioUnidad * 1
            };

        }

        private string GetUsuarioId()
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

        private int ExisteProductoEnCarrito(int? id)
        {
            HttpSessionStateBase session = HttpContext.Session;
            List<Carrito> carritoCompra = (List<Carrito>)session["CARRITO"];
            for (int i = 0; i < carritoCompra.Count; i++)
            {
                if (carritoCompra[i].Productos.Id.Equals(id))
                    return i;
            }
            return -1;
        }

        private decimal SumaTotalProductosCarrito()
        {
            HttpSessionStateBase session = HttpContext.Session;
            decimal totalSuma = 0;
            List<Carrito> carritoCompra = (List<Carrito>)session["CARRITO"];
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
