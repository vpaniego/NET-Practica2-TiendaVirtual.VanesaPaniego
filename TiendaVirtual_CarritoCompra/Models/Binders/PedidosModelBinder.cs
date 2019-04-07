using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TiendaVirtual_CarritoCompra.Models.Binders
{
    public class PedidosModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpSessionStateBase session = controllerContext.HttpContext.Session;
            Pedidos pedido = (Pedidos)session["KEY_PEDIDO"];
            if (pedido == null)
            {
                pedido = new Pedidos();
                session["KEY_PEDIDO"] = pedido;
            }

            return pedido;
        }
    }
}