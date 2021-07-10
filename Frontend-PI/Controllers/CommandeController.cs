using Frontend_PI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Frontend_PI.Controllers
{
    public class CommandeController : Controller
    {
        // GET: Commande
        public ActionResult Index()
        {
            List<Commande> commandes = new List<Commande>();
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://localhost:8081");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = httpClient.GetAsync("SpringMVC/servlet/ListOrder").Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                ViewBag.result = responseMessage.Content.ReadAsAsync<IEnumerable<Commande>>().Result;
                commandes = ViewBag.result;
                return View(commandes);
            }
            //Commande commande = new Commande("AF334F", new DateTime(), "EN COURS", "PAR CARTE", "TUNISIA", "2091");
            return View();
        }

        // GET: Commande/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Commande/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Commande/Create
        [HttpPost]
        public ActionResult Create(Commande commande)
        {
            try
            {
                // TODO: Add insert logic here
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri("http://localhost:8081");
                var response = httpClient.PostAsJsonAsync<Commande>("SpringMVC/servlet/addOrder", commande).ContinueWith((p) => p.Result.EnsureSuccessStatusCode());

                List<Commande> commandes = new List<Commande>();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage responseMessage = httpClient.GetAsync("SpringMVC/servlet/ListOrder").Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    ViewBag.result = responseMessage.Content.ReadAsAsync<IEnumerable<Commande>>().Result;
                    commandes = ViewBag.result;
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Commande/Edit/5
        public ActionResult Edit(int id)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://localhost:8081");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = httpClient.GetAsync("SpringMVC/servlet/findOrderById/" + id).Result;
            ViewBag.result = responseMessage.Content.ReadAsAsync<Commande>().Result;
            return View(ViewBag.result);
        }

        // POST: Commande/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Commande/Delete/5
        public ActionResult Delete(int id)
        {
            
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://localhost:8081");
            httpClient.DeleteAsync("SpringMVC/servlet/deleteOrder/" + id);

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = httpClient.GetAsync("SpringMVC/servlet/findOrderById/" + id).Result;
            ViewBag.result = responseMessage.Content.ReadAsAsync<Commande>().Result;

            return View(ViewBag.result);
        }

        // POST: Commande/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
