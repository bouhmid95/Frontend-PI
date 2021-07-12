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
                var response = httpClient.PostAsJsonAsync<FeedBack>("SpringMVC/servlet/addOrder", feedBack).ContinueWith((p) => p.Result.EnsureSuccessStatusCode());

                List<Commande> commandes = new List<Commande>();
                HttpClient http = new HttpClient();
                httpClient.BaseAddress = new Uri("http://localhost:8081");
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
