using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TiendaVirtual_CarritoCompra.Models.Binders
{
    public class ProductosModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpSessionStateBase session = controllerContext.HttpContext.Session;
            Productos producto = (Productos)session["KEY_PRODUCTO"];
            if(producto == null)
            {
                producto = new Productos();
                session["KEY_PRODUCTO"] = producto;
            }

            return producto;
        }
    }
}