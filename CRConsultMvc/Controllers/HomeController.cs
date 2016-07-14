using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRConsultMvc.Models;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CRConsultMvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        //index.post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(EmailFormModel model)
        {
            if (ModelState.IsValid)
            {
                var body = "<p><strong>Email da:</strong> {0} ({1})</p><p><strong>Ditta: </strong>{2}</p><p><strong>Telefono:</strong> {3}</p><p><strong>Message:</strong></p><p>{4}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress("cesare@cr-consult.eu"));
                message.From = new MailAddress("webservice@cr-consult.it");
                message.Subject = "Richiesta info inviata dal sito CRConsult.eu";
                message.Body = string.Format(body, model.Nome, model.Email, model.Ditta, model.Telefono, model.Messaggio);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    await smtp.SendMailAsync(message);
                    return RedirectToAction("MailInviata");
                }
            }
            return View(model);
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
        public ActionResult MailInviata()
        {
            return View();
        }
        public ActionResult LavoriWeb()
        {
            return View();
        }
        public ActionResult Test()
        {
            return View();
        }

    }
}