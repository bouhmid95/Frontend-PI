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
    public class ProductController : Controller
    {
        List<Product> productList = new List<Product>();
        HttpClient httpClient;
        string baseAddress;

        public ProductController()
        {
        
        baseAddress = "http://localhost:8081/SpringMVC/servlet/";
        httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(baseAddress);
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
        }

        [HttpPost]
        public ActionResult Remplissage(int id,int qty)
        {
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = httpClient.GetAsync(baseAddress+ "/findProduct/" + id).Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                Product product = responseMessage.Content.ReadAsAsync<Product>().Result;
                productList.Add(product);

                string message = "SUCCESS";
                return Json(new { Message = message, JsonRequestBehavior.AllowGet });

            }
            string message1 = "Failed";
            return Json(new { Message = message1, JsonRequestBehavior.AllowGet });
           
        }
        // GET: Product
        public ActionResult Index()
        {
            HttpResponseMessage responseMessage = httpClient.GetAsync(baseAddress+ "findAllProduct").Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                ViewBag.result = responseMessage.Content.ReadAsAsync<IEnumerable<Models.Product>>().Result;
                return View(ViewBag.result);
            }
            return View();

        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
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

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Product/Edit/5
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

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Product/Delete/5
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
