using Frontend_PI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Frontend_PI.Controllers
{
    public class CommandeController : Controller
    {

        public CommandeController() {
            ProductController.sumPriceProduct = 0;
        }

        // GET: Commande
        public ActionResult Index()
        {
            List<Commande> commandes = new List<Commande>();
            HttpClient httpClient = new HttpClient();
            String id = Session["id"].ToString();
            httpClient.BaseAddress = new Uri("http://localhost:8081");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = httpClient.GetAsync("SpringMVC/servlet/findOrderByUser/"+ id).Result;
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
        public ActionResult Details(int id, String reference)
        {
            Session["referenceOrder"] = reference;
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://localhost:8081");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = httpClient.GetAsync("SpringMVC/servlet/ListOrderDetailsByOrderId/" + id).Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                ViewBag.result = responseMessage.Content.ReadAsAsync<IEnumerable<CommandeDetails>>().Result;
                Session["listProductToDownload"] = ViewBag.result;
                return View(ViewBag.result);
            }
            return View();
        }

        public ActionResult StatistiqueCommande()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://localhost:8081");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = httpClient.GetAsync("SpringMVC/servlet/findOrderNumberForUser").Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                ViewBag.result = responseMessage.Content.ReadAsAsync<IEnumerable<CountOrderByUser>>().Result;
                List<CountOrderByUser> countOrderByUsers = ViewBag.result;
                ViewBag.countOrderLst = countOrderByUsers;
                return View("StatistiqueCommande", countOrderByUsers);
            }
            return View("StatistiqueCommande");
        }

        [HttpPost]
        public ActionResult downloadFacture()
        {
            List<CommandeDetails> commandeDetails = (List<CommandeDetails>)Session["listProductToDownload"];
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://localhost:8081");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("cache-control", "no-cache");
            var response = httpClient.PostAsJsonAsync("SpringMVC/servlet/downloadOrderFile", commandeDetails).Result;
            int statusCode = (int)response.StatusCode;
            if (statusCode == 200)
            {
                String fileTitle = @Session["referenceOrder"].ToString();
                String fileName = "Order-Report-"+ fileTitle +"-"+ DateTime.Now.ToString("dd_MMMM_yyyy") + ".pdf";
                System.Diagnostics.Process.Start("D:\\PdfReportRepo\\" + fileName);
            }
                return View();
        }

        public static String show(String x)
        {
            return "";
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
                Random rnd = new Random();
                int reference = rnd.Next(1, 9999);
                commande.reference = "REF-" + reference ;
                commande.status = "NEW" ;
                commande.orderDate = DateTime.Now;
                commande.idUser = Convert.ToInt16(Session["id"].ToString());
                //USER INFO
                HttpResponseMessage responseMessageUser = httpClient.GetAsync("SpringMVC/servlet/findUser/" + Convert.ToInt16(Session["id"].ToString())).Result;
                User userCmd = responseMessageUser.Content.ReadAsAsync<User>().Result;
                commande.user = userCmd;
                //
                var response = httpClient.PostAsJsonAsync("SpringMVC/servlet/addOrder", commande).Result;

                int statusCode = (int) response.StatusCode;

                //var response = httpClient.PostAsJsonAsync<Commande>("SpringMVC/servlet/addOrder", commande).ContinueWith((p) => p.Result.EnsureSuccessStatusCode());
                if (statusCode == 200)
                {
                    Commande addedCommand = response.Content.ReadAsAsync<Commande>().Result;
                    List<CommandeDetails> commandeDetailsList = (List<CommandeDetails>)Session["commandeDetailsList"];
                    foreach(var commandeDetails in commandeDetailsList){
                        commandeDetails.idOrder = addedCommand.id ;
                    }
                    var resp = httpClient.PostAsJsonAsync("SpringMVC/servlet/addOrderDetails", commandeDetailsList).Result;
                    return RedirectToAction("Index");
                }
                else
                    return View();
            }
            catch
            {
                return View();
            }
            /*	"reference" : "REF 0098",
	"status" : "En cours",
	"adresse" : "TUNIS",
	"typePaiement" : "carte",
	"codePostal" : "2077",
	"user" : {
		"id" : 2
	}*/
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
