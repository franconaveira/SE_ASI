using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SE_ASI.Models
{
    public class Kpi
    {
        public string nombre { get; set; }
        public float valor { get; set; }

        public string medida { get; set; }

        public Kpi (string unNombre , float unValor , string unaMedida)
        {
            nombre = unNombre;
            valor = unValor;
            medida = unaMedida;
        }
    }
}