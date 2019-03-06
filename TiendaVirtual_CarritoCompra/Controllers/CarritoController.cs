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
            return View(db.Carrito.ToList());
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
        public ActionResult Add(int id)
        {
            string usuarioId = getUsuarioId();

            Carrito carrito = db.Carrito.SingleOrDefault(
          c => c.UsuarioId == usuarioId
          && c.Productos.Id == id);
            if (carrito == null)
            {
                carrito = getCarrito(id, usuarioId);
                db.Carrito.Add(carrito);
            } else
            {
                carrito.Cantidad++;                
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public Carrito getCarrito(int id, string usuarioId)
        {
            Productos producto = db.Productos.Find(id);
            return new Carrito
            {
                Productos = producto,
                FechaAlta = DateTime.Now,
                Cantidad = 1,
                UsuarioId = usuarioId
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
