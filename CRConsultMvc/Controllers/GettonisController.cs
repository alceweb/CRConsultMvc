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
using System.Threading.Tasks;

namespace CRConsultMvc.Controllers
{
    public class GettonisController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public GettonisController()
        {

        }

        public GettonisController(ApplicationUserManager userManager)
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

        // GET: Gettonis
        public ActionResult Index()
        {
            return View(db.Gettonis.ToList());
        }

        public ActionResult IndexUsr(string uid)
        {
            var gettoni = db.Gettonis.Include(u=>u.Nome).Where(g => g.Id == uid).ToList();
            return View(gettoni);
        }

        [Authorize]
        public ActionResult IndexUt()
        {
            var uid = User.Identity.GetUserId();
            return View(db.Gettonis.Where(u=>u.Id == uid).ToList());
        }

        // GET: Gettonis/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gettoni gettoni = db.Gettonis.Find(id);
            if (gettoni == null)
            {
                return HttpNotFound();
            }
            return View(gettoni);
        }

        // GET: Gettonis/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Gettonis/Create
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Gettone_id,Id,Data,NGettoni,Totale,Pagato")] Gettoni gettoni)
        {
            if (ModelState.IsValid)
            {
                db.Gettonis.Add(gettoni);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(gettoni);
        }

        // GET: Gettonis/Create
        public ActionResult CreateUt(string uid)
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateUt([Bind(Include = "Gettone_id,Id,Data,NGettoni,Totale,Pagato")] Gettoni gettoni)
        {
            var uid = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(uid);
            var gett = user.Gettone;
            if (ModelState.IsValid)
            {
                gettoni.Id = User.Identity.GetUserId();
                gettoni.Data = DateTime.Now;
                gettoni.Totale = gett * gettoni.NGettoni;
                db.Gettonis.Add(gettoni);
                db.SaveChanges();
                return RedirectToAction("IndexUt", "Interventis");
            }

            return View(gettoni);
        }

        // GET: Gettonis/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gettoni gettoni = db.Gettonis.Find(id);
            if (gettoni == null)
            {
                return HttpNotFound();
            }
            return View(gettoni);
        }

        // POST: Gettonis/Edit/5
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Gettone_id,Id,Data,NGettoni,Totale,Pagato")] Gettoni gettoni)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gettoni).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gettoni);
        }

        // GET: Gettonis/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gettoni gettoni = db.Gettonis.Find(id);
            if (gettoni == null)
            {
                return HttpNotFound();
            }
            return View(gettoni);
        }

        // POST: Gettonis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Gettoni gettoni = db.Gettonis.Find(id);
            db.Gettonis.Remove(gettoni);
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
