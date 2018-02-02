using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRConsultMvc.Models
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Nome ruolo")]
        public string Name { get; set; }
    }

    public class EditUserViewModel
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "User name")]
        public string UserName { get; set; }
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
        [Display(Name = "Nome azienda")]
        public string Ditta { get; set; }
        [Display(Name = "Partita Iva")]
        public string PartitaIva { get; set; }
        [Display(Name = "Codice Fiscale")]
        public string CodiceFiscale { get; set; }
        [Display(Name = "Privato")]
        public bool Privato { get; set; }
        [Display(Name ="Valore gettone")]
        [DataType(DataType.Currency)]
        public float? Gettone { get; set; }
        public IEnumerable<SelectListItem> RolesList { get; set; }
    }
}