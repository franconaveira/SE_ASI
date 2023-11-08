using Newtonsoft.Json;
using SE_ASI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SE_ASI.Controllers
{
    public class RegistrarController : Controller
    {
        // GET: Registrar
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string registrarDenuncia ()
        {
            string resul;
            var resolveRequest = HttpContext.Request;
            resolveRequest.InputStream.Seek(0, SeekOrigin.Begin);
            string jsonString = new StreamReader(resolveRequest.InputStream).ReadToEnd();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var jsonDeserializado = JsonConvert.DeserializeAnonymousType(jsonString, new { numExpediente = "", numDenuncia = "" });
            //CONEXION BASE DE DATOS
            SE_ASIEntities bd = new SE_ASIEntities();
            Diagnostico diagno = new Diagnostico();
            
            diagno.Numero_Denuncia_Legal = jsonDeserializado.numDenuncia;
            diagno = bd.Diagnostico.Find(int.Parse(jsonDeserializado.numExpediente));
            if(diagno != null)
            {
                diagno.Numero_Denuncia_Legal = jsonDeserializado.numDenuncia;
                bd.Entry(diagno).State = System.Data.Entity.EntityState.Modified;
                bd.SaveChanges();
                resul = "Registro insertado exitosamente";
                Console.WriteLine(resul);
            }
            else
            {
                resul = "Número de diagnóstico no encontrado";
            }
            return resul;
        }
    }
}