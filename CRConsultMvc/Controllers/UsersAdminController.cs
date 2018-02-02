using CRConsultMvc.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.IO;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.Web.Helpers;

namespace CRConsultMvc.Controllers
{
    public class UsersAdminController : Controller
    {

        public UsersAdminController()
        {
        }

        public UsersAdminController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
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

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        //
        // GET: /Users/
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index()
        {
            ViewBag.UtentiCount = UserManager.Users.Count();
            return View(await UserManager.Users.ToListAsync());
        }

        [Authorize]
        public ActionResult IndexUs()
        {
            var utente = User.Identity.GetUserId();
            ViewBag.uid = utente;
            return View();
        }

        //
        // GET: /Users/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ruoli = RoleManager.Roles.Where(r => r.Name.Contains("Squadra"));
            ViewBag.Ruoli = ruoli;
            var user = await UserManager.FindByIdAsync(id);
            ViewBag.RoleNames = await UserManager.GetRolesAsync(user.Id);
            foreach (var rol in ViewBag.RoleNames)
            {
                @ViewBag.Rol = rol;
            }
            return View(user);
        }

        //
        // GET: /Users/Create
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create()
        {
            //Get the list of Roles
            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
            return View();
        }

        //
        // POST: /Users/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> Create(RegisterViewModel userViewModel, params string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = userViewModel.Email, Email = userViewModel.Email };
                var adminresult = await UserManager.CreateAsync(user, userViewModel.Password);

                //Add User to the selected Roles 
                if (adminresult.Succeeded)
                {
                    if (selectedRoles != null)
                    {
                        var result = await UserManager.AddToRolesAsync(user.Id, selectedRoles);
                        if (!result.Succeeded)
                        {
                            ModelState.AddModelError("", result.Errors.First());
                            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
                            return View();
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", adminresult.Errors.First());
                    ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
                    return View();

                }
                return RedirectToAction("Index");
            }
            ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
            return View();
        }

        //
        // GET: /Users/Edit/1
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            ViewBag.Uid = id;
            if (user == null)
            {
                return HttpNotFound();
            }

            var userRoles = await UserManager.GetRolesAsync(user.Id);

            return View(new EditUserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Nome = user.Nome,
                Cognome = user.Cognome,
                Indirizzo = user.Indirizzo,
                Città = user.Città,
                CAP = user.CAP,
                Telefono = user.Telefono,
                Ditta = user.Ditta,
                PartitaIva = user.PartitaIva,
                CodiceFiscale = user.CodiceFiscale,
                Privato = user.Privato,
                Gettone = user.Gettone,
                RolesList = RoleManager.Roles.ToList().Select(x => new SelectListItem()
                {
                    Selected = userRoles.Contains(x.Name),
                    Text = x.Name,
                    Value = x.Name
                })
            });
        }

        //
        // POST: /Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Email,UserName,Nome,Cognome,Indirizzo,Città,CAP,Telefono,Ditta,PartitaIva,CodiceFiscale,Privato,Gettone")] EditUserViewModel editUser, params string[] selectedRole)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(editUser.Id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                user.UserName = editUser.Email;
                user.UserName = editUser.UserName;
                user.Email = editUser.Email;
                user.Nome = editUser.Nome;
                user.Cognome = editUser.Cognome;
                user.Indirizzo = editUser.Indirizzo;
                user.Città = editUser.Città;
                user.CAP = editUser.CAP;
                user.Telefono = editUser.Telefono;
                user.Ditta = editUser.Ditta;
                user.PartitaIva = editUser.PartitaIva;
                user.CodiceFiscale = editUser.CodiceFiscale;
                user.Privato = editUser.Privato;
                user.Gettone = editUser.Gettone;
                var userRoles = await UserManager.GetRolesAsync(user.Id);

                selectedRole = selectedRole ?? new string[] { };

                var result = await UserManager.AddToRolesAsync(user.Id, selectedRole.Except(userRoles).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                result = await UserManager.RemoveFromRolesAsync(user.Id, userRoles.Except(selectedRole).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                return RedirectToAction("IndexUs");
            }
            ModelState.AddModelError("", "Something failed.");
            return View();
        }


        //
        // GET: /Users/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /Users/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var user = await UserManager.FindByIdAsync(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                var result = await UserManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }

    }
}