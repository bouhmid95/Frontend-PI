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
    public class DeliveryManController : Controller
    {
        System.Net.Http.HttpClient httpClient;
        string baseAddress;

        public DeliveryManController()
        {
            baseAddress = "http://localhost:8081/SpringMVC/servlet/";
            httpClient = new System.Net.Http.HttpClient();
            httpClient.BaseAddress = new Uri(baseAddress);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public ActionResult AffecterCommande()
        {

            var resp = httpClient.GetAsync(baseAddress + "ListOrder/");
            ViewBag.result = resp.Result.Content.ReadAsAsync<IEnumerable<Commande>>().Result;
            ViewBag.Title = "Liste des Commandes";
            return View(ViewBag.result);
       
        }

        public ActionResult AffecterLivreur( int id,int cp,string refer)
        {
            ViewBag.id = id;
            ViewBag.cp = cp;
            ViewBag.refer = refer;
            var resp = httpClient.GetAsync(baseAddress + "dm/");
            ViewBag.result = resp.Result.Content.ReadAsAsync<IEnumerable<DeliveryMan>>().Result;

            return View(ViewBag.result);
        }

        public ActionResult AffecterLivreurExecute(int id, int cp, string refer)
        {
            ViewBag.id = id;
            ViewBag.cp = cp;
            ViewBag.refer = refer; 
              var resp = httpClient.GetAsync(baseAddress + "dm/optimisationAlgo/"+ cp);
            ViewBag.result = resp.Result.Content.ReadAsAsync<IEnumerable<DeliveryMan>>().Result;
            return View(ViewBag.result);
        }






        // GET: DeliveryMan
        public ActionResult Index()
        {
            var resp = httpClient.GetAsync(baseAddress + "dm/");
            ViewBag.result = resp.Result.Content.ReadAsAsync<IEnumerable<DeliveryMan>>().Result;
            ViewBag.Title = "Liste des Livreurs";

            return View(ViewBag.result);

        }

        public DeliveryManStat StatByDeliveryMan(int id)
        {
            var resp = httpClient.GetAsync(baseAddress + "dm/getDeliveryManStatById/" + id);

            var result = resp.Result.Content.ReadAsAsync<DeliveryManStat>().Result;
            return (DeliveryManStat)result;

        }

        public ActionResult CommandeByDeliveryMan(int id)
        {
            var resp = httpClient.GetAsync(baseAddress + "dm/getListOrdersByDeliveryManId___Criteria/"+id);
            ViewBag.result = resp.Result.Content.ReadAsAsync<IEnumerable<Commande>>().Result;
            ViewBag.Title = "Liste des Livreurs";
            ViewBag.stat = StatByDeliveryMan(id);

            if (ViewBag.result!=null)
            {
                return View(ViewBag.result);
            }

            return View();

        }

        // GET: DeliveryMan/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DeliveryMan/Create
        public ActionResult Create()
        {
            return View();
        }

     
     

        // POST: DeliveryMan/Create
        [HttpPost]
        public ActionResult Create(DeliveryMan deliveryman)
        {
            try
            {
                var APIResponse = httpClient.PostAsJsonAsync<DeliveryMan>(baseAddress + "/dm/adddvm",
                deliveryman).ContinueWith(postTask => postTask.Result.EnsureSuccessStatusCode());
                return RedirectToAction("Index");
            }
            catch
            {
                return View("Index");
            }
        }



        // GET: DeliveryMan/Edit/5
        public ActionResult Edit(int id)
        {
            var resp = httpClient.GetAsync(baseAddress + "dm/findDeliveryManById/"+id);
            ViewBag.result = resp.Result.Content.ReadAsAsync<DeliveryMan>().Result;
            return View(ViewBag.result);
        }

        // POST: DeliveryMan/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, DeliveryMan deliveryman)
        {
            try
            {
                var APIResponse = httpClient.PostAsJsonAsync<DeliveryMan>(baseAddress + "/dm/updatedvm",
                deliveryman).ContinueWith(postTask => postTask.Result.EnsureSuccessStatusCode());
                return RedirectToAction("Index");
            }
            catch
            {
                return View("Index");
            }
        }

        // GET: DeliveryMan/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var APIResponse = httpClient.DeleteAsync(baseAddress + "/dm/deletedvm/" + id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View("Index");
            }
        }

        // POST: DeliveryMan/Delete/5
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
