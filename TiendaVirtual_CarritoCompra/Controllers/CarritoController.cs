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
            //return View(db.Carrito.ToList());            

            return View((List<Carrito>)HttpContext.Session["CARRITO"]);
        }

        // GET: Carrito/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carrito carrito = db.Carrito.Find(id);
            if (carrito == null)
            {
                return HttpNotFound();
            }
            return View(carrito);
        }

        // GET: Carrito/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Carrito/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Cantidad,UsuarioId,FechaAlta")] Carrito carrito)
        {
            if (ModelState.IsValid)
            {
                db.Carrito.Add(carrito);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(carrito);
        }

        // GET: Carrito/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carrito carrito = db.Carrito.Find(id);
            if (carrito == null)
            {
                return HttpNotFound();
            }
            return View(carrito);
        }

        // POST: Carrito/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Cantidad,UsuarioId,FechaAlta")] Carrito carrito)
        {
            if (ModelState.IsValid)
            {
                db.Entry(carrito).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(carrito);
        }

        // GET: Carrito/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carrito carrito = db.Carrito.Find(id);
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
            Carrito carrito = db.Carrito.Find(id);
            db.Carrito.Remove(carrito);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Carrito
        public ActionResult AddEF(int id)
        {
            string usuarioId = GetUsuarioId();

            Carrito carrito = db.Carrito.SingleOrDefault(
          c => c.UsuarioId == usuarioId
          && c.Productos.Id == id);
            if (carrito == null)
            {
                carrito = GetCarrito(id, usuarioId);
                db.Carrito.Add(carrito);
            } else
            {
                carrito.Cantidad++;                
            }
            db.SaveChanges();
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
            return new Carrito
            {
                Productos = db.Productos.SingleOrDefault(
           p => p.Id == id),
                FechaAlta = DateTime.Now,
                Cantidad = 1,
                UsuarioId = usuarioId
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

        private int ExisteProductoEnCarrito(int id)
        {
            HttpSessionStateBase session = HttpContext.Session;
            List<Carrito> carritoCompra = (List<Carrito>)session["CARRITO"];
            for (int i = 0; i < carritoCompra.Count; i++) {
                if (carritoCompra[i].Productos.Id.Equals(id))
                    return i;
            }                
            return -1;
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
