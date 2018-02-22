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
            if (ModelState.IsValid)
            {

                //Invio la mail a crconsult
                //MailMessage message = new MailMessage(
                //    "webservice@cr-consult.it",
                //    esperto.Email,
                //    "Abbiamo ricevuto la tua richiesta",
                //    "Gentile cliente, abbiamo registrato la tua richiesta: <hr/>" +
                //    "Data: <strong>" + DateTime.Now + "</strong><br/>" +
                //    "Nome contatto: <strong>" + esperto.Nome + " <em>" + esperto.Ditta + " </em></strong><br/>Telefono: <strong>" + esperto.Telefono + "</strong><br/>Mail: <strong>[" + esperto.Email + "]</strong> <br/>" +
                //    "Motivo della richiesta: <br/><strong>" +
                //    esperto.Richiesta +
                //    "</strong><hr/>Verrai contattato al più presto da un nostro incaricato.<br/><br/>Cordiali saluti<br/>WebService C.R.Consult"
                //    );
                var body = "La tua richiesta è stata registrata.<hr><ul><li>"
                    + esperto.Nome + "</li><li>"
                    + esperto.Telefono + "</li><li>"
                    + esperto.Email + "</li></ul><br/><strong><em>"
                    + esperto.Richiesta + "</strong></em>";
                var message = new MailMessage();
                message.To.Add(new MailAddress(esperto.Email));
                message.From = new MailAddress("webservice@cr-consult.it");
                message.Subject = "Contatta l'esperto";
                message.Body = string.Format(body, esperto.Nome, esperto.Email, esperto.Telefono , esperto.Richiesta);
                message.IsBodyHtml = true;
                MailAddress bcc = new MailAddress("cesare@cr-consult.eu");
                message.Bcc.Add(bcc);
                
                using (var smtp = new SmtpClient())
                {
                    await smtp.SendMailAsync(message);
                }
                return RedirectToAction("FormOk", "Home");
            }
            return View(esperto);
        }

        public ActionResult FormContratto()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> FormContratto(EmailContratto contratto)
        {
            //Validate Google recaptcha here
            var response = Request["g-recaptcha-response"];
            string secretKey = "6Lfl-0MUAAAAAHRpIqwh_zXLB1jnN6MUPPcE4mKq";
            var client = new WebClient();
            var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));
            var obj = JObject.Parse(result);
            var status = (bool)obj.SelectToken("success");
            ViewBag.Message = status ? "Recaptcha validato con successo" : "Devi dimostrare di non essere un robot";
            if (ModelState.IsValid && status)
            {

                //Invio la mail a crconsult
                MailMessage message = new MailMessage(
                    "webservice@cr-consult.it",
                    "cesare@cr-consult.eu",
                    "Richiesta informazioni per attivazione contratto assistenza dal sito cr-consult.eu",
                    "Il giorno " + DateTime.Now + "<br/><strong>" +
                    contratto.Nome + " <em> " + contratto.Ditta + " </em><br/>Tel." + contratto.Telefono + " [" + contratto.Email + "] " + "</strong> " +
                    "<br/> ha inviato una richiesta di informazioni per attivazione cntratto di assistenza dal sito www.cr-consult.eu<hr/><strong>" +
                    contratto.Note +
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
                    contratto.Email,
                    "Richiesta di informazini per l'attivazione del contratto di manutenzione a www.cr-consult.eu",
                    "Gentile cliente, abbiamo ricevuto la tua richiesta dal sito cr-consult.eu.<hr/>" +
                    "Data: <strong>" + DateTime.Now + "</strong><br/>" +
                    "Nome contatto: <strong>" + contratto.Nome + " <em>" + contratto.Ditta + " </em></strong><br/>Telefono: <strong>" + contratto.Telefono + "</strong><br/>Mail: <strong>[" + contratto.Email + "]</strong> <br/>" +
                    "Motivo della richiesta: <br/><strong>" +
                    contratto.Note +
                    "</strong><hr/>Verrai contattato al più presto da un nostro incaricato per stabilire le modalità di incontro.<br/>" +
                    "Ti ricordo che per attivare un contratto è necessaria un'analisi approfondita della strutttura informatica da assistere. Sarà quindi necessario un incontro che permetterà a noi di capire comìè strutturata la tua realtà e a te di conoscere personalmente che ti seguirà."
                    );
                message1.IsBodyHtml = true;
                using (var smtp = new SmtpClient())
                {
                    await smtp.SendMailAsync(message1);
                }
                return RedirectToAction("FormOk", "Home");
            }
            return View(contratto);
        }

        [Authorize]
        public ActionResult Riservata()
        {
            return View();
        }
        public ActionResult FormOk()
        {
            return View();
        }

    }
}