﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AIGEspol_Frontend.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using AIGEspol_Frontend.Tools;
using System.Text;

namespace AIGEspol_Frontend.Controllers
{
    public class FiltrosController : Controller
    {
        private string FixApiResponseString(string input)
        {
            input = input.TrimStart('\"');
            input = input.TrimEnd('\"');
            input = input.Replace("\\", "");
            return input;
        }

        public async Task<int> GetId()
        {
            var username = new { usuario = "rrivaden" };
            int idPersona;
            StringContent content = new StringContent(JsonConvert.SerializeObject(username), Encoding.UTF8, "application/json");
            string apiResponse;
           
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync(Constants.ApiUrl + "api/idPersonaPorUsuario", content))
                {
                    apiResponse = await response.Content.ReadAsStringAsync();
                    apiResponse = FixApiResponseString(apiResponse);
                    idPersona = JsonConvert.DeserializeObject<Facultad>(apiResponse).idPersona;
                }
            }
            return idPersona;
        }

        public async Task<string> Facultades()
        {
            string apiResponse;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Constants.ApiUrl + "api/facultades"))
                {
                    apiResponse = await response.Content.ReadAsStringAsync();
                    apiResponse = FixApiResponseString(apiResponse);
                    //facultadesList = JsonConvert.DeserializeObject<List<Facultad>>(apiResponse);
                }
            }
            return apiResponse;

        }

        public async Task<string> MateriasUsuario(int idPersona)
        {
            var id = new { idPersona = idPersona };
            //var id = new { idPersona = GetId() };
            StringContent content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
            string apiResponse;

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync(Constants.ApiUrl + "api/cursosRelacionados", content))
                {
                    apiResponse = await response.Content.ReadAsStringAsync();
                    apiResponse = FixApiResponseString(apiResponse);
                }
            }
            return apiResponse;

        }

        public async Task<string> MateriasFacultad(int idFacultad)
        {
            var id = new { idFacultad = idFacultad };
            StringContent content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
            string apiResponse;

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync(Constants.ApiUrl + "api/materiasPorFacultad", content))
                {
                    apiResponse = await response.Content.ReadAsStringAsync();
                    apiResponse = FixApiResponseString(apiResponse);
                }
            }
            return apiResponse;

        }

        public async Task<string> EstudiantesCurso(int idCurso)
        {
            var id = new { idCurso = idCurso };
            StringContent content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
            string apiResponse;

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync(Constants.ApiUrl + "api/estudiantesPorCurso", content))
                {
                    apiResponse = await response.Content.ReadAsStringAsync();
                    apiResponse = FixApiResponseString(apiResponse);
                }
            }
            return apiResponse;

        }

        public async Task<string> DecanosFacultad(int IdFacultad)
        {
            var id = new { IdFacultad = IdFacultad };
            StringContent content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
            string apiResponse;

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync(Constants.ApiUrl + "api/dirigentesFacultad", content))
                {
                    apiResponse = await response.Content.ReadAsStringAsync();
                    apiResponse = FixApiResponseString(apiResponse);
                }
            }
            return apiResponse;

        }

        public async Task<string> Decanos()
        {
            string apiResponse;

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Constants.ApiUrl + "api/directivos"))
                {
                    apiResponse = await response.Content.ReadAsStringAsync();
                    apiResponse = FixApiResponseString(apiResponse);
                }
            }
            return apiResponse;

        }

        public async Task<string> DocentesFacultad(int IdFacultad)
        {
            var id = new { IdFacultad = IdFacultad };
            StringContent content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
            string apiResponse;

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync(Constants.ApiUrl + "api/profesoresPorFacultad", content))
                {
                    apiResponse = await response.Content.ReadAsStringAsync();
                    apiResponse = FixApiResponseString(apiResponse);
                }
            }
            return apiResponse;

        }

        public async Task<string> Docentes()
        {
            string apiResponse;

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Constants.ApiUrl + "api/profesores"))
                {
                    apiResponse = await response.Content.ReadAsStringAsync();
                    apiResponse = FixApiResponseString(apiResponse);
                }
            }
            return apiResponse;

        }

        public async Task<string> EstudiantesMateria(int IdMateria)
        {
            var id = new { IdMateria = IdMateria };
            StringContent content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
            string apiResponse;

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync(Constants.ApiUrl + "api/estudiantesPorMateria", content))
                {
                    apiResponse = await response.Content.ReadAsStringAsync();
                    apiResponse = FixApiResponseString(apiResponse);
                }
            }
            return apiResponse;

        }

        public async Task<string> EstudiantesFacultad(int IdFacultad)
        {
            var id = new { IdFacultad = IdFacultad };
            StringContent content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
            string apiResponse;

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync(Constants.ApiUrl + "api/estudiantesPorFacultad", content))
                {
                    apiResponse = await response.Content.ReadAsStringAsync();
                    apiResponse = FixApiResponseString(apiResponse);
                }
            }
            return apiResponse;

        }

        public async Task<string> Buscar(string nombrePersona)
        {
            var id = new { nombre = nombrePersona };
            StringContent content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
            string apiResponse;

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync(Constants.ApiUrl + "api/personasPorNombreYApellido", content))
                {
                    apiResponse = await response.Content.ReadAsStringAsync();
                    apiResponse = FixApiResponseString(apiResponse);
                }
            }
            return apiResponse;

        }

        public async Task<string> Zonas()
        {
            string apiResponse;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Constants.ApiUrl + "api/espacio/zonas"))
                {
                    apiResponse = await response.Content.ReadAsStringAsync();
                    apiResponse = FixApiResponseString(apiResponse);
                }
            }
            return apiResponse;

        }

        public async Task<string> BloquesZona(int zona)
        {
            string apiResponse;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Constants.ApiUrl + "api/espacio/bloques/zona/" + zona))
                {
                    apiResponse = await response.Content.ReadAsStringAsync();
                    apiResponse = FixApiResponseString(apiResponse);
                }
            }
            return apiResponse;

        }

        public async Task<string> LugaresBloque(int idBloque, DateTime fechaInicio, DateTime fechaFin)
        {
            var id = new { idBloque = idBloque, fechaInicio  = fechaInicio, fechaFin = fechaFin };
            StringContent content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
            string apiResponse;

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync(Constants.ApiUrl + "api/aulasDisponibles", content))
                {
                    apiResponse = await response.Content.ReadAsStringAsync();
                    apiResponse = FixApiResponseString(apiResponse);
                }
            }
            return apiResponse;

        }
    }
}