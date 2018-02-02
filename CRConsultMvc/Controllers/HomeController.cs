using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRConsultMvc.Models;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;

namespace CRConsultMvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Index1()
        {
            return View();
        }

        //index.post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index1(EmailFormModel model)
        {
            if (ModelState.IsValid)
            {
                var body = "<p><strong>Email da:</strong> {0} ({1})</p><p><strong>Ditta: </strong>{2}</p><p><strong>Telefono:</strong> {3}</p><p><strong>Message:</strong></p><p>{4}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress("cesare@cr-consult.eu"));
                message.From = new MailAddress("webservice@cr-consult.it");
                message.Subject = "Richiesta info " + model.settore + " inviata dal sito CRConsult.eu";
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

        public ActionResult Stat()
        {
            return View();
        }

        public ActionResult CompletaRegistrazione()
        {
            ViewBag.Uid = User.Identity.GetUserId();
            return View();
        }

        public ActionResult FormEsperto()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> FormEsperto(EmailEsperto esperto)
        {
                //Validate Google recaptcha here
                var response = Request["g-recaptcha-response"];
                string secretKey = "6Lfl-0MUAAAAAHRpIqwh_zXLB1jnN6MUPPcE4mKq";
                var client = new WebClient();
                var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));
                var obj = JObject.Parse(result);
                var status = (bool)obj.SelectToken("success");
                ViewBag.Message = status ? "Recaptcha validato con successo" : "Devi dimostrare di non essere un robot";
            if (ModelState.IsValid&& status)
            {

                //Invio la mail a crconsult
                MailMessage message = new MailMessage(
                    "webservice@cr-consult.it",
                    "cesare@cr-consult.eu",
                    "Richiesta informazioni dal sito cr-consult.eu",
                    "Il giorno " + DateTime.Now + "<br/><strong>" +
                    esperto.Nome + " <em> " + esperto.Ditta + " </em><br/>Tel." + esperto.Telefono + " [" + esperto.Email + "] " + "</strong> " +
                    "<br/> ha inviato una richiesta di informazioni dal sito www.cr-consult.eu<hr/><strong>" +
                    esperto.Richiesta +
                    "</strong>"
                    );
                message.IsBodyHtml = true;
                using (var smtp = new SmtpClient())
                {
                    await smtp.SendMailAsync(message);
                }
                //Invio la mail al mittente
                MailMessage message1 = new MailMessage(
                    "webservice@cr-consult.it",
                    esperto.Email ,
                    "La tua richiesta a www.cr-consult.eu",
                    "Gentile cliente, abbiamo ricevuto la tua richiesta dal sito cr-consult.eu.<hr/>" +
                    "Data: <strong>" + DateTime.Now + "</strong><br/>" +
                    "Nome contatto: <strong>" + esperto.Nome + " <em>" + esperto.Ditta + " </em></strong><br/>Telefono: <strong>" + esperto.Telefono + "</strong><br/>Mail: <strong>[" + esperto.Email + "]</strong> <br/>" + 
                    "Motivo della richiesta: <br/><strong>" +
                    esperto.Richiesta +
                    "</strong><hr/>Verrai contattato al più presto da un nostro incaricato.<br/>Ti ricordo che questa richiesta di intervento non comporta nessun impegno economico.<br>" +
                    "Se, dopo l'analisi del nostro esperto, saranno ritenuti necessari degli interventi a pagamento, ti verrà presentato un preventivo di spesa.<br/>"
                    );
                message1.IsBodyHtml = true;
                using (var smtp = new SmtpClient())
                {
                    await smtp.SendMailAsync(message1);
                }
                return RedirectToAction("FormOk", "Home");
            }
            return View(esperto);
        }

        public ActionResult FormOk()
        {
            return View();
        }

    }
}