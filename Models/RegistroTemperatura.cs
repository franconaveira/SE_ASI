using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SE_ASI.Models
{
    public class RegistroTemperatura
    {
        public int id { get; set; }
        public float temperatura { get; set; }
        public int fecha{ get; set; } //pasar a DATETIME
        public int estado { get; set; } // 0 sin fiebre ; 1 con fiebre ; > 1 con fiebre tratado
        public int dispositivo { get; set; }
        public DateTime fechaHora { get; set; }
        //public persona
        public bool sos (int unId)
        {
            return (id == unId);
        }
    }
}