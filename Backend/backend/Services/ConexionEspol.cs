﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using backend.Services;
using backend.Tools;

namespace backend.Services
{
    public class ConexionEspol
    {
        private HttpClient Conexion { get; }

        public ConexionEspol()
        {
            this.Conexion = new HttpClient();
            this.Conexion.BaseAddress = new Uri(Constants.UrlWebServices);

            //En caso de que necesitemos headers (Que es muy probable)
            //conexion.DefaultRequestHeaders.Add("Nombre del header","Valor del header");
        }

        public async Task<string> datosMapa(int dia)
        {
            HttpResponseMessage respuesta = await this.Conexion.GetAsync(this.Conexion.BaseAddress + Constants.wsDatosMapa + dia.ToString());
            string result = respuesta.Content.ReadAsStringAsync().Result;
            return result;
        }

        public async Task<string> datosPersona(int idPersona)
        {
            HttpResponseMessage respuesta = await this.Conexion.GetAsync(this.Conexion.BaseAddress + Constants.wsDatosMapa + idPersona.ToString());
            string result = respuesta.Content.ReadAsStringAsync().Result;
            return result;
        }

        /* Ejemplo de cómo hacer un get
        public async Task<IEnumerable<GitHubIssue>> GetAspNetDocsIssues()
        {
            var response = await Client.GetAsync(
                "/repos/aspnet/AspNetCore.Docs/issues?state=open&sort=created&direction=desc");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync
                <IEnumerable<GitHubIssue>>(responseStream);
        }
        */


    }
}
