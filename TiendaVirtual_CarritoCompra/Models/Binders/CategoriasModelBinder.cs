using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TiendaVirtual_CarritoCompra.Models.Binders
{
    public class CategoriasModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpSessionStateBase session = controllerContext.HttpContext.Session;
            Categorias categoria = (Categorias) session["KEY_CATEGORIA"];
            if(categoria == null)
            {
                categoria = new Categorias();
                session["KEY_CATEGORIA"] = categoria;
            }

            return categoria;
        }
    }
}