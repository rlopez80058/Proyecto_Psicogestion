using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto_Psicogestion.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contacto()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Usuarios()
        {
            return View();
        }

        public ActionResult Citas()
        {
            return View();
        }

        public ActionResult Login(string Email, string Password)
        {
            return View();
        }

        public ActionResult Registro(string Email, string Password)
        {
            return View();
        }

        public ActionResult Comentarios()
        {
            return View();
        }



        public ActionResult Formulario()
        {
            return View();
        }

        public ActionResult CertificadoRegalo()
        {
            return View();
        }

        public ActionResult Expedientes()
        {
            return View();
        }

        public ActionResult Pagos()
        {
            return View();
        }

    }
}