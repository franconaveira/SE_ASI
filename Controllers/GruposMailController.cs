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
    public class GruposMailController : Controller
    {
        // GET: GruposMail
        public Administradora adm;
        public ActionResult Index()
        {
            adm = (Administradora)Session["ClaseAdministradora"];
            if(adm == null)
                return RedirectToAction("Index", "Home");

            dynamic mymodel = new ExpandoObject();
            mymodel.Grupos = adm.ListaGruposDistribucion();
            mymodel.Correos = adm.ListaCorreos();
            return View(mymodel);
//            return View(adm.ListaGruposDistribucion());
        }
        
        public void addGrupoDistribucion(string nombreGrupo, string descripcionGrupo)
        {
            adm = (Administradora)Session["ClaseAdministradora"];
            adm.agregarGrupoDistribucionBD(nombreGrupo, descripcionGrupo);
            Session["ClaseAdministradora"] = adm;
        }

        public void deleteGrupoDistribucion(string grupoId)
        {
            adm = (Administradora)Session["ClaseAdministradora"];
            adm.quitarGrupoDistribucionBD(grupoId);
            Session["ClaseAdministradora"] = adm;
        }

        public void updateGrupoDistribucion(string groupId, string groupName, string groupDesc)
        {
            adm = (Administradora)Session["ClaseAdministradora"];
            adm.actualizarGrupoDistribucionBD(groupId, groupName, groupDesc);
            Session["ClaseAdministradora"] = adm;
        }

        public string getCorreos(string idGrupo) //devuelve los correos del grupo especificaod
        {
            adm = (Administradora)Session["ClaseAdministradora"];
            List<Correo> lc = adm.obtenerCorreosDeGrupo(int.Parse(idGrupo));
            var json = System.Text.Json.JsonSerializer.Serialize(lc);
            return json;
            //return Json(new { success = true, responseText = "The attached file is not supported." }, JsonRequestBehavior.AllowGet);
        }

        public string getCorreosDisponibles(string idGrupo)
        {
            adm = (Administradora)Session["ClaseAdministradora"];
            List<Correo> lisCorreosAsignados = adm.obtenerCorreosDeGrupo(int.Parse(idGrupo));
            List<Correo> lisCorreosTodos = new List<Correo>();

            foreach (Correo c in adm.ListaCorreos())
                lisCorreosTodos.Add(c);

            foreach (Correo c in lisCorreosAsignados)
                lisCorreosTodos.Remove(c);

            var json = System.Text.Json.JsonSerializer.Serialize(lisCorreosTodos);
            return json;
        }

        public string getCorreoById(string idCorreo)
        {
            adm = (Administradora)Session["ClaseAdministradora"];
            Correo c = adm.buscarCorreo(int.Parse(idCorreo));
            List<Correo> lc = new List<Correo>();
            lc.Add(c);
            var json = System.Text.Json.JsonSerializer.Serialize(lc);
            return json;

        }

        public void updateMiembrosGD(long[] asd)
        {
            adm = (Administradora)Session["ClaseAdministradora"];
            int idGrupo = int.Parse(asd[0].ToString());
            adm.eliminarAsociacionCorreos(idGrupo);
            
            for(int i=1; i<asd.Length; i++)
            {
                adm.agregarCorreoAGD(idGrupo, int.Parse(asd[i].ToString()));
            }

            Session["ClaseAdministradora"] = adm;
        }
    }


}