using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CRConsultMvc.Models
{
    public class EmailFormModel
    {
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
}