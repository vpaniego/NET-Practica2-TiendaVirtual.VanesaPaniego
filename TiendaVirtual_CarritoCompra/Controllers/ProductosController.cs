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
    public class ProductosController : Controller
    {
        private TiendaVirtualCCEntities db = new TiendaVirtualCCEntities();

        // GET: Productos
        public ActionResult Index()
        {
            return View(db.Productos.ToList());
        }

        // GET: Productos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Productos productos = db.Productos.Find(id);
            if (productos == null)
            {
                return HttpNotFound();
            }
            return View(productos);
        }

        // GET: Productos/Create
        [Authorize(Users = "admin1@correo.es")]
        public ActionResult Create()
        {            
            SelectList lstCategoriasSelectList = new SelectList(db.Categorias, "Id", "Nombre");

            Productos producto = new Productos
            {
                SelectListCategorias = lstCategoriasSelectList
            };

            return View(producto);
        }
        
        // POST: Productos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "admin1@correo.es")]
        public ActionResult Create([Bind(Include = "Id,Nombre,Descripcion,PathImagen,ImageFile,PrecioUnidad,SelectedIdCategoria")] Productos productos)
        {
            if (ModelState.IsValid)
            {
                productos.Categoria = db.Categorias.Find(productos.SelectedIdCategoria);
                db.Productos.Add(productos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productos);
        }

        // GET: Productos/Edit/5
        [Authorize(Users = "admin1@correo.es")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Productos productos = db.Productos.Find(id);
            if (productos == null)
            {
                return HttpNotFound();
            }
            return View(productos);
        }

        // POST: Productos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "admin1@correo.es")]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Descripcion,PathImagen,PrecioUnidad")] Productos productos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productos);
        }

        // GET: Productos/Delete/5
        [Authorize(Users = "admin1@correo.es")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Productos productos = db.Productos.Find(id);
            if (productos == null)
            {
                return HttpNotFound();
            }
            return View(productos);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "admin1@correo.es")]
        public ActionResult DeleteConfirmed(int id)
        {
            Productos productos = db.Productos.Find(id);
            db.Productos.Remove(productos);
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
