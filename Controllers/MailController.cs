using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.IO;
//using System.Text.Json;
using Newtonsoft.Json;

using SE_ASI.Models;


namespace SE_ASI.Controllers
{
    public class MailController : Controller
    {
        // GET: Mail
        public Administradora adm;
        public ActionResult Index()
        {
            adm = (Administradora)Session["ClaseAdministradora"];
            if (adm == null)
                return RedirectToAction("Index", "Home");

            return View(adm.ListaCorreos());
        }

        [HttpPost]
        public void addMail (string mail , string name)
        {
            adm = (Administradora)Session["ClaseAdministradora"];
            adm.agregarCorreoBD(mail, name);
            Session["ClaseAdministradora"] = adm;
        }

        [HttpPost]
        public void removeMail(string idCorreo)
        {
            adm = (Administradora)Session["ClaseAdministradora"];
          
            adm.quitarCorreoBD(idCorreo);  
            Session["ClaseAdministradora"] = adm;
        }

        [HttpPost]
        public void updateMail(string idCorreo, string correo , string nombre)
        {
            adm = (Administradora)Session["ClaseAdministradora"];
            adm.actualizarCorreoBD(idCorreo , correo , nombre);
            Session["ClaseAdministradora"] = adm;
        }
    }
}