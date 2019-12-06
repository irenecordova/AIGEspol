using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AIGEspol_Frontend.Tools;

namespace AIGEspol_Frontend.Services
{
    public class ConexionBackend
    {
        private HttpClient Conexion { get; }

        public ConexionBackend(HttpClient conexion)
        {
            conexion.BaseAddress = new Uri(Constants.ApiUrl);
            
            //En caso de que necesitemos headers (Que es muy probable)
            //conexion.DefaultRequestHeaders.Add("Nombre del header","Valor del header");
            
            this.Conexion = conexion;
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
