using System;
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
    public class MapaController : Controller
    {
        private string FixApiResponseString(string input)
        {
            input = input.TrimStart('\"');
            input = input.TrimEnd('\"');
            input = input.Replace("\\", "");
            return input;
        }

        // GET: Mapa
        public ActionResult Index()
        {
            return View();
        }

        public async Task<string> Generar(DateTime fecha)
        {
            var id = new { Fecha = fecha };
            StringContent content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
            string apiResponse;

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync(Constants.ApiUrl + "api/datosMapa", content))
                {
                    apiResponse = await response.Content.ReadAsStringAsync();
                    apiResponse = FixApiResponseString(apiResponse);
                }
            }
            return apiResponse;

        }

        public async Task<string> Estadisticas(DateTime fecha)
        {
            var id = new { Fecha = fecha };
            StringContent content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
            string apiResponse;

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync(Constants.ApiUrl + "api/estadisticas", content))
                {
                    apiResponse = await response.Content.ReadAsStringAsync();
                    apiResponse = FixApiResponseString(apiResponse);
                }
            }
            return apiResponse;

        }

    }
}