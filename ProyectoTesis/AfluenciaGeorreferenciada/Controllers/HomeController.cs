using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AfluenciaGeorreferenciada.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Sistema de afluencia institucional georreferenciada para el soporte a la toma de decisiones.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Escuela Superior Politécnica del Litoral";

            return View();
        }

        public ActionResult Map()
        {
            ViewBag.Message = "Visualizador de densidad de personas en ESPOL.";

            return View();
        }
    }
}