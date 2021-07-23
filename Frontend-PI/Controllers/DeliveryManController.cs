using Frontend_PI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

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

        public ActionResult Statistique()
        {
            int nbCommande = 0;
            int commandeLivre = 0;
            List<Commande> listCommande;
            var resp = httpClient.GetAsync(baseAddress + "ListOrder/");
            listCommande = (List<Commande>)resp.Result.Content.ReadAsAsync<IEnumerable<Commande>>().Result;
            if (listCommande != null)
            {
                nbCommande = listCommande.Count();

                foreach(Commande elm in listCommande)
                {
                    if (elm.status == "DELIVERED")
                        commandeLivre = commandeLivre + 1;
                }
            }
            var respStat = httpClient.GetAsync(baseAddress + "dm/getAllDeliveryManStat/");

            List<DeliveryManStat> listDeliveryManStat = (List<DeliveryManStat>)respStat.Result.Content.ReadAsAsync<IEnumerable<DeliveryManStat>>().Result;
            if (listDeliveryManStat != null)
            {
              

                foreach (DeliveryManStat elm in listDeliveryManStat)
                {
                    //ici
                      if(commandeLivre!=0)
                      elm.taux_livraison = elm.orders_delivered*100/ elm.total_orders;

                    if (nbCommande != 0)
                        elm.taux_affectation = elm.total_orders * 100 / nbCommande;

                }
                ViewBag.totalC = nbCommande;
                ViewBag.totalL = commandeLivre;
                ViewBag.list = listDeliveryManStat;
            }
            return View(ViewBag.list);
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

        [HttpPost]
        public ActionResult AffecterCommandeLivreur(int idCommande, int idLivreur)
        {
            // Commande c;
            // AffectationDto affect = new AffectationDto();
            // var resp = httpClient.GetAsync(baseAddress + "/findOrderById/"+cId);

            // c = resp.Result.Content.ReadAsAsync<Commande>().Result;

            // List<Commande> list = new List<Commande>();
            // list.Add(c);
            // affect.orders = list;
            // affect.idDeliveryMan = idLivreur;
            // string json = JsonConvert.SerializeObject(affect);



            //var httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(baseAddress + "dm/affectOrdersToDeliveryMan/");
            // httpWebRequest.ContentType = "application/json";
            // httpWebRequest.Method = "POST";


            // using (var streamWriter = new System.IO.StreamWriter(httpWebRequest.GetRequestStream()))
            // {

            //    streamWriter.Write(json);
            // }

            // var httpResponse = (System.Net.HttpWebResponse)httpWebRequest.GetResponse();
            // using (var streamReader = new System.IO.StreamReader(httpResponse.GetResponseStream()))
            //  {
            //     
            // }
            // var resp2 = httpClient.GetAsync(baseAddress + "dm/getListOrdersByDeliveryManId___Criteria/" + idLivreur);
            // ViewBag.result = resp2.Result.Content.ReadAsAsync<IEnumerable<Commande>>().Result;
            // ViewBag.Title = "Liste des Livreurs";
            // ViewBag.stat = StatByDeliveryMan(idLivreur);
            // ViewBag.idLivreur = idLivreur;
            //  ViewBag.idCommande = cId;


            //  if (ViewBag.result != null)
            // {
            //     return View(ViewBag.result);
            //  }
            var resp = httpClient.GetAsync(baseAddress + "dm/affectOrdersToDeliveryMan/" + idLivreur+ "/"+ idCommande);
              

            return Json(idLivreur, JsonRequestBehavior.AllowGet);
        }

        

        public ActionResult AffecterLivreurExecute(int id, int cp, string refer)
        {
            ViewBag.id = id;
            ViewBag.cp = cp;
            ViewBag.refer = refer; 
              var resp = httpClient.GetAsync(baseAddress + "dm/optimisationAlgo/"+ cp);
            ViewBag.result = resp.Result.Content.ReadAsAsync<IEnumerable<DeliveryMan>>().Result;

            if (ViewBag.result != null)
            {
                return View(ViewBag.result);
            }
            return View();
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
            ViewBag.idLivreur = id;

            if (ViewBag.result!=null)
            {
                return View(ViewBag.result);
            }

            return View();

        }

        [HttpPost]
        public ActionResult SupprimerAffectation(int idCommande)
        {

            var resp = httpClient.GetAsync(baseAddress + "dm/supprimerAffectation/" + idCommande);
            if (resp != null)
            {
                ViewBag.result = resp.Result.Content.ReadAsAsync<Commande>().Result;
            }


              return Json("", JsonRequestBehavior.AllowGet);
            // return CommandeByDeliveryMan(idLivreur);

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
