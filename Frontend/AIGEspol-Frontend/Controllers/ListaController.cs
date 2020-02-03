using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AIGEspol_Frontend.Models;
using System.Net.Http;
using Newtonsoft.Json;
using AIGEspol_Frontend.Tools;
using System.Text;

namespace AIGEspol_Frontend.Controllers
{
    public class ListaController : Controller
    {
        private string FixApiResponseString(string input)
        {
            input = input.TrimStart('\"');
            input = input.TrimEnd('\"');
            input = input.Replace("\\", "");
            return input;
        }

        // GET: Lista
        public ActionResult Index()
        {
            return View();
        }


        // POST: Lista/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<string> Create(Lista lista)
        {
            try
            {
                string apiResponse = "";
                Lista receivedLista = new Lista();
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(lista), Encoding.UTF8, "application/json");
                    Console.WriteLine(content);
                    using (var response = await httpClient.PostAsync(Constants.ApiUrl + "api/lista_personalizada", content))
                    {
                        apiResponse = await response.Content.ReadAsStringAsync();
                        apiResponse = FixApiResponseString(apiResponse);
                    }
                }

                return apiResponse;
            }
            catch
            {
                return "{data:'error'}";
            }
        }

        public async Task<string> Listas(int idPersona)
        {
            var id = new { idPersona = idPersona };
            StringContent content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
            string apiResponse;

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync(Constants.ApiUrl + "api/lista_personalizada/persona", content))
                {
                    apiResponse = await response.Content.ReadAsStringAsync();
                    apiResponse = FixApiResponseString(apiResponse);
                }
            }
            return apiResponse;

        }

        public async Task<string> PersonasLista(int idLista)
        {
            string apiResponse;

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Constants.ApiUrl + "api/lista_personalizada/" + idLista + "/personas"))
                {
                    apiResponse = await response.Content.ReadAsStringAsync();
                    apiResponse = FixApiResponseString(apiResponse);
                }
            }
            return apiResponse;

        }
    }
}