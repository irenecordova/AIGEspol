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

        public async Task<string> TipoSemana(DateTime fecha)
        {
            HttpResponseMessage respuesta = await this.Conexion.PostAsJsonAsync(Constants.wsTipoSemana, new { fecha = fecha});
            string result = respuesta.Content.ReadAsStringAsync().Result;
            return result;
        }

        public async Task<string> datosMapa(int dia, string tipoSemana)
        {
            //HttpResponseMessage respuesta = await this.Conexion.GetAsync(this.Conexion.BaseAddress + Constants.wsDatosMapa + dia.ToString());
            HttpResponseMessage respuesta = await this.Conexion.PostAsJsonAsync(Constants.wsDatosMapa, new { dia = dia, tipoSemana = tipoSemana });
            string result = respuesta.Content.ReadAsStringAsync().Result;
            return result;
        }



        public async Task<string> personaPorNombreYApellido (string nombres, string apellidos)
        {
            string url = this.Conexion.BaseAddress + Constants.wsPersonaNombreApellido;
            HttpResponseMessage respuesta = await this.Conexion.PostAsJsonAsync("api/prueba1", new { nombres = nombres, apellidos = apellidos });
            string result = respuesta.Content.ReadAsStringAsync().Result;
            return result;
        }

        public async Task<string> estudiantesPorCarrera(int idCarrera)
        {
            string url = this.Conexion.BaseAddress + Constants.wsEstudiantesPorCarrera;
            var contenido = "{\"idPrograma\":" + idCarrera + ",\"";
            HttpContent c = new StringContent(contenido, Encoding.UTF8, "application/json");
            HttpResponseMessage respuesta = await this.Conexion.PostAsync(url, c);
            string result = respuesta.Content.ReadAsStringAsync().Result;
            return result;
        }

        public async Task<string> estudiantesPorFacultad(int idFacultad)
        {
            string url = this.Conexion.BaseAddress + Constants.wsEstudiantesPorFacultad;
            var contenido = "{\"idFacultad\":" + idFacultad + ",\"";
            HttpContent c = new StringContent(contenido, Encoding.UTF8, "application/json");
            HttpResponseMessage respuesta = await this.Conexion.PostAsync(url, c);
            string result = respuesta.Content.ReadAsStringAsync().Result;
            return result;
        }

        public async Task<string> estudiantesPorMateria(int idMateria)
        {
            string url = this.Conexion.BaseAddress + Constants.wsEstudiantesPorMateria;
            var contenido = "{\"idMateria\":" + idMateria + ",\"";
            HttpContent c = new StringContent(contenido, Encoding.UTF8, "application/json");
            HttpResponseMessage respuesta = await this.Conexion.PostAsync(url, c);
            string result = respuesta.Content.ReadAsStringAsync().Result;
            return result;
        }

        public async Task<string> estudiantesPorCurso(int idMateria)
        {
            string url = this.Conexion.BaseAddress + Constants.wsEstudiantesPorCurso;
            var contenido = "{\"idCurso\":" + idMateria + ",\"";
            HttpContent c = new StringContent(contenido, Encoding.UTF8, "application/json");
            HttpResponseMessage respuesta = await this.Conexion.PostAsync(url, c);
            string result = respuesta.Content.ReadAsStringAsync().Result;
            return result;
        }

        public async Task<string> profesoresPorFacultad(int idFacultad)
        {
            string url = this.Conexion.BaseAddress + Constants.wsProfesoresPorFacultad;
            var contenido = "{\"idFacultad\":" + idFacultad + ",\"";
            HttpContent c = new StringContent(contenido, Encoding.UTF8, "application/json");
            HttpResponseMessage respuesta = await this.Conexion.PostAsync(url, c);
            string result = respuesta.Content.ReadAsStringAsync().Result;
            return result;
        }

        public async Task<string> llamadaEsProfesor(int idPersona)
        {
            string url = this.Conexion.BaseAddress + Constants.wsEsProfesor;
            var contenido = "{\"idPersona\":" + idPersona + ",\"";
            HttpContent c = new StringContent(contenido, Encoding.UTF8, "application/json");
            HttpResponseMessage respuesta = await this.Conexion.PostAsync(url, c);
            string result = respuesta.Content.ReadAsStringAsync().Result;
            return result;
        }

        public bool esProfesor(int idPersona)
        {
            var result = llamadaEsProfesor(idPersona).Result;
            var diccionario = JsonConvert.DeserializeObject<Dictionary<string, bool>>(result);
            return diccionario["resultado"];
        }

        public async Task<string> cursosProfesor(int idPersona) 
        {
            string url = this.Conexion.BaseAddress + Constants.wsCursosProfesor;
            var contenido = "{\"idPersona\":" + idPersona + ",\"";
            HttpContent c = new StringContent(contenido, Encoding.UTF8, "application/json");
            HttpResponseMessage respuesta = await this.Conexion.PostAsync(url, c);
            string result = respuesta.Content.ReadAsStringAsync().Result;
            return result;
        }

        public async Task<string> cursosEstudiante(int idPersona)
        {
            string url = this.Conexion.BaseAddress + Constants.wsCursosEstudiante;
            var contenido = "{\"idPersona\":" + idPersona + ",\"";
            HttpContent c = new StringContent(contenido, Encoding.UTF8, "application/json");
            HttpResponseMessage respuesta = await this.Conexion.PostAsync(url, c);
            string result = respuesta.Content.ReadAsStringAsync().Result;
            return result;
        }

        public async Task<string> decanoFacultad(int idFacultad)
        {
            string url = this.Conexion.BaseAddress + Constants.wsDecanoFacultad;
            var contenido = "{\"idFacultad\":" + idFacultad + ",\"";
            HttpContent c = new StringContent(contenido, Encoding.UTF8, "application/json");
            HttpResponseMessage respuesta = await this.Conexion.PostAsync(url, c);
            string result = respuesta.Content.ReadAsStringAsync().Result;
            return result;
        }
        public async Task<string> subdecanoFacultad(int idFacultad)
        {
            string url = this.Conexion.BaseAddress + Constants.wsSubdecanoFacultad;
            var contenido = "{\"idFacultad\":" + idFacultad + ",\"";
            HttpContent c = new StringContent(contenido, Encoding.UTF8, "application/json");
            HttpResponseMessage respuesta = await this.Conexion.PostAsync(url, c);
            string result = respuesta.Content.ReadAsStringAsync().Result;
            return result;
        }

        public async Task<string> horariosPersonas(List<int> idsPersonas)
        {
            string url = this.Conexion.BaseAddress + Constants.wsHorariosPersonas;
            var contenido = "{\"idsPersonas\":" + idsPersonas + ",\"}";
            HttpContent c = new StringContent(contenido, Encoding.UTF8, "application/json");
            HttpResponseMessage respuesta = await this.Conexion.PostAsync(url, c);
            string result = respuesta.Content.ReadAsStringAsync().Result;
            return result;
        }

        public string prueba(List<int> idsPersonas)
        {
            string url = this.Conexion.BaseAddress + Constants.wsHorariosPersonas;
            var contenido = "{\"idsPersonas\":" + JsonConvert.SerializeObject(idsPersonas) + ",\"}";
            return contenido;
        }

        
    }
}
