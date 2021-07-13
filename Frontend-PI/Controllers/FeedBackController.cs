using Frontend_PI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace Frontend_PI.Controllers
{
    public class FeedBackController : Controller
    {
        // GET: FeedBack
        public ActionResult Index()
        {
            List<FeedBack> feedBacks = new List<FeedBack>();
            HttpClient httpClient = new HttpClient();

            httpClient.BaseAddress = new Uri("http://localhost:8081");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = httpClient.GetAsync("SpringMVC/servlet/ListFeedback/").Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                ViewBag.result = responseMessage.Content.ReadAsAsync<IEnumerable<Commande>>().Result;
                feedBacks = ViewBag.result;
                return View(feedBacks);
            }
            //Commande commande = new Commande("AF334F", new DateTime(), "EN COURS", "PAR CARTE", "TUNISIA", "2091");
            return View();
        }

        // GET: FeedBack/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FeedBack/Create
        public ActionResult Create(FeedBack feedBack)
        {
            try
            {
                // TODO: Add insert logic here
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri("http://localhost:8081");

                var response = httpClient.PostAsJsonAsync("SpringMVC/servlet/addOrder", feedBack).Result;
                int statusCode = (int)response.StatusCode;

                //var response = httpClient.PostAsJsonAsync<Commande>("SpringMVC/servlet/addOrder", commande).ContinueWith((p) => p.Result.EnsureSuccessStatusCode());
                if (statusCode == 200)
                    return RedirectToAction("Index");
                else
                    return View();
            }
            catch
            {
                return View();
            }
        }

        // POST: FeedBack/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: FeedBack/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FeedBack/Edit/5
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

        // GET: FeedBack/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FeedBack/Delete/5
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
