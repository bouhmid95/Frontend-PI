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
    public class StockController : Controller
    {
        HttpClient httpClient;
        string baseAddress;

        public StockController()
        {
            baseAddress = "http://localhost:8081/SpringMVC/servlet/";
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseAddress);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        // GET: Stock
        public ActionResult Index()
        {
            HttpResponseMessage responseMessage = httpClient.GetAsync(baseAddress + "findAllStock").Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                ViewBag.result = responseMessage.Content.ReadAsAsync<IEnumerable<Models.Stock>>().Result;
                return View(ViewBag.result);
            }
            return View();
        }

        // GET: Stock/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Stock/Create
        public ActionResult Create()
        {
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = httpClient.GetAsync(baseAddress + "getAvailableProductsToStock").Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                var products = responseMessage.Content.ReadAsAsync<IEnumerable<Models.Product>>().Result;
                ViewBag.products = new SelectList(products, "id", "title");

            }
            return View();
        }

        // POST: Stock/Create
        [HttpPost]
        public ActionResult Create(Stock stock)
        {
            try
            {
                var APIResponse = httpClient.PostAsJsonAsync<Stock>(baseAddress + "addStock/",
                stock).ContinueWith(postTask => postTask.Result.EnsureSuccessStatusCode());

                return RedirectToAction("Index");
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Stock/Edit/5
        public ActionResult Edit(int id)
        {
            HttpResponseMessage responseMessage = httpClient.GetAsync(baseAddress + "findStock/" + id).Result;

            if (responseMessage.IsSuccessStatusCode)
            {
                ViewBag.result = responseMessage.Content.ReadAsAsync<Stock>().Result;

                return View(ViewBag.result);
            }
            return View();
        }

        // POST: Stock/Edit/5
        [HttpPost]
        public ActionResult Edit( Stock stock)
        {
            try
            {
                Product product = new Product();
                
              
                
                var APIResponse = httpClient.PostAsJsonAsync<Stock>(baseAddress + "updateStock/",
                stock).ContinueWith(postTask => postTask.Result.EnsureSuccessStatusCode());

                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Stock/Delete/5
        public ActionResult Delete(int id)
        {

            var APIResponse = httpClient.DeleteAsync(baseAddress + "/deleteStock/" + id).Result;
            if (APIResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        // POST: Stock/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Stock stock)
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
