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
    public class KPIController : Controller
    {
        public Administradora adm;

        // GET: KPI
        public ActionResult Index()
        {
            adm = (Administradora)Session["ClaseAdministradora"];
            if (adm == null)
                return RedirectToAction("Index", "Home");
            else
            {
                adm.calcularKpis();
                dynamic mymodel = new ExpandoObject();
                mymodel.Indicadores = adm.ListaIndicadores();
                return View(mymodel);
            }
        }
    }
}