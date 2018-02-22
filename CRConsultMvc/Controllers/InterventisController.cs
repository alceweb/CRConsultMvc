using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CRConsultMvc.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Web.Helpers;


namespace CRConsultMvc.Controllers
{
    [Authorize]
    public class InterventisController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public InterventisController()
        {

        }

        public InterventisController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Interventis
        public ActionResult Index()
        {
            return View(db.Interventis.ToList());
        }

        public async Task<ActionResult> Riservata()
        {
            var uid = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(uid);
            var interventi = new List<Interventi>();
            var gettoni = new List<Gettoni>();
            if (User.IsInRole("Admin"))
            {
                interventi = db.Interventis.OrderBy(u => u.Chiuso).ThenByDescending(u => u.DataChiamata).ToList();
                gettoni = db.Gettonis.ToList();
            }
            else
            {
                interventi = db.Interventis.Where(u => u.Id == uid).OrderBy(u => u.Chiuso).ThenByDescending(u => u.DataChiamata).ToList();
                gettoni = db.Gettonis.Where(u => u.Id == uid).ToList();
           }
            int get = db.Gettonis.Where(g => g.Id == uid).Sum(g => (int?)g.NGettoni).GetValueOrDefault();
            int tic = db.Interventis.Where(g => g.Id == uid).Sum(g => (int?)g.NGettoni).GetValueOrDefault();
            ViewBag.GettoniCount = get - tic;
            ViewBag.Interventi = interventi;
            ViewBag.Gettoni = gettoni;
            ViewBag.InterventiCount = interventi.Count();
            ViewBag.InterventiChiusiCount = interventi.Where(c => c.Chiuso == true).Count();
            ViewBag.InterventiApertiCount = interventi.Where(c => c.Chiuso == false).Count();
            ViewBag.User = user;
            ViewBag.Uid = uid;
            return View();
        }
        public async Task<ActionResult> IndexUsr(string uid)
        {
            var utente = await UserManager.FindByIdAsync(uid);
            int get = db.Gettonis.Where(g => g.Id == uid).Sum(g => (int?)g.NGettoni).GetValueOrDefault();
            int tic = db.Interventis.Where(g => g.Id == uid).Sum(g => (int?)g.NGettoni).GetValueOrDefault();
            ViewBag.Utente = utente.Ditta;
            ViewBag.GettoniCount = get - tic;
            ViewBag.Uid = uid;
            var interventi = db.Interventis.Where(u => u.Id == uid).ToList(); ;
            return View(interventi);
        }

    
        public async Task<ActionResult> IndexUt()
        {
            var uid = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(uid);
            var tickets = db.Interventis.Where(u => u.Id == uid).ToList();
            ViewBag.Uid = uid;
            ViewBag.Gettone = user.Gettone;
            ViewBag.InterventiCount = tickets.Count();
            int gettoni = db.Gettonis.Where(g => g.Id == uid).Sum(g => (int?)g.NGettoni).GetValueOrDefault();
            int interventi = tickets.Sum(g => (int?)g.NGettoni).GetValueOrDefault();
            ViewBag.GettoniR = gettoni - interventi;
            return View(db.Interventis.Where(u => u.Id == uid).OrderBy(c=>c.Chiuso).ThenByDescending(c=>c.DataChiamata));
        }

        // GET: Interventis/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interventi interventi = db.Interventis.Find(id);
            if (interventi == null)
            {
                return HttpNotFound();
            }
            return View(interventi);
        }

        // GET: Interventis/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Interventis/Create
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Intervento_Id,Id,DataChiamata,DataIntervento,NGettoni,Chiuso", Exclude ="Descrizione,Intervento")] Interventi interventi)
        {
            FormCollection collection = new FormCollection(Request.Unvalidated().Form);
            interventi.Descrizione = collection["Descrizione"];
            interventi.Intervento = collection["Intervento"];
            if (ModelState.IsValid)
            {
                db.Interventis.Add(interventi);
                db.SaveChanges();
                return RedirectToAction("Riservata");
            }

            return View(interventi);
        }

        public ActionResult CreateUt()
        {
            var uid = User.Identity.GetUserId();
            int gettoni = db.Gettonis.Where(g => g.Id == uid).Sum(g => (int?)g.NGettoni).GetValueOrDefault();
            int interventi = db.Interventis.Where(g => g.Id == uid).Sum(g => (int?)g.NGettoni).GetValueOrDefault();
            ViewBag.GettoniR = gettoni - interventi;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateUt([Bind(Include = "Intervento_Id,DataChiamata,DataIntervento,NGettoni,Chiuso", Exclude = "Descrizione")] Interventi interventi)
        {
            FormCollection collection = new FormCollection(Request.Unvalidated().Form);
            interventi.Descrizione = collection["Descrizione"];
            if (ModelState.IsValid)
            {
                var uid = User.Identity.GetUserId();
                interventi.Id = uid;
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                var umail = user.Email;
                interventi.DataChiamata = DateTime.Now;
                db.Interventis.Add(interventi);
                db.SaveChanges();
                //Invio la mail a crconsult
                //MailMessage message = new MailMessage("webservice@cr-consult.it",
                //    umail,
                //    "Abbiamo ricevuto la tua richiesta",
                //    "Gentile cliente, il giorno " + DateTime.Now.ToString("dddd dd MMM yyyy") + " alle " + DateTime.Now.ToString("HH.mm") + " abbiamo registrato il tuo tiicket N. CRC-" + interventi.Intervento_Id + ": <hr/>" +
                //    "Data: <strong>" + DateTime.Now + "</strong><br/>" +
                //    interventi.Descrizione +
                //    "</strong><hr/>Verrai contattato al più presto dal tuo specialista di riferimento<br>"
                //    );
                //message.IsBodyHtml = true;
                //MailAddress bcc = new MailAddress("cesare@cr-consult.eu");
                //message.Bcc.Add(bcc);
                //using (var smtp = new SmtpClient())
                //{
                //    await smtp.SendMailAsync(message);
                //}

                return RedirectToAction("IndexUt", new {uid = uid });
            }

            return View(interventi);
        }


        public async Task<ActionResult> CreateUsr(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            ViewBag.User = user;
            return View();
        }
         // GET: Interventis/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interventi interventi = db.Interventis.Find(id);
            if (interventi == null)
            {
                return HttpNotFound();
            }
            return View(interventi);
        }

       // POST: Interventis/Edit/5
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Intervento_Id,Id,DataChiamata,DataIntervento,NGettoni,Descrizione,Chiuso", Exclude = "Descrizione,Intervento")] Interventi interventi)
        {
            FormCollection collection = new FormCollection(Request.Unvalidated().Form);
            interventi.Descrizione = collection["Descrizione"];
            interventi.Intervento = collection["Intervento"];
            if (ModelState.IsValid)
            {
                
                db.Entry(interventi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Riservata");
            }
            return View(interventi);
        }

        // GET: Interventis/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interventi interventi = db.Interventis.Find(id);
            if (interventi == null)
            {
                return HttpNotFound();
            }
            return View(interventi);
        }

        // POST: Interventis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Interventi interventi = db.Interventis.Find(id);
            db.Interventis.Remove(interventi);
            db.SaveChanges();
            return RedirectToAction("Riservata");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
