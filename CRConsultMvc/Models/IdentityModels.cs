using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System;
using System.Security.Principal;
using System.ComponentModel;
using System.Collections.Generic;

namespace CRConsultMvc.Models
{
    // È possibile aggiungere dati di profilo dell'utente specificando altre proprietà della classe ApplicationUser. Per ulteriori informazioni, visitare http://go.microsoft.com/fwlink/?LinkID=317594.
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Nome")]
        public string Nome { get; set; }
        [Display(Name = "Cognome")]
        public string Cognome { get; set; }
        [Display(Name = "Indirizzo")]
        public string Indirizzo { get; set; }
        [Display(Name = "Città")]
        public string Città { get; set; }
        [Display(Name = "CAP")]
        public string CAP { get; set; }
        [Display(Name = "Telefono")]
        public string Telefono { get; set; }
        [Display(Name =("Nome azienda"))]
        public string Ditta { get; set; }
        [Display(Name ="Partita Iva")]
        public string PartitaIva { get; set; }
        [Display(Name ="Codice Fiscale")]
        public string CodiceFiscale { get; set; }
        [Display(Name ="Privato")]
        public bool Privato { get; set; }
        [Display(Name ="Numero gettoni")]
        public int NGettoni { get; set; }
        [Display(Name ="Valore gettone")]
        [DataType(DataType.Currency)]
        [DefaultValue("25")]
        public float? Gettone { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Tenere presente che il valore di authenticationType deve corrispondere a quello definito in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Aggiungere qui i reclami utente personalizzati
            return userIdentity;
        }

    }


    public class Interventi
    {
        [Key]
        public int Intervento_Id { get; set; }
        public string Id { get; set; }
        public virtual ApplicationUser Nome { get; set; }
        [Display(Name ="Data chiamata")]
        public DateTime DataChiamata { get; set; }
        [Display(Name ="Data chiusura")]
        public DateTime? DataIntervento { get; set; }
        [Display(Name ="Numero gettoni")]
        public int NGettoni { get; set; }
        [Display(Name ="Motivo chiamata")]
        public string Descrizione { get; set; }
        [Display(Name ="Descrizione intervento")]
        public string Intervento { get; set; }
        [Display(Name ="Chiuso")]
        public bool Chiuso { get; set; }

    }

    public class Gettoni
    {
        [Key]
        public int Gettone_id { get; set; }
        public string Id { get; set; }
        public virtual ApplicationUser Nome { get; set; }
        [Display(Name ="Data acquisto")]
        public DateTime Data { get; set; }
        [Display(Name ="Numero gettoni")]
        public int NGettoni { get; set; }
        [Display(Name ="Totale")]
        [DataType(DataType.Currency)]
        public float? Totale { get; set; }
        [Display(Name ="Pagato")]
        public bool Pagato { get; set; }


    }
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet<CRConsultMvc.Models.Interventi> Interventis { get; set; }
        public DbSet<CRConsultMvc.Models.Gettoni> Gettonis { get; set; }

    }
}