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
            HttpResponseMessage responseMessage = httpClient.GetAsync(baseAddress+ "findProduct/" + id).Result;
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

        //Get: ProductFront

        public ActionResult AllProducts()
        {
            HttpResponseMessage responseMessage = httpClient.GetAsync(baseAddress + "findAllProduct").Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                ViewBag.result = responseMessage.Content.ReadAsAsync<IEnumerable<Models.Product>>().Result;
                return View(ViewBag.result);
            }
            return View();

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
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = httpClient.GetAsync(baseAddress + "findProduct/" + id).Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                ViewBag.result = responseMessage.Content.ReadAsAsync<Product>().Result;
                return View(ViewBag.result);
            }
            return View();

        }

        // GET: Product/Create
        public ActionResult Create()
        {
           
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage responseMessage = httpClient.GetAsync(baseAddress+"findAllCategory").Result;
            if (responseMessage.IsSuccessStatusCode)
            {
               var  categories = responseMessage.Content.ReadAsAsync<IEnumerable<Models.Category>>().Result;
                ViewBag.mycategories = new SelectList(categories,"id","name");

            }
            return View();

        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(Product product)
        {
            try
            {

                var APIResponse = httpClient.PostAsJsonAsync<Product>(baseAddress + "addProduct/",
                product).ContinueWith(postTask => postTask.Result.EnsureSuccessStatusCode());

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
            HttpResponseMessage responseMessage = httpClient.GetAsync(baseAddress + "findProduct/" + id).Result;

            if (responseMessage.IsSuccessStatusCode)
            {
                ViewBag.result = responseMessage.Content.ReadAsAsync<Product>().Result;
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage responseMessage1 = httpClient.GetAsync(baseAddress + "findAllCategory").Result;
                if (responseMessage1.IsSuccessStatusCode)
                {
                    var categories = responseMessage1.Content.ReadAsAsync<IEnumerable<Models.Category>>().Result;
                    ViewBag.mycategories = new SelectList(categories, "id", "name");

                }

                return View(ViewBag.result);
            }

            return View();
        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit( Product product)
        {
            try
            {
                var APIResponse = httpClient.PostAsJsonAsync<Product>(baseAddress + "updateProduct/",
               product).ContinueWith(postTask => postTask.Result.EnsureSuccessStatusCode());

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
            var APIResponse = httpClient.DeleteAsync(baseAddress + "/deleteProduct/" + id).Result;
            if (APIResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        // POST: Product/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Product product)
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
