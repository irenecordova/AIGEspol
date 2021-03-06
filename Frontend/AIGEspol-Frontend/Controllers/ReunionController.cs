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
    public class ReunionController : Controller
    {
        private string FixApiResponseString(string input)
        {
            input = input.TrimStart('\"');
            input = input.TrimEnd('\"');
            input = input.Replace("\\", "");
            return input;
        }

        public async Task<string> Reuniones(int idPersona)
        {
            //var id = new { idPersona = 90118 };
            //StringContent content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
            string apiResponse;

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Constants.ApiUrl + "api/reunion/usuario/" + idPersona))
                {
                    apiResponse = await response.Content.ReadAsStringAsync();
                    apiResponse = FixApiResponseString(apiResponse);
                }
            }
            return apiResponse;

        }

        public async Task<string> Invitaciones(int idPersona)
        {
            var id = new { idPersona = idPersona };
            StringContent content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
            string apiResponse;

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync(Constants.ApiUrl + "api/invitacion/reuniones", content))
                {
                    apiResponse = await response.Content.ReadAsStringAsync();
                    apiResponse = FixApiResponseString(apiResponse);
                }
            }
            return apiResponse;

        }

        public async Task<string> Cancelar(int idReunion)
        {
            var id = new { idReunion = idReunion };
            StringContent content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
            string apiResponse;

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync(Constants.ApiUrl + "api/reunion/cancelar", content))
                {
                    apiResponse = await response.Content.ReadAsStringAsync();
                    apiResponse = FixApiResponseString(apiResponse);
                }
            }
            return apiResponse;

        }

        public async Task<string> Aceptar(int idInvitacion)
        {
            var id = new { idInvitacion = idInvitacion };
            StringContent content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
            string apiResponse;

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync(Constants.ApiUrl + "api/invitacion/aceptar", content))
                {
                    apiResponse = await response.Content.ReadAsStringAsync();
                    apiResponse = FixApiResponseString(apiResponse);
                }
            }
            return apiResponse;

        }

        public async Task<string> Rechazar(int idInvitacion)
        {
            var id = new { idInvitacion = idInvitacion };
            StringContent content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
            string apiResponse;

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync(Constants.ApiUrl + "api/invitacion/rechazar", content))
                {
                    apiResponse = await response.Content.ReadAsStringAsync();
                    apiResponse = FixApiResponseString(apiResponse);
                }
            }
            return apiResponse;

        }

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

                    using (var response = await httpClient.PostAsync(Constants.ApiUrl + "api/reunion", content))
                    {
                        apiResponse = await response.Content.ReadAsStringAsync();
                    }
                }

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