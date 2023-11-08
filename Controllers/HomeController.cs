using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Dynamic;
using System.Text.Json;
using System.Web.Script.Serialization;
using System.Data.SqlClient;
using System.Security.Policy;
using Ubiety.Dns.Core;
using Newtonsoft.Json.Linq;
using System.Data.Entity.Infrastructure;

using SE_ASI.Models;

namespace SE_ASI.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            // Add a new item to the cache
            return View();
        }

        [HttpPost]
        public String detectarASI()
        {
            var resolveRequest = HttpContext.Request;
            resolveRequest.InputStream.Seek(0, SeekOrigin.Begin);
            string jsonString = new StreamReader(resolveRequest.InputStream).ReadToEnd();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            HttpWebRequest httpRequest = (HttpWebRequest)HttpWebRequest.Create("http://localhost:8080/ASI");
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";
            var streamWriter = new StreamWriter(httpRequest.GetRequestStream());
            streamWriter.Write(jsonString);
            //log.Debug("Peticion: " + jsonString);
            streamWriter.Close();
            string result;

            WebResponse resp = httpRequest.GetResponse();
            using (var reader = new StreamReader(resp.GetResponseStream()))
            {
                result = reader.ReadToEnd();
            }
            //log.Debug("Respuesta: " + result);

            //CONEXION BASE DE DATOS
            SE_ASIEntities bd = new SE_ASIEntities();

            //INSERTAR INDICADORES RECIBIDOS
            Informe_Recibido informe = new Informe_Recibido();
            var jsonIndicadores = JsonConvert.DeserializeAnonymousType(jsonString, new { sexo = "", edad = "", gestando = "", relato = "", semen = "", enfermedad_transm_sexual = "", lesiones_genitales = "", inflamacion_vaginal = "", incontinencia_urinaria = "", dolores_de_abdomen = "", incontinenciaFecal = "", conductas_Sexualizadas = "", fugas = "", consumo_problematico = "", retraido = "", autolesiones = "", problemas_al_dormir = "", comportamientoSuicida = "", problemas_alimenticios = "", terrores_sin_causa_aparente = "", desconexion_mental = "", Temor_lugar_especifico = "", incontinencia_fecal = ""});
            informe.Sexo = jsonIndicadores.sexo;
            informe.Edad = int.Parse(jsonIndicadores.edad);
            informe.Relato = jsonIndicadores.relato;

            if (jsonIndicadores.gestando.Equals("No"))
                informe.Gestando = false;
            else
                informe.Gestando = true;

            informe.Semen = jsonIndicadores.semen;
            informe.Enfermedad_transm_sexual = jsonIndicadores.enfermedad_transm_sexual;
            informe.Lesiones_genitales = jsonIndicadores.lesiones_genitales;

            if (jsonIndicadores.inflamacion_vaginal.Equals("No"))
                informe.Inflamación_Vaginal = false;
            else
                informe.Inflamación_Vaginal = true;

            informe.Incontinencia_urinaria = jsonIndicadores.incontinencia_urinaria;
            informe.Incontinencia_fecal = jsonIndicadores.incontinencia_fecal;
            informe.Dolores_de_abdomen = jsonIndicadores.dolores_de_abdomen;
            informe.Conductas_Sexualizadas = jsonIndicadores.conductas_Sexualizadas;
            informe.Fugas = jsonIndicadores.fugas;

            if (jsonIndicadores.retraido.Equals("No"))
                informe.Retraido = false;
            else
                informe.Retraido = false;
            informe.Autolesiones = jsonIndicadores.autolesiones;
            informe.Problemas_al_dormir = jsonIndicadores.problemas_al_dormir;
            if (jsonIndicadores.comportamientoSuicida.Equals("No"))
                informe.Comportamiento_suicida = false;
            else
                informe.Comportamiento_suicida = true;
            informe.Problemas_alimenticios = jsonIndicadores.problemas_alimenticios;
            informe.Terrores_sin_causa_aparente = jsonIndicadores.terrores_sin_causa_aparente;
            informe.Problemas_al_dormir = jsonIndicadores.desconexion_mental;
            informe.Consumo_problemático = jsonIndicadores.consumo_problematico;
            informe.Desconexión_mental = jsonIndicadores.desconexion_mental;
            bd.Informe_Recibido.Add(informe);
            bd.SaveChanges();
            

            //INSERTAR RESULTADO DEL DIAGNOSTICO
            Diagnostico diagno = new Diagnostico();
            var jsonDiagnostico = JsonConvert.DeserializeAnonymousType(result, new { sospecha = "", gradoSospecha = "", abordaje = ""});
            if (jsonDiagnostico.sospecha.Equals(true))
            {
                diagno.Sospecha = true;
                diagno.Nivel = jsonDiagnostico.gradoSospecha;
            }
            else
            {
                diagno.Sospecha = false;
                diagno.Nivel = jsonDiagnostico.gradoSospecha;
            }
            diagno.Descripcion_Abordaje = jsonDiagnostico.abordaje;
            diagno.ID_Indicadores_Obtenidos = informe.ID_Informe;
            bd.Diagnostico.Add(diagno);
            bd.SaveChanges();



            //FIN
            //Esto hago para agregar a result el numero de diagnostico generado en la base de datos y poder mostrarlo en pantalla
            JObject resultModificado = JsonConvert.DeserializeObject<JObject>(result);
            resultModificado["numeroDiagnostico"] = diagno.ID_Diagnostico;
            string resultadoFinal = resultModificado.ToString(Formatting.Indented);
            return resultadoFinal;
        }

    }
}