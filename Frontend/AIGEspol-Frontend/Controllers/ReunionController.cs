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
    public class ReunionController : Controller
    {
        // GET: Reunion
        public ActionResult Index()
        {
            return View();
        }

        // GET: Reunion/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Reunion/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Reunion/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<string> Create(Reunion reunion)
        {
            try
            {
                string apiResponse = "";
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(reunion), Encoding.UTF8, "application/json");
                    Console.WriteLine(content);
                    using (var response = await httpClient.PostAsync(Constants.ApiUrl + "api/reunion", content))
                    {
                        apiResponse = await response.Content.ReadAsStringAsync();
                        //receivedLista = JsonConvert.DeserializeObject<Lista>(apiResponse);
                    }
                }

                //var conexion = new HttpClient();
                //var lista = new List<int>();
                //lista.Add(2);
                //lista.Add(6);
                //lista.Add(4);
                //conexion.BaseAddress = new Uri(Constants.ApiUrl);
                //conexion.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //HttpResponseMessage respuesta = await conexion.PostAsJsonAsync("api/reunion", new
                //{
                //    idCreador = 1,
                //    asunto = "prueba",
                //    descripcion = "jkkkj",
                //    idLugar = 1,
                //    fechaInicio = DateTime.Now,
                //    fechaFin = DateTime.Now,
                //    idPersonas = lista,
                //});
                //string result = respuesta.Content.ReadAsStringAsync().Result;
                //return result;

                return apiResponse;
            }
            catch
            {
                return "{data:'error'}";
            }
        }
        // GET: Reunion/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Reunion/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Reunion/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Reunion/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}