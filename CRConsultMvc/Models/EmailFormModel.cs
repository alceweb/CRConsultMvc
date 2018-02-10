using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CRConsultMvc.Models
{
    public class EmailFormModel
    {
        public string settore {get;set;}
        [Required, Display(Name = "Nome")]
        public string Nome { get; set; }
        public string Ditta { get; set; }
        [Required, Display(Name ="Telefono")]
        public string Telefono { get; set; }
        [Required, Display(Name = "Email"), EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Messaggio { get; set; }
        [Required, Display(Name ="Privacy")]
        public bool Privacy { get; set; }
    }

    public class EmailEsperto
    {
        [Required, Display(Name = "Nome")]
        public string Nome { get; set; }
        public string Ditta { get; set; }
        [Required, Display(Name = "Telefono")]
        public string Telefono { get; set; }
        [Required, Display(Name = "Email"), EmailAddress]
        public string Email { get; set; }
        [Required, Display(Name ="Motivo richiesta assistenza")]
        [DataType(DataType.MultilineText)]
        public string Richiesta { get; set; }
        [Required, Display(Name = "Privacy")]
        public bool Privacy { get; set; }
    }

    public class EmailContratto
    {

        [Required, Display(Name = "Nome Cognome")]
        public string Nome { get; set; }
        public string Ditta { get; set; }
        [Required, Display(Name = "Telefono")]
        public string Telefono { get; set; }
        [Required, Display(Name = "Email"), EmailAddress]
        public string Email { get; set; }
        [Required, Display(Name = "Note")]
        [DataType(DataType.MultilineText)]
        public string Note { get; set; }
        [Required, Display(Name = "Privacy")]
        public bool Privacy { get; set; }
    }
}