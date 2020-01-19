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
        // GET: Lista
        public ActionResult Index()
        {
            return View();
        }

        // GET: Lista/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Lista/Create
        public ActionResult Create()
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
                        //receivedLista = JsonConvert.DeserializeObject<Lista>(apiResponse);
                    }
                }

                return apiResponse;
            }
            catch
            {
                return "{data:'error'}";
            }
        }

        // GET: Lista/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Lista/Edit/5
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

        // GET: Lista/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Lista/Delete/5
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