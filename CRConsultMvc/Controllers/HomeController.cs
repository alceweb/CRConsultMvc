using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRConsultMvc.Controllers
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

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Networking()
        {
            ViewBag.Message = "Networking";
            return View();
        }
        public ActionResult WebPage()
        {
            ViewBag.Message = "Pagina Web";
            return View();
        }
        public ActionResult Assistenza()
        {
            ViewBag.Message = "Assistenza";
            return View();
        }
        public ActionResult Privacy()
        {
            ViewBag.Message = "Documento privacy";
            return View();
        }
    }
}