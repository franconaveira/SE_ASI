using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SE_ASI.Models
{
    public class TipoAlerta
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public List<GrupoDistribucion> lisGruposDistribucion { get; set; }

        public TipoAlerta(int unId , string unNomb , string unaDesc )
        {
            id = unId;
            nombre = unNomb;
            descripcion = unaDesc;
            lisGruposDistribucion = new List<GrupoDistribucion>();
        }

        public List<GrupoDistribucion> ListaGD()
        {
            return lisGruposDistribucion;
        }


        public void agregarGD (GrupoDistribucion gd)
        {
            lisGruposDistribucion.Add(gd);
        }

        public void quitarGD(GrupoDistribucion gd)
        {
            lisGruposDistribucion.Remove(gd);
        }

        public bool sos(int unId)
        {
            return id == unId;
        }

        ///<summary>
        ///Elimina todos los Grupos de distribucion asociados 
        ///</summary>
        public void eliminarTodosGrupos()
        {
            lisGruposDistribucion.Clear();
        }

        ///<summary>
        ///Si tiene asignado el GD del id que se pasa por parametro, devuelve la posicion . caso contrario devuelve null
        ///</summary>
        public GrupoDistribucion tieneGrupo(int idGrupo)
        {
            foreach (GrupoDistribucion gd in lisGruposDistribucion)
            {
                if (gd.sos(idGrupo) == true)
                    return gd;
            }

            return null;
        }
    }
}