using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Frontend_PI.Models;
using Newtonsoft.Json;

namespace Frontend_PI.Controllers
{

   
    public class DMController : Controller
    {

        System.Net.Http.HttpClient httpClient;
        string baseAddress;
        public DMController()
        {
            baseAddress = "http://localhost:8081/SpringMVC/servlet/";
            httpClient = new System.Net.Http.HttpClient();
            httpClient.BaseAddress = new Uri(baseAddress);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        // GET: DM
       
        public ActionResult Index()
        {
            // list des commandes

            var respCommande = httpClient.GetAsync(baseAddress + "ListOrder/");
            var resultCommande = (List<Commande>)respCommande.Result.Content.ReadAsAsync<IEnumerable<Commande>>().Result;
            var resp = httpClient.GetAsync(baseAddress + "dm/");
            var result = (List<DeliveryMan>)resp.Result.Content.ReadAsAsync<IEnumerable<DeliveryMan>>().Result;
           
            AffectationDto listAffectation = new AffectationDto();
            List<SelectListItem> listDeliveryManLoad = new List<SelectListItem>();
            listDeliveryManLoad.Add(new SelectListItem()
            {
                Text = "choisir",
                Value = null
            });
            foreach (DeliveryMan elm in result)
            {
                listDeliveryManLoad.Add(new SelectListItem()
                {
                    Text = elm.firstName,
                    Value = elm.id.ToString()
                });
           
            }

            listAffectation.affected = listDeliveryManLoad;
            //test if is cheked 
           // resultCommande.ForEach((x) =>
           // {
             //   if (x.id == 1)
             //   {
            //        x.isChecked = true;
            //    }
        //    });

            listAffectation.orders = resultCommande;
            return View(listAffectation);

        }
         
       [HttpPost]
      public ActionResult index(int idDeliveryMan, List<int> listAffectation)
        {

            Commande c;
            AffectationDto affect = new AffectationDto();
            List<Commande> list = new List<Commande>();
            if (listAffectation != null && listAffectation.Count()>0)
            {
                for (int i = 0; i < listAffectation.Count; i++)
                {
                    var resp = httpClient.GetAsync(baseAddress + "/findOrderById/" + listAffectation[i]);

                    c = resp.Result.Content.ReadAsAsync<Commande>().Result;


                    list.Add(c);
                }
            }
            else
            {
                list = null;
            }
    
           

            affect.orders = list;
            affect.idDeliveryMan = idDeliveryMan;
            string json = JsonConvert.SerializeObject(affect);



            var httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(baseAddress + "dm/affectOrdersToDeliveryMan/");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";


            using (var streamWriter = new System.IO.StreamWriter(httpWebRequest.GetRequestStream()))
            {

                streamWriter.Write(json);
            }

            var httpResponse = (System.Net.HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new System.IO.StreamReader(httpResponse.GetResponseStream()))
            {

            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult getOrdersByDeliveryMan(int idDeliveryMan)
        {
            List<int> ordersAffected = new List<int>();
            if (idDeliveryMan !=0)
            {

                var respCommande = httpClient.GetAsync(baseAddress + "dm/getListOrdersByDeliveryManId___Criteria/"+idDeliveryMan);
                var resultCommande = (List<Commande>)respCommande.Result.Content.ReadAsAsync<IEnumerable<Commande>>().Result;
              if(resultCommande != null)
                {
                    foreach(Commande elm in resultCommande)
                    {
                        ordersAffected.Add(elm.id);
                    }
                }
            }
           
              
            
           

              return Json(ordersAffected, JsonRequestBehavior.AllowGet);

        }





            //   public ActionResult AffecterCommande()
            //  {

        //    var resp = httpClient.GetAsync(baseAddress + "ListOrder/");
        //    ViewBag.result = resp.Result.Content.ReadAsAsync<IEnumerable<Commande>>().Result;
        //   ViewBag.Title = "Liste des Commandes";
        //   return View(ViewBag.result);

        // }

        // GET: DM/Details/5

        public ActionResult Details()
        {
            return View();
        }

        // GET: DM/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DM/Create
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

        // GET: DM/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DM/Edit/5
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


        public ActionResult Delete(int id)
        {
            return View();
        }

        //delivery Man
        public ActionResult ListAffect()
        {
            var idLivreur = Session["Id"];
            var nomLivreur = Session["FirstName"];
            //get Livreur
            var respl = httpClient.GetAsync(baseAddress + "dm/getDeliveryManByName___Criteria/" + nomLivreur);
            var Livreur = respl.Result.Content.ReadAsAsync<DeliveryMan>().Result;

            if(Livreur != null)
            {
                var resp = httpClient.GetAsync(baseAddress + "dm/getListOrdersByDeliveryManId___Criteria/" + Livreur.id);
                ViewBag.result = resp.Result.Content.ReadAsAsync<IEnumerable<Commande>>().Result;
                ViewBag.Livreur = Livreur;
            }
            
         

            // ViewBag.idLivreur = id;

            if (ViewBag.result != null)
            {
                return View(ViewBag.result);
            }

            return View();

        }

           [HttpPost]
        public ActionResult ValiderLivraison(int idLivreur , int idCommande)
        {
            var resp = httpClient.GetAsync(baseAddress + "dm/setOrderDelivered/"+ idLivreur+"/"+ idCommande);
           
            if (resp != null)
            {
                ViewBag.result = resp.Result.Content.ReadAsAsync<Commande>().Result;
            }


              return Json("true", JsonRequestBehavior.AllowGet);
            // return CommandeByDeliveryMan(idLivreur);

        }

        // POST: DM/Delete/5
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
