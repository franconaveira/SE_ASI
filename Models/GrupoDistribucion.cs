using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SE_ASI.Models
{
   /* public class GrupoDistribucion
    {
        public string id { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        //public List<Correo> lisCorreos { get; set; }
    }*/

    public class GrupoDistribucion
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public List<Correo> lisCorreos { get; set; }

        public GrupoDistribucion(int unId,  string unNombre , string unaDesc )
        {
            id = unId;
            nombre = unNombre;
            descripcion = unaDesc;
            lisCorreos = new List<Correo>();
        }
        public List<Correo> ListaCorreos()
        {
            return lisCorreos;
        }
        public void AgregarCorreo(Correo unCorreo)
        {
            lisCorreos.Add(unCorreo);
        }

        public void QuitarCorreo(Correo unCorreo)
        {
            lisCorreos.Remove(unCorreo);
        }

        public bool sos(int unId)
        {
            return (id == unId);
        }

        public void eliminarTodosCorreos()
        {
            lisCorreos.Clear();
        }

        public Correo tieneCorreo(int idCorreo)
        {
            foreach(Correo c in lisCorreos)
            {
                if (c.sos(idCorreo) == true)
                    return c;
            }

            return null;
        }

    }
}