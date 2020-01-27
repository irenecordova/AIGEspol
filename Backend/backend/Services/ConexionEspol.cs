using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using backend.Services;
using backend.Tools;
using backend.Models.Envios;
using System.Text;
using Newtonsoft.Json;

namespace backend.Services
{
    public class ConexionEspol
    {
        private HttpClient Conexion { get; }

        public ConexionEspol()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.Proxy = new WebProxy();
            this.Conexion = new HttpClient(handler);
            this.Conexion.BaseAddress = new Uri(Constants.UrlWebServices);
            this.Conexion.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //En caso de que necesitemos headers (Que es muy probable)
            //conexion.DefaultRequestHeaders.Add("Nombre del header","Valor del header");
        }

        public async Task<string> PeriodoDeFecha(DateTime fecha)
        {
            HttpResponseMessage respuesta = await this.Conexion.PostAsJsonAsync("periodoAcademico/periodoDeFecha", new { fecha = fecha });
            string result = respuesta.Content.ReadAsStringAsync().Result;
            return result;
        }

        public async Task<string> TipoSemana(DateTime fecha)
        {
            HttpResponseMessage respuesta = await this.Conexion.PostAsJsonAsync(Constants.wsTipoSemana, new { fecha = fecha});
            string result = respuesta.Content.ReadAsStringAsync().Result;
            return result;
        }

        public async Task<string> datosMapa(DateTime fecha, int dia, string tipoSemana)
        {
            //HttpResponseMessage respuesta = await this.Conexion.GetAsync(this.Conexion.BaseAddress + Constants.wsDatosMapa + dia.ToString());
            HttpResponseMessage respuesta = await this.Conexion.PostAsJsonAsync(Constants.wsDatosMapa, new { fecha = fecha, dia = dia, tipoSemana = tipoSemana });
            string result = respuesta.Content.ReadAsStringAsync().Result;
            return result;
        }

        public async Task<string> estadisticas(DateTime fecha, int dia, string tipoSemana)
        {
            HttpResponseMessage respuesta = await this.Conexion.PostAsJsonAsync(Constants.wsEstadisticas, new { fecha = fecha, dia = dia, tipoSemana = tipoSemana });
            string result = respuesta.Content.ReadAsStringAsync().Result;
            return result;
        }

        public async Task<string> horariosPersonas(List<int> ids, DateTime fecha)
        {
            HttpResponseMessage respuesta = await this.Conexion.PostAsJsonAsync(Constants.wsHorariosPersonas, new { fecha = fecha, idsPersonas = ids });
            string result = respuesta.Content.ReadAsStringAsync().Result;
            return result;
        }

        public async Task<string> personaPorNombreYApellido (string nombre)
        {
            HttpResponseMessage respuesta = await this.Conexion.PostAsJsonAsync(Constants.wsPersonaNombreCompleto, new { nombre = nombre});
            string result = respuesta.Content.ReadAsStringAsync().Result;
            return result;
        }

        public async Task<string> estudiantesPorCarrera(int idCarrera)
        {
            HttpResponseMessage respuesta = await this.Conexion.PostAsJsonAsync(
                Constants.wsEstudiantesPorCarrera, new { idPrograma = idCarrera});
            string result = respuesta.Content.ReadAsStringAsync().Result;
            return result;
        }

        public async Task<string> estudiantesPorFacultad(int idFacultad)
        {
            HttpResponseMessage respuesta = await this.Conexion.PostAsJsonAsync(
                Constants.wsEstudiantesPorFacultad, new { idFacultad = idFacultad });
            string result = respuesta.Content.ReadAsStringAsync().Result;
            return result;
        }

        public async Task<string> estudiantesPorMateria(int idMateria)
        {
            HttpResponseMessage respuesta = await this.Conexion.PostAsJsonAsync(
                Constants.wsEstudiantesPorMateria, new { idMateria = idMateria });
            string result = respuesta.Content.ReadAsStringAsync().Result;
            return result;
        }

        public async Task<string> estudiantesPorCurso(int idCurso)
        {
            HttpResponseMessage respuesta = await this.Conexion.PostAsJsonAsync(
                Constants.wsEstudiantesPorCurso, new { idCurso = idCurso });
            string result = respuesta.Content.ReadAsStringAsync().Result;
            return result;
        }

        public async Task<string> profesoresPorFacultad(int idFacultad)
        {
            HttpResponseMessage respuesta = await this.Conexion.PostAsJsonAsync(
                Constants.wsProfesoresPorFacultad, new { idFacultad = idFacultad });
            string result = respuesta.Content.ReadAsStringAsync().Result;
            return result;
        }

        public async Task<string> profesoresPorMateria(int idMateria)
        {
            HttpResponseMessage respuesta = await this.Conexion.PostAsJsonAsync(
                Constants.wsProfesoresPorMateria, new { idMateria = idMateria });
            string result = respuesta.Content.ReadAsStringAsync().Result;
            return result;
        }

        public async Task<string> cursosRelacionados(int idPersona)
        {
            HttpResponseMessage respuesta = await this.Conexion.PostAsJsonAsync(
                Constants.wsCursosRelacionados, new { idPersona = idPersona });
            string result = respuesta.Content.ReadAsStringAsync().Result;
            return result;
        }

        public async Task<string> decanoFacultad(int idFacultad)
        {
            HttpResponseMessage respuesta = await this.Conexion.PostAsJsonAsync(
                Constants.wsDecanoFacultad, new { idFacultad = idFacultad });
            string result = respuesta.Content.ReadAsStringAsync().Result;
            return result;
        }
        public async Task<string> subdecanoFacultad(int idFacultad)
        {
            HttpResponseMessage respuesta = await this.Conexion.PostAsJsonAsync(
                Constants.wsSubdecanoFacultad, new { idFacultad = idFacultad });
            string result = respuesta.Content.ReadAsStringAsync().Result;
            return result;
        }
        
        public async Task<string> facultades()
        {
            HttpResponseMessage respuesta = await this.Conexion.GetAsync(Constants.UrlWebServices + Constants.wsFacultades);
            string result = respuesta.Content.ReadAsStringAsync().Result;
            return result;
        }

        public async Task<string> carreras()
        {
            HttpResponseMessage respuesta = await this.Conexion.GetAsync(Constants.UrlWebServices + Constants.wsCarreras);
            string result = respuesta.Content.ReadAsStringAsync().Result;
            return result;
        }

        public async Task<string> carrerasPorFacultad(int idFacultad)
        {
            HttpResponseMessage respuesta = await this.Conexion.GetAsync(Constants.UrlWebServices + Constants.wsCarrerasPorFacultad + idFacultad.ToString());
            string result = respuesta.Content.ReadAsStringAsync().Result;
            return result;
        }

        public async Task<string> idPersonaPorUsuario(string usuario)
        {
            HttpResponseMessage respuesta = await this.Conexion.PostAsJsonAsync(
                Constants.wsIdPorUsuario, new { username = usuario });
            string result = respuesta.Content.ReadAsStringAsync().Result;
            return result;
        }

        public async Task<string> materiasPorFacultad(int idFacultad)
        {
            HttpResponseMessage respuesta = await this.Conexion.PostAsJsonAsync(
                Constants.wsMateriasPorFacultad, new { idFacultad = idFacultad });
            string result = respuesta.Content.ReadAsStringAsync().Result;
            return result;
        }

        public async Task<string> directivosTodos()
        {
            HttpResponseMessage respuesta = await this.Conexion.GetAsync(Constants.UrlWebServices + Constants.wsDecanosSubdecanosTodos);
            string result = respuesta.Content.ReadAsStringAsync().Result;
            return result;
        }

        public async Task<string> profesoresTodos()
        {
            HttpResponseMessage respuesta = await this.Conexion.GetAsync(Constants.UrlWebServices + Constants.wsProfesoresTodos);
            string result = respuesta.Content.ReadAsStringAsync().Result;
            return result;
        }
    }
}
