using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TiendaVirtual_CarritoCompra.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Sancho callaba y comía bellotas, y visitaba muy a menudo el segundo zaque(odre pequeño) que, porque se enfriase el vino, le tenía colgado de un alcornoque.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Puede contactar con nosotros en la dirección:";

            return View();
        }
    }
}