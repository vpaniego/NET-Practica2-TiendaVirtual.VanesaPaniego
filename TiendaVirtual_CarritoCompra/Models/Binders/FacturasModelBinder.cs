using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TiendaVirtual_CarritoCompra.Models.Binders
{
    public class FacturasModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpSessionStateBase session = controllerContext.HttpContext.Session;
            Facturas factura = (Facturas)session["KEY_FACTURA"];
            if (factura == null)
            {
                factura = new Facturas();
                session["KEY_FACTURA"] = factura;
            }

            return factura;

        }
    }
}