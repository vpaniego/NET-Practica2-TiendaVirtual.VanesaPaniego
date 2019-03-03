﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TiendaVirtual_CarritoCompra.Models.Binders
{
    public class CarritoCompraModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpSessionStateBase session = controllerContext.HttpContext.Session;
            CarritoCompra carrito = (CarritoCompra)session["KEY_CARRITO"];
            if(carrito == null)
            {
                carrito = new CarritoCompra();
                session["KEY_CARRITO"] = carrito;
            }

            return carrito;
        }
    }
}