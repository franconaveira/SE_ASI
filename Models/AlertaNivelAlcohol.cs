using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SE_ASI.Models
{
    public class AlertaNivelAlcohol
    {
        public int id { get; set; }
        public int tipoAlerta { get; set; }
        public int fecha { get; set; } //pasar a DATETIME
        public int estado { get; set; } //
        public int notificado { get; set; } // 0 aun no se envio el mail , 1 se envio el mail 
        public DateTime fechaHora { get; set; }
    }
}