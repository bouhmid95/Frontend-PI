using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using Frontend_PI.Models;

namespace Frontend_PI.Controllers
{
    public class CommentController : Controller
    {


        HttpClient httpClient;
        string baseAddress;
        public CommentController()
        {
            baseAddress = "http://localhost:8081/SpringMVC/servlet/";
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseAddress);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
                                                                                                                 //var _AccessToken = Session[" AccessToken "];
                                                                                                                 // httpClient.DefaultRequestHeaders.Add(" Authorization ", String.Format(" Bearer {0} ", _AccessToken));
        }
        // GET: Comment
        public ActionResult Index()
        {
            var APIResponse = httpClient.GetAsync(baseAddress + "findAllComments");

            //HttpResponseMessage responseMessage = httpClient.GetAsync("findUser/6").Result;

            if (APIResponse.Result.IsSuccessStatusCode)
            {
                ViewBag.result = APIResponse.Result.Content.ReadAsAsync<IEnumerable<Comment>>().Result;
                return View(ViewBag.result);
            }
            return View();
        }

        // GET: Comment/Details/5
        public ActionResult Details(int id)
        {
            var APIResponse = httpClient.GetAsync(baseAddress + "findComment/" + id);

            //HttpResponseMessage responseMessage = httpClient.GetAsync("findUser/6").Result;

            if (APIResponse.Result.IsSuccessStatusCode)
            {
                ViewBag.result = APIResponse.Result.Content.ReadAsAsync<Comment>().Result;
                return View(ViewBag.result);
            }
            return View();


            /*  HttpClient httpClient = new HttpClient();
              httpClient.BaseAddress = new Uri("http://localhost:8081");
              httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
              HttpResponseMessage responseMessage = httpClient.GetAsync("SpringMVC/servlet/findUser/" + id).Result;
              if (responseMessage.IsSuccessStatusCode)
              {
                  ViewBag.result = responseMessage.Content.ReadAsAsync<User>().Result;
              }
              return View();*/

        }

        // GET: Comment/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Comment/Create
        [HttpPost]
        public ActionResult Create(string comment , int idpublication)
        {
            try
            {
                var idUser = Convert.ToInt16(Session["id"].ToString());


                var resp = httpClient.GetAsync(baseAddress + "addComment/" + comment + "/" + idpublication + "/" + idUser);
                var res = resp.Result.Content.ReadAsAsync<Comment>().Result;
            }
            catch
            {
                return Json("true", JsonRequestBehavior.AllowGet);
            }
            return null;
        }


        // GET: Publication
        public ActionResult IndexAdminComment(int id)
        {
            var APIResponse = httpClient.GetAsync(baseAddress + "findCommentsByUser/" + id);

            //HttpResponseMessage responseMessage = httpClient.GetAsync("findUser/6").Result;

            if (APIResponse.Result.IsSuccessStatusCode)
            {
                ViewBag.result = APIResponse.Result.Content.ReadAsAsync<IEnumerable<Comment>>().Result;
                return View(ViewBag.result);
            }
            return View();
        }


        // GET: Comment/Edit/5
        public ActionResult Edit(int id)
        {
            httpClient.BaseAddress = new Uri("http://localhost:8081");
            HttpResponseMessage responseMessage = httpClient.GetAsync("SpringMVC/servlet/findComment/" + id).Result;

            if (responseMessage.IsSuccessStatusCode)
            {
                ViewBag.result = responseMessage.Content.ReadAsAsync<Comment>().Result;
                Console.WriteLine(ViewBag.result);
                return View(ViewBag.result);
            }
            return View();
        }

        // POST: Comment/Edit/5
        [HttpPost]
        public ActionResult Edit(Comment comment)
        {
            try
            {
                var APIResponse = httpClient.PostAsJsonAsync<Comment>(baseAddress + "updateComment/",
                comment).ContinueWith(postTask => postTask.Result.EnsureSuccessStatusCode());



                return RedirectToAction("Index");


            }
            catch
            {
                return View();
            }
        }

        // GET: Comment/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Comment/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var APIResponse = httpClient.DeleteAsync(baseAddress + "deleteComment/" + id
                ).ContinueWith(postTask => postTask.Result.EnsureSuccessStatusCode());
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
