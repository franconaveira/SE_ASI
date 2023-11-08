using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SE_ASI.Models
{
    public class Persona
    {
        public int id { get; set; }
        public int dni { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string fechaNac { get; set; }
        public string sexo { get; set; }
        public string direccion { get; set; }
        public string localidad { get; set; }
        public int telefono { get; set; }
        public string correo { get; set; }
        public string obrasocial { get; set; }

        public Persona(int d , string n , string a , string fn , string s, string dir , string loc, int tel , string co , string os)
        {
            dni = d;
            nombre = n;
            apellido = a;
            fechaNac = fn;
            sexo = s;
            direccion = dir;
            localidad = loc;
            telefono = tel;
            correo = co;
            obrasocial = os;
        }
    }
}