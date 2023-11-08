using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
//using System.Text.Json;
using Newtonsoft.Json;

using System.Net.Mail;


namespace SE_ASI.Models
{
    public class Administradora
    {
        public List<Correo> lisCorreos;
        public List<GrupoDistribucion> lisGruposDistribucion;
        public List<RegistroTemperatura> lisRT; //TODOS
        public List<RegistroTemperatura> lisRT_SinFiebre;
        public List<RegistroTemperatura> lisRT_ConFiebre_Abiertos;
        public List<RegistroTemperatura> lisRT_ConFiebre_Cerrados;
        List<AlertaNivel> lisAl;
        public bool alertaNivelAgua;
        public List<TipoAlerta> lisTipoAlertas;
        public List<Kpi> lisIndicadores;
        public List<AlertaNivelAlcohol> lisAlertasAlcohol;

        public List<Correo> ListaCorreos()
        {
            //devolver ordenado!!!!
            return lisCorreos;
        }

        public List<Kpi> ListaIndicadores()
        {
            return lisIndicadores;
        }

        public List<TipoAlerta> ListaTiposAlerta()
        {
            return lisTipoAlertas;
        }

        public List<AlertaNivel> ListaAlertasAgua()
        {
            return lisAl;
        }

        public List<GrupoDistribucion> ListaGruposDistribucion()
        {
            return lisGruposDistribucion;//devolver ordenado!!!!
        }

        public List<RegistroTemperatura> ListaRT_SinFiebre()
        {
            return lisRT_SinFiebre;
        }

        public List<RegistroTemperatura> ListaRT_ConFiebre_Abiertos()
        {
            return lisRT_ConFiebre_Abiertos;
        }

        public List<RegistroTemperatura> ListaRT_ConFiebre_Cerrados()
        {
            return lisRT_ConFiebre_Cerrados;
        }

        public int AlertaNivel()
        {
            if (alertaNivelAgua)
                return 1;
            else
                return 0;
        }

        public Administradora()
        {
            lisCorreos = new List<Correo>();
            lisGruposDistribucion = new List<GrupoDistribucion>();
            lisRT = new List<RegistroTemperatura>();
            lisRT_SinFiebre = new List<RegistroTemperatura>();
            lisRT_ConFiebre_Abiertos = new List<RegistroTemperatura>();
            lisRT_ConFiebre_Cerrados = new List<RegistroTemperatura>();
            lisAl = new List<AlertaNivel>();
            alertaNivelAgua = false;
            lisTipoAlertas = new List<TipoAlerta>();
            lisIndicadores = new List<Kpi>();
            lisAlertasAlcohol = new List<AlertaNivelAlcohol>();
        }

        #region CORREOS
        public void recuperarCorreos()
        {
            WebRequest request = WebRequest.Create("http://localhost/puedes_pasar/correos.php"); // Pasar a variable del webConfig
            WebResponse response = request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                lisCorreos = JsonConvert.DeserializeObject<List<Correo>>(responseFromServer);
                response.Close();
            }
        }
        public void agregarCorreoBD(string mail, string name)
        {
            WebRequest request = WebRequest.Create("http://localhost/puedes_pasar/correo.php/" + "?mail=" + mail + "&name=" + name);
            WebResponse response = request.GetResponse();
            int id = 0;

            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                id = JsonConvert.DeserializeObject<int>(responseFromServer);
            }
            if (id != 0)
            {
                Correo c = new Correo(id, mail, name);
                this.agregarCorreo(c);
            }
            response.Close();
        }

        public void agregarCorreo(Correo c)
        {
            lisCorreos.Add(c);
        }

        public void quitarCorreoBD(string idCorreo)
        {
            // NUEVO : validar si no tiene asignado grupo
            WebRequest request = WebRequest.Create("http://localhost/puedes_pasar/correoDelete.php/" + "?correoId=" + idCorreo);
            WebResponse response = request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
            }
            response.Close();

            this.quitarCorreoDeGrupo(int.Parse(idCorreo));
            this.quitarCorreo(int.Parse(idCorreo));
        }

        public void quitarCorreo(int id)
        {
            Correo c = buscarCorreo(id);

            if (c != null)
                lisCorreos.Remove(c);
        }

        public void quitarCorreoDeGrupo(int idCorreo)
        {
            Correo c = null;

            foreach (GrupoDistribucion gd in lisGruposDistribucion)
            {
                c = gd.tieneCorreo(idCorreo);
                if (c != null)
                    gd.QuitarCorreo(c);
            }
        }

        public Correo buscarCorreo(int id)
        {
            int i = 0;

            while (i < lisCorreos.Count() && !lisCorreos[i].sos(id))
                i++;

            if (i < lisCorreos.Count())
                return lisCorreos[i];
            else
                return null;

        }
        public void actualizarCorreoBD(string idCorreo, string correo, string nombre)
        {
            // NUEVO : validar si no tiene asignado grupo
            WebRequest request = WebRequest.Create("http://localhost/puedes_pasar/correoUpdate.php/" + "?mailId=" + idCorreo + "&mailName=" + nombre + "&mailAddress=" + correo);
            WebResponse response = request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
            }
            response.Close();

            this.actualizarCorreo(int.Parse(idCorreo), correo, nombre);
        }

        public void actualizarCorreo(int idC, string nuevaDirCorreo, string nuevoNombre)
        {
            Correo c = this.buscarCorreo(idC);
            c.nombre = nuevoNombre;
            c.correo = nuevaDirCorreo;
        }
        #endregion //Correos

        #region Grupos Distribucion
        public void recuperarGruposDistribucion()
        {
            WebRequest request = WebRequest.Create("http://localhost/puedes_pasar/GrupoDistribucion_Get.php");
            WebResponse response = request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                lisGruposDistribucion = JsonConvert.DeserializeObject<List<GrupoDistribucion>>(responseFromServer);
            }
            response.Close();
        }

        public void agregarGrupoDistribucionBD(string nombre, string desc)
        {
            WebRequest request = WebRequest.Create("http://localhost/puedes_pasar/GrupoDistribucion_Create.php" + "?grupoNombre=" + nombre + "&grupoDescripcion=" + desc);
            WebResponse response = request.GetResponse();
            int id = 0;

            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                id = JsonConvert.DeserializeObject<int>(responseFromServer);
            }
            if (id != 0)
            {
                GrupoDistribucion gd = new GrupoDistribucion(id, nombre, desc);
                this.agregarGrupoDistribucion(gd);
            }
            response.Close();
        }

        public void agregarGrupoDistribucion(GrupoDistribucion gd)
        {
            lisGruposDistribucion.Add(gd);
        }

        public void quitarGrupoDistribucionBD(string idGrupo)
        {
            // NUEVO : validar si no tiene asignado grupo
            WebRequest request = WebRequest.Create("http://localhost/puedes_pasar/GrupoDistribucion_Delete.php" + "?grupoId=" + idGrupo);
            WebResponse response = request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
            }
            response.Close();
            GrupoDistribucion gd = this.buscarGrupoDistribucion(int.Parse(idGrupo));
            gd.eliminarTodosCorreos();
            this.quitarGrupoDeTipoAlerta(int.Parse(idGrupo));
            this.quitarGrupoDistribucion(int.Parse(idGrupo));
        }


        public void quitarGrupoDeTipoAlerta(int idGrupo)
        {
            GrupoDistribucion gd = null;

            foreach (TipoAlerta ta in lisTipoAlertas)
            {
                gd = ta.tieneGrupo(idGrupo);
                if (gd != null)
                    ta.quitarGD(gd);
            }
        }


        public void quitarGrupoDistribucion(int id)
        {
            GrupoDistribucion gd = buscarGrupoDistribucion(id);

            if (gd != null)
                lisGruposDistribucion.Remove(gd);
        }

        public GrupoDistribucion buscarGrupoDistribucion(int id)
        {
            int i = 0;

            while (i < lisGruposDistribucion.Count() && !lisGruposDistribucion[i].sos(id))
                i++;

            if (i < lisGruposDistribucion.Count())
                return lisGruposDistribucion[i];
            else
                return null;

        }

        public void actualizarGrupoDistribucionBD(string groupId, string groupName, string groupDesc)
        {
            WebRequest request = WebRequest.Create("http://localhost/puedes_pasar/GrupoDistribucion_Update.php/" + "?GroupId=" + groupId + "&GroupName=" + groupName + "&GroupDesc=" + groupDesc);
            WebResponse response = request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
            }
            response.Close();

            this.actualizarGrupoDistribucion(int.Parse(groupId), groupName, groupDesc);
        }

        public void actualizarGrupoDistribucion(int idGD, string newName, string NewDesc)
        {
            GrupoDistribucion gd = this.buscarGrupoDistribucion(idGD);
            gd.nombre = newName;
            gd.descripcion = NewDesc;
        }

        #endregion Grupos Distribucion


        #region relacion Grupo-Mail
        private class relGM
        {
            public int idGrupo { get; set; }
            public int idCorreo { get; set; }
        }

        public void recuperarRelacionGrupoMailBD()
        {
            WebRequest request = WebRequest.Create("http://localhost/puedes_pasar/Grupos_rel_correos_Get.php");
            WebResponse response = request.GetResponse();
            List<relGM> lisRelacionGruposCorreos = new List<relGM>();

            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                lisRelacionGruposCorreos = JsonConvert.DeserializeObject<List<relGM>>(responseFromServer);
            }

            GrupoDistribucion gd = null;
            Correo c = null;

            foreach (relGM r in lisRelacionGruposCorreos)
            {
                gd = this.buscarGrupoDistribucion(r.idGrupo);

                if (gd != null)
                {
                    c = this.buscarCorreo(r.idCorreo);
                    if (c != null)
                        gd.AgregarCorreo(c);
                }
            }
            response.Close();
        }

        public List<Correo> obtenerCorreosDeGrupo(int idGrupo)
        {
            GrupoDistribucion gd = this.buscarGrupoDistribucion(idGrupo);
            return (gd.ListaCorreos());
        }




        public void eliminarCorreosBD(int idGrupo) //ELIMINA TODOS LOS CORREOS RELACIONADOS A UN GRUPO DE DISTRIBUCION
        {
            WebRequest request = WebRequest.Create("http://localhost/puedes_pasar/GrupoDistribucion_DelRel.php/" + "?GroupId=" + idGrupo);
            WebResponse response = request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
            }
            response.Close();
        }

        public void eliminarAsociacionCorreos(int idGrupo)
        {
            GrupoDistribucion gd = this.buscarGrupoDistribucion(idGrupo);
            this.eliminarCorreosBD(idGrupo);
            gd.eliminarTodosCorreos();
        }

        public void agregarCorreoAGD(int idGrupo, int idCorreo)
        {
            GrupoDistribucion gd = this.buscarGrupoDistribucion(idGrupo);
            Correo c = this.buscarCorreo(idCorreo);
            this.agregarCorreoAGDBD(idGrupo, idCorreo);
            gd.AgregarCorreo(c);
        }
        public void agregarCorreoAGDBD(int idGrupo, int idCorreo)
        {
            WebRequest request = WebRequest.Create("http://localhost/puedes_pasar/GrupoCorreo_rel_Create.php/" + "?GroupId=" + idGrupo + "&MailId=" + idCorreo);
            WebResponse response = request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
            }
            response.Close();
        }

        #endregion relacion Grupo-Mail

        #region REGISTRO TEMPERATURAS
        private DateTime UnixTimeStampToDateTime(int unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public void recuperarRegistrosTemperatura()
        {
            WebRequest request = WebRequest.Create("http://localhost/puedes_pasar/RegistrosTemperatura_GET.php"); // Pasar a variable del webConfig
            WebResponse response = request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                lisRT = JsonConvert.DeserializeObject<List<RegistroTemperatura>>(responseFromServer);
                response.Close();
            }

            foreach (RegistroTemperatura r in lisRT)
            {
                DateTime dt = this.UnixTimeStampToDateTime(r.fecha);
                r.fechaHora = dt;

                if (r.estado == 0)
                {
                    lisRT_SinFiebre.Add(r);
                }
                else
                {
                    if (r.estado == 1)
                    {
                        lisRT_ConFiebre_Abiertos.Add(r);
                    }
                    else //r.estado>1 || r.estado==-1
                    {
                        lisRT_ConFiebre_Cerrados.Add(r);
                    }
                }
            }
        }

        #endregion REGISTRO TEMPERATURAS

        #region PERSONAS
        public int crearPersona(string dni, string sex, string nom, string ape, string fnac, string dir, string loc, string tel, string mail, string os)
        {
            WebRequest request = WebRequest.Create("http://localhost/puedes_pasar/Persona_Create.php" + "?dni=" + dni + "&nom=" + nom + "&ape=" + ape + "&fnac=" + fnac + "&sex=" + sex + "&dir=" + dir + "&loc=" + loc + "&tel=" + tel + "&mail=" + mail + "&prepId=" + os);
            WebResponse response = request.GetResponse();
            int id = 0;

            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                id = JsonConvert.DeserializeObject<int>(responseFromServer);
            }
            /*
            if (id != 0)
            {
                
                Persona p = new GrupoDistribucion(id, nombre, desc);
                this.agregarGrupoDistribucion(gd);
            }*/
            response.Close();
            return id;
        }

        public void registrarFalsoPositivo(string rtId)
        {
            WebRequest request = WebRequest.Create("http://localhost/puedes_pasar/RegistrosTemperatura_FalsoPositivo.php/" + "?regTempId=" + rtId);
            WebResponse response = request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
            }
            response.Close();

            this.cerrarAlerta(int.Parse(rtId), -1);
        }
        public void cerrarAlertaTempDB(string rtId, string idPers)
        {
            WebRequest request = WebRequest.Create("http://localhost/puedes_pasar/RegistrosTemperatura_Update.php/" + "?regTempId=" + rtId + "&idPers=" + idPers);
            WebResponse response = request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
            }
            response.Close();

            this.cerrarAlerta(int.Parse(rtId), int.Parse(idPers));
        }

        public void cerrarAlerta(int regTempId, int idPers)
        {
            RegistroTemperatura rt = this.buscarRegistroTemperaturaAbiertos(regTempId);
            rt.estado = idPers;
            lisRT_ConFiebre_Cerrados.Add(rt);
            lisRT_ConFiebre_Abiertos.Remove(rt);
        }

        public RegistroTemperatura buscarRegistroTemperaturaAbiertos(int id)
        {
            int i = 0;

            while (i < lisRT_ConFiebre_Abiertos.Count() && !lisRT_ConFiebre_Abiertos[i].sos(id))
                i++;

            if (i < lisRT_ConFiebre_Abiertos.Count())
                return lisRT_ConFiebre_Abiertos[i];
            else
                return null;

        }

        #endregion PERSONAS

        #region ALERTA NIVEL AGUA

        public void recuperarAlertasAlcohol()
        {
            WebRequest request = WebRequest.Create("http://localhost/puedes_pasar/alertaNivelAlcohol_Get.php/");
            WebResponse response = request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                lisAlertasAlcohol = JsonConvert.DeserializeObject<List<AlertaNivelAlcohol>>(responseFromServer);
                response.Close();
            }

            foreach (AlertaNivelAlcohol alerta in lisAlertasAlcohol)
            {
                DateTime dt = this.UnixTimeStampToDateTime(alerta.fecha);
                alerta.fechaHora = dt;

                if (alerta.notificado == 0)
                {
                    // mandar mail ;
                    TipoAlerta ta = this.buscarTipoAlerta(2); //2 = Alerta Nivel Agua

                    foreach (GrupoDistribucion gd in ta.ListaGD())
                    {
                        foreach (Correo c in gd.ListaCorreos())
                        {
                            //try catch por si esta mal el mail
                            this.enviarMailAlertaAlcohol(alerta, c.correo);
                            this.cerrarAlertaNivelAlcohol(alerta.id.ToString());
                            alerta.notificado = 1;

                        }
                    }
                }
            }
        }

        public void cerrarAlertaNivelAlcohol(string idAlerta )
        {
            WebRequest request = WebRequest.Create("http://localhost/puedes_pasar/alertaNivelAgua_Notificado.php/" + "?id=" + idAlerta);
            WebResponse response = request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
            }
            response.Close();
        }


        public void recuperarAlertasAgua()
        {
            WebRequest request = WebRequest.Create("http://localhost/puedes_pasar/alertaNivelAgua_Get.php/");
            WebResponse response = request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                lisAl = JsonConvert.DeserializeObject<List<AlertaNivel>>(responseFromServer);
                response.Close();
            }

            if (lisAl.Count > 0)
                alertaNivelAgua = true;
            else
                alertaNivelAgua = false;

            response.Close();
        }
        #endregion ALERTA NIVEL AGUA

        #region TIPO ALERTA
        public void recuperarTiposAlerta()
        {
            WebRequest request = WebRequest.Create("http://localhost/puedes_pasar/TiposAlerta_Get.php");
            WebResponse response = request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                lisTipoAlertas = JsonConvert.DeserializeObject<List<TipoAlerta>>(responseFromServer);
                response.Close();
            }

        }

        private class relTAGD //TipoAlerta GrupoDistribucion
        {
            public int idTipoAlerta { get; set; }
            public int idGrupoDistribucion { get; set; }
        }

        public void recuperarRelacionTipoAlertaGD()
        {

            WebRequest request = WebRequest.Create("http://localhost/puedes_pasar/Alertas_rel_GD_Get.php");
            WebResponse response = request.GetResponse();
            List<relTAGD> lisRelacionTipoAlertaGD = new List<relTAGD>();

            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                lisRelacionTipoAlertaGD = JsonConvert.DeserializeObject<List<relTAGD>>(responseFromServer);
            }

            GrupoDistribucion gd = null;
            TipoAlerta tipoDeAlerta = null;

            foreach (relTAGD r in lisRelacionTipoAlertaGD)
            {
                tipoDeAlerta = this.buscarTipoAlerta(r.idTipoAlerta);

                if (tipoDeAlerta != null)
                {
                    gd = this.buscarGrupoDistribucion(r.idGrupoDistribucion);
                    if (gd != null)
                        tipoDeAlerta.agregarGD(gd);
                }
            }
            response.Close();
        }

        public TipoAlerta buscarTipoAlerta(int id)
        {
            int i = 0;

            while (i < lisTipoAlertas.Count() && !lisTipoAlertas[i].sos(id))
                i++;

            if (i < lisTipoAlertas.Count())
                return lisTipoAlertas[i];
            else
                return null;

        }

        public List<GrupoDistribucion> obtenerGruposDeAlerta(int idTipoAlerta)
        {
            TipoAlerta ta = this.buscarTipoAlerta(idTipoAlerta);
            return (ta.ListaGD());
        }

        ///<summary>
        ///Elimina todos los Grupos de distribucion asociados a un Tipo de Alerta
        ///</summary>
        public void eliminarAsociacionGruposATD(int idTipoAlerta)
        {
            TipoAlerta tipoAlerta = this.buscarTipoAlerta(idTipoAlerta);
            this.eliminarGruposDeTABD(idTipoAlerta);
            tipoAlerta.eliminarTodosGrupos();
        }

        ///<summary>
        ///Elimina todos los Grupos de distribucion asociados a un Tipo de Alerta en la BD
        ///</summary>
        public void eliminarGruposDeTABD(int idTipoAlerta)
        {
            WebRequest request = WebRequest.Create("http://localhost/puedes_pasar/TipoAlerta_DelRel.php/" + "?AlertTypeId=" + idTipoAlerta);
            WebResponse response = request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
            }
            response.Close();
        }

        public void agregarGDATipoAlerta(int idTipoAlerta, int idGrupoDistribucion)
        {
            GrupoDistribucion gd = this.buscarGrupoDistribucion(idGrupoDistribucion);
            TipoAlerta tipoAlerta = this.buscarTipoAlerta(idTipoAlerta);
            this.agregarGDATipoAlertaBD(idTipoAlerta, idGrupoDistribucion);
            tipoAlerta.agregarGD(gd);
        }
        public void agregarGDATipoAlertaBD(int idTipoAlerta, int idGrupoDistribucion)
        {
            WebRequest request = WebRequest.Create("http://localhost/puedes_pasar/TipoAlertaGrupo_rel_Create.php/" + "?AlertTypeId=" + idTipoAlerta + "&GroupId=" + idGrupoDistribucion);
            WebResponse response = request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
            }
            response.Close();
        }
        #endregion TIPO ALERTA


        #region KPI -- INDICADORES
        public void calcularKpis()
        {

            lisIndicadores.Clear();
            lisRT.Clear();
            lisRT_ConFiebre_Abiertos.Clear();
            lisRT_ConFiebre_Cerrados.Clear();
            lisRT_SinFiebre.Clear();

            this.recuperarRegistrosTemperatura();
            
            this.kpi_cantMediciones();
            this.kpi_cantMedicionesFiebre();
            this.kpi_cantMedicionesSinFiebre();
            this.kpi_tempMedia();
            // cantidad mediciones de T realizadas 
            
        }

        public void kpi_cantMediciones()
        {
            Kpi ind = new Kpi("Cantidad mediciones totales", lisRT.Count(), "Mediciones");
            lisIndicadores.Add(ind);

            int cantRegMes = 0;
            int cantRegDia = 0;
            int cantRegSemana = 0;
            int cantRegHora = 0;
            DateTime ultimoIngreso = new DateTime();
            TimeSpan ts;

            foreach (RegistroTemperatura rt in lisRT)
            {
                ts = DateTime.Now - rt.fechaHora;

                if (ts.Days <= 7)
                    cantRegSemana++;

                if (rt.fechaHora.Month == DateTime.Now.Month && rt.fechaHora.Year == DateTime.Now.Year)
                {
                    cantRegMes++;
                    
                    if (rt.fechaHora.Date.Day.Equals(DateTime.Now.Day))
                    {
                        cantRegDia++;
                        TimeSpan tAux = DateTime.Now - rt.fechaHora;
                        if (tAux.Hours <= 1)
                            cantRegHora++;
                    }
                }
                    
                if (rt.fechaHora > ultimoIngreso)
                    ultimoIngreso = rt.fechaHora;
            }

            ind = new Kpi("Cantidad mediciones este Mes" , cantRegMes, "Mediciones");
            lisIndicadores.Add(ind);

            ind = new Kpi("Cantidad mediciones esta Semana", cantRegSemana, "Mediciones");
            lisIndicadores.Add(ind);

            ind = new Kpi("Cantidad mediciones Hoy ", cantRegDia , "Mediciones");
            lisIndicadores.Add(ind);

            ind = new Kpi("Cantidad mediciones ultima Hora ", cantRegHora, "Mediciones");
            lisIndicadores.Add(ind);

            ts = DateTime.Now - ultimoIngreso;

            ind = new Kpi("Cantidad dias ultimo ingreso ", ts.Days , "Dia(s)");
            lisIndicadores.Add(ind);

            ind = new Kpi("Cantidad horas ultimo ingreso ", ts.Hours + (ts.Days*24), "Hora(s)");
            lisIndicadores.Add(ind);

            ind = new Kpi("Cantidad minutos ultimo ingreso ", ts.Minutes + (( ts.Hours + (ts.Days * 24))*60), "Minutos");
            lisIndicadores.Add(ind);
        }
        public void kpi_cantMedicionesFiebre()
        {
            Kpi ind = new Kpi("Cantidad mediciones con Fiebre", lisRT_ConFiebre_Abiertos.Count() + lisRT_ConFiebre_Cerrados.Count() , "Mediciones");
            lisIndicadores.Add(ind);
        }
        public void kpi_cantMedicionesSinFiebre()
        {
            Kpi ind = new Kpi("Cantidad mediciones sin Fiebre", lisRT_SinFiebre.Count(), "Mediciones");
            lisIndicadores.Add(ind);

            
        }

        public void kpi_tempMedia()
        {
            float acum = 0;
            float prom, tempMin = 990, tempMax = 0, falsoPositiv = 0, porcFalsoPositivo = 0;

            foreach (RegistroTemperatura rt in this.lisRT)
            {
                acum += rt.temperatura;

                if (rt.temperatura < tempMin)
                    tempMin = rt.temperatura;

                if (rt.temperatura > tempMax)
                    tempMax = rt.temperatura;

                if (rt.estado == -1)
                    falsoPositiv++;
                    
            }

            prom = acum / lisRT.Count();
            prom = (float)(Math.Truncate((double)prom * 100.0 / 100.0));
            Kpi ind = new Kpi("Temperatura media", prom, "° C");
            lisIndicadores.Add(ind);

            ind = new Kpi("Temperatura Minima", tempMin, "° C");
            lisIndicadores.Add(ind);

            ind = new Kpi("Temperatura Maxima", tempMax, "° C");
            lisIndicadores.Add(ind);

            porcFalsoPositivo = (falsoPositiv * 100) / lisRT_ConFiebre_Cerrados.Count() ;
            porcFalsoPositivo = (float)(Math.Truncate((double)porcFalsoPositivo * 100.0 / 100.0));
            ind = new Kpi("Porcentaje Falsos Positivos", porcFalsoPositivo, "%");
            lisIndicadores.Add(ind);

        }
        #endregion KPI - INDICADORES



        public void enviarMailTemperatura(RegistroTemperatura rt, Persona pers, string correo)  //string , int , string
        {
            string emisor = "PuedesPasARG@gmail.com";
            string destinatario = correo;
            string pwd = "awknrgnjjxtsfhkk";//"PuedesPasar2022";
            string asunto = "Alerta Registro Temperatura Alta! ";
            string cuerpo = "Se registró una alerta de Temperatura: " + System.Environment.NewLine + 
            "Fecha Hora: " + rt.fechaHora.ToString() + System.Environment.NewLine +
            "Temperatura: " + rt.temperatura + System.Environment.NewLine +
            "Dispositivo: " + rt.dispositivo  + System.Environment.NewLine +
            "Nombre: " + pers.nombre + System.Environment.NewLine +
            "Apellido:" + pers.apellido + System.Environment.NewLine +
            "FechaNac:" + pers.fechaNac + System.Environment.NewLine +
            "Sexo:" + pers.sexo + System.Environment.NewLine +
            "Direccion:" + pers.direccion + System.Environment.NewLine +
            "Localidad:" + pers.localidad + System.Environment.NewLine +
            "Telefono:" + pers.telefono + System.Environment.NewLine +
            "Correo:" + pers.correo + System.Environment.NewLine +
            "Obra Social:" + pers.obrasocial;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 25;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(emisor, pwd);

            try
            {

           
            using (var message = new MailMessage(emisor, destinatario))
            {
                message.Priority = MailPriority.High;
                message.Subject = asunto;
                message.Body = cuerpo;
                message.IsBodyHtml = false;
                smtp.Send(message);
            }
            } catch(Exception e)
            {

            }
        }

        public void enviarMailAlertaAlcohol(AlertaNivelAlcohol alerta, string  correo)
        {
            string emisor = "PuedesPasARG@gmail.com";
            string destinatario = correo;
            string pwd = "awknrgnjjxtsfhkk";//"PuedesPasar2022";
            string asunto = "Alerta Nivel Critico Alcohol! ";
            string cuerpo = "Se registró una alerta de Nivel critico de alcohol: " + System.Environment.NewLine +
            "Fecha Hora: " + alerta.fechaHora.ToString() + System.Environment.NewLine ;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 25;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(emisor, pwd);

            try
            {
                using (var message = new MailMessage(emisor, destinatario))
                {
                    message.Priority = MailPriority.High;
                    message.Subject = asunto;
                    message.Body = cuerpo;
                    message.IsBodyHtml = false;
                    smtp.Send(message);
                }
            }
            catch (Exception e)
            {

            }
        }

    }
}