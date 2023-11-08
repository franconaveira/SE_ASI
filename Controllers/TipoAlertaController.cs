using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Dynamic;
using System.Text.Json;

using SE_ASI.Models;

namespace SE_ASI.Controllers
{
    public class TipoAlertaController : Controller
    {
        // GET: TipoAlerta
        public Administradora adm;

        public ActionResult Index()
        {
            adm = (Administradora)Session["ClaseAdministradora"];
            if (adm == null)
                return RedirectToAction("Index", "Home");

            dynamic mymodel = new ExpandoObject();
            mymodel.TiposAlerta = adm.ListaTiposAlerta();
            mymodel.GruposDistribucion = adm.ListaGruposDistribucion();
            return View(mymodel);

        }
        public string getGruposDistribucion(string idTipoAlerta) //devuelve los correos del grupo especificaod
        {
            adm = (Administradora)Session["ClaseAdministradora"];
            List<GrupoDistribucion> lgd = adm.obtenerGruposDeAlerta(int.Parse(idTipoAlerta));
            var json = System.Text.Json.JsonSerializer.Serialize(lgd);
            return json;
        }

        public string getGruposDistribucionDisponibles(string idTipoAlerta)
        {
            adm = (Administradora)Session["ClaseAdministradora"];
            List<GrupoDistribucion> lisGDAsignados = adm.obtenerGruposDeAlerta(int.Parse(idTipoAlerta));
            List<GrupoDistribucion> lisgdTodos = new List<GrupoDistribucion>();

            foreach (GrupoDistribucion c in adm.ListaGruposDistribucion())
                lisgdTodos.Add(c);

            foreach (GrupoDistribucion c in lisGDAsignados)
                lisgdTodos.Remove(c);

            var json = System.Text.Json.JsonSerializer.Serialize(lisgdTodos);
            return json;
        }

        public string getGDById(string idGrupoDistribucion) //get correo by id
        {
            
            adm = (Administradora)Session["ClaseAdministradora"];
     
            GrupoDistribucion gd = adm.buscarGrupoDistribucion(int.Parse(idGrupoDistribucion));

                List<GrupoDistribucion> lgd = new List<GrupoDistribucion>();
                lgd.Add(gd);
                var json = System.Text.Json.JsonSerializer.Serialize(lgd);
                return json;
        }
        public void updateGruposTA(long[] asd)
        {
            adm = (Administradora)Session["ClaseAdministradora"];
            int idTipoAlerta = int.Parse(asd[0].ToString());
            adm.eliminarAsociacionGruposATD(idTipoAlerta);

            for (int i = 1; i < asd.Length; i++)
            {
                adm.agregarGDATipoAlerta(idTipoAlerta, int.Parse(asd[i].ToString()));
            }

            Session["ClaseAdministradora"] = adm;
        }

    }
}