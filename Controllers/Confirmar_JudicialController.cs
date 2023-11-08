using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    public class Confirmar_JudicialController : Controller
    {
        // GET: Confirmar
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public String buscarDenunciaLegal()
        {
            var resolveRequest = HttpContext.Request;
            resolveRequest.InputStream.Seek(0, SeekOrigin.Begin);
            string jsonString = new StreamReader(resolveRequest.InputStream).ReadToEnd();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var jsonDeserializado = JsonConvert.DeserializeAnonymousType(jsonString, new { numExpediente = "" });
            //CONEXION BASE DE DATOS
            SE_ASIEntities bd = new SE_ASIEntities();
            Informe_Recibido informe = new Informe_Recibido();
            Diagnostico diagno = new Diagnostico();

            diagno = bd.Diagnostico.FirstOrDefault(Numero_Denuncia_Legal => Numero_Denuncia_Legal.Numero_Denuncia_Legal == jsonDeserializado.numExpediente);     
                
                if (diagno != null)
                {
                    informe = bd.Informe_Recibido.Find(diagno.ID_Diagnostico);
                         if (informe != null)
                             {
                                string resul = JsonConvert.SerializeObject(diagno);

                                JObject resultModificado = JsonConvert.DeserializeObject<JObject>(resul);
                                string resultadoFinal = resultModificado.ToString(Formatting.Indented);
                                return resultadoFinal;
                              }
                }

            else
            {
                return "Diagnostico no encontrado";
            }
            return "Diagnostico no encontrado";


        }

        [HttpPost]
        public String confirmarDiagnostico()
        {
            string resul;
            var resolveRequest = HttpContext.Request;
            resolveRequest.InputStream.Seek(0, SeekOrigin.Begin);
            string jsonString = new StreamReader(resolveRequest.InputStream).ReadToEnd();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var jsonDeserializado = JsonConvert.DeserializeAnonymousType(jsonString, new { numExpediente = "", Confirmacion_Experto = "", valorExperto = "" });
            //CONEXION BASE DE DATOS
            SE_ASIEntities bd = new SE_ASIEntities();
            Diagnostico diagno = new Diagnostico();
            diagno = bd.Diagnostico.Single(Numero_Denuncia_Legal => Numero_Denuncia_Legal.Numero_Denuncia_Legal == jsonDeserializado.numExpediente);
            if (diagno != null)
            {
                if (jsonDeserializado.Confirmacion_Experto.Equals("1"))
                {
                    diagno.Confirmacion_Legal = true;
                }
                else
                {
                    diagno.Confirmacion_Legal = false;
                }
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