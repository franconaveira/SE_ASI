using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SE_ASI.Models
{

    public class Correo // los nombres de los atributos deben coincidir a la BD para que el JSON se pueda descerealizar
    {
        public int id { get; set; }
        public string correo { get; set; }
        public string nombre { get; set; }

        public Correo (int unId , string unCorreo , string unNombre)
        {
            id = unId;
            correo = unCorreo;
            nombre = unNombre;
        }
        
        public bool sos(int unId)
        {
            return (id == unId);
        }

    }
}