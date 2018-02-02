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
            return View(db.Interventis.OrderBy(c=>c.Chiuso).ThenByDescending(d=>d.DataChiamata).ToList());
        }

        public ActionResult IndexUsr(string uid)
        {
            var interventi = db.Interventis.Where(u => u.Id == uid).ToList(); ;
            return View(interventi);
        }

    
        public async Task<ActionResult> IndexUt()
        {
            var uid = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(uid);
            ViewBag.Uid = uid;
            ViewBag.Gettone = user.Gettone;
            int gettoni = db.Gettonis.Where(g => g.Id == uid).Sum(g => (int?)g.NGettoni).GetValueOrDefault();
            int interventi = db.Interventis.Where(g => g.Id == uid).Sum(g => (int?)g.NGettoni).GetValueOrDefault();
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
        public ActionResult Create([Bind(Include = "Intervento_Id,Id,DataChiamata,DataIntervento,NGettoni,Descrizione,Chiuso")] Interventi interventi)
        {
            if (ModelState.IsValid)
            {
                db.Interventis.Add(interventi);
                db.SaveChanges();
                return RedirectToAction("Index");
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
        public ActionResult CreateUt([Bind(Include = "Intervento_Id,DataChiamata,DataIntervento,NGettoni,Descrizione,Chiuso")] Interventi interventi)
        {
            if (ModelState.IsValid)
            {
                var uid = User.Identity.GetUserId();
                interventi.Id = uid;
                interventi.DataChiamata = DateTime.Now;
                db.Interventis.Add(interventi);
                db.SaveChanges();
                return RedirectToAction("IndexUt", new {uid = uid });
            }

            return View(interventi);
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
        public ActionResult Edit([Bind(Include = "Intervento_Id,Id,DataChiamata,DataIntervento,NGettoni,Descrizione,Chiuso")] Interventi interventi)
        {
            if (ModelState.IsValid)
            {
                
                db.Entry(interventi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
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
            return RedirectToAction("Index");
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
