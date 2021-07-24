using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Frontend_PI.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.IO;

namespace Frontend_PI.Controllers
{
    public class PublicationController : Controller


    {

        HttpClient httpClient;
        string baseAddress;
        public PublicationController()
        {
            baseAddress = "http://localhost:8081/SpringMVC/servlet/";
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseAddress);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
                                                                                                                 //var _AccessToken = Session[" AccessToken "];
                                                                                                                 // httpClient.DefaultRequestHeaders.Add(" Authorization ", String.Format(" Bearer {0} ", _AccessToken));
        }
        // GET: Publication
        public ActionResult Index()
        {
            var APIResponse = httpClient.GetAsync(baseAddress + "findValidatedPublications");

            //HttpResponseMessage responseMessage = httpClient.GetAsync("findUser/6").Result;

            if (APIResponse.Result.IsSuccessStatusCode)
            {
                ViewBag.result = APIResponse.Result.Content.ReadAsAsync<IEnumerable<Publication>>().Result;
                return View(ViewBag.result);
            }
            return View();
        }



        // GET: Publication
        public ActionResult IndexAdmin()
        {
            var APIResponse = httpClient.GetAsync(baseAddress + "findNotValidatedPublications");

            //HttpResponseMessage responseMessage = httpClient.GetAsync("findUser/6").Result;

            if (APIResponse.Result.IsSuccessStatusCode)
            {
                ViewBag.result = APIResponse.Result.Content.ReadAsAsync<IEnumerable<Publication>>().Result;
                return View(ViewBag.result);
            }
            return View();
        }

        // GET: Publication/Details/5
        public ActionResult Details(int id)
        {
            var idUser = Convert.ToInt16(Session["id"].ToString());
            var APIResponse = httpClient.GetAsync(baseAddress + "findPublication/" + id);
            var APIResponse2 = httpClient.GetAsync(baseAddress + "findCommentsByPublicationId/" + id);

            //HttpResponseMessage responseMessage = httpClient.GetAsync("findUser/6").Result;

            if (APIResponse.Result.IsSuccessStatusCode)
            {
                Publication pub = APIResponse.Result.Content.ReadAsAsync<Publication>().Result;
                ViewBag.result = pub;
                ViewBag.idp = pub.id;
               // return View(ViewBag.result);
            }
            if (APIResponse2.Result.IsSuccessStatusCode)
            {
                ViewBag.comments = APIResponse2.Result.Content.ReadAsAsync<IEnumerable<Comment>>().Result;
            }
            return View(ViewBag.result);


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


        // GET: Publication/Details/5
        public ActionResult DetailsAdmin(int id)
        {
            var APIResponse = httpClient.GetAsync(baseAddress + "findPublication/" + id);

            //HttpResponseMessage responseMessage = httpClient.GetAsync("findUser/6").Result;

            if (APIResponse.Result.IsSuccessStatusCode)
            {
                ViewBag.result = APIResponse.Result.Content.ReadAsAsync<Publication>().Result;
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





        // GET: Publication/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Publication/Create
        [HttpPost]
        public ActionResult Create(Publication publication,HttpPostedFileBase file)
        {
            try
            {
                //publication.image = file.filename;
                //if (file.contentlength > 0)
                //{
                //    var path = path.combine(server.mappath("~/content/uploads/"), file.filename);
                //    file.saveas(path);
                //}
                publication.idUser = Convert.ToInt16(Session["id"].ToString());
                var APIResponse = httpClient.PostAsJsonAsync<Publication>(baseAddress + "addPublication/",
                publication).ContinueWith(postTask => postTask.Result.EnsureSuccessStatusCode());
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Publication/Edit/5
        public ActionResult Edit(int id)
        {
            httpClient.BaseAddress = new Uri("http://localhost:8081");
            HttpResponseMessage responseMessage = httpClient.GetAsync("SpringMVC/servlet/findPublication/" + id).Result;

            if (responseMessage.IsSuccessStatusCode)
            {
                ViewBag.result = responseMessage.Content.ReadAsAsync<Publication>().Result;
                Console.WriteLine(ViewBag.result);
                return View(ViewBag.result);
            }
            return View();
        }


        // POST: Publication/Edit/5
        [HttpPost]
        public ActionResult Edit(Publication publication)
        {
            try
            {
                var APIResponse = httpClient.PostAsJsonAsync<Publication>(baseAddress + "updatePublication/",
                publication).ContinueWith(postTask => postTask.Result.EnsureSuccessStatusCode());



                return RedirectToAction("Index");


            }
            catch
            {
                return View();
            }
        }



        public ActionResult LikePublication(int id)
        {
            try
            {
                var APIResponse = httpClient.GetAsync(baseAddress + "findPublication/" + id);
                var APIResponse3 = httpClient.GetAsync(baseAddress + "findCommentsByPublicationId/" + id);

                if (APIResponse.Result.IsSuccessStatusCode)
                {
                    Publication publication = APIResponse.Result.Content.ReadAsAsync<Publication>().Result;
                
                var APIResponse2 = httpClient.PostAsJsonAsync<Publication>(baseAddress + "likePublication",
                publication).ContinueWith(postTask => postTask.Result.EnsureSuccessStatusCode());
                    ViewBag.comments = APIResponse3.Result.Content.ReadAsAsync<IEnumerable<Comment>>().Result;

                    return View("Details" , publication);
                }
                return View("Index");

            }
            catch
            {
                return View();
            }
        }



        public ActionResult DisLikePublication(int id)
        {
            try
            {
                var APIResponse = httpClient.GetAsync(baseAddress + "findPublication/" + id);
                var APIResponse3 = httpClient.GetAsync(baseAddress + "findCommentsByPublicationId/" + id);
                if (APIResponse.Result.IsSuccessStatusCode)
                {
                    Publication publication = APIResponse.Result.Content.ReadAsAsync<Publication>().Result;

                    var APIResponse2 = httpClient.PostAsJsonAsync<Publication>(baseAddress + "DislikePublication",
                    publication).ContinueWith(postTask => postTask.Result.EnsureSuccessStatusCode());
                    ViewBag.comments = APIResponse3.Result.Content.ReadAsAsync<IEnumerable<Comment>>().Result;
                    return View("Details", publication);
                }
                return View();

            }
            catch
            {
                return View();
            }
        }


        // GET: Publication/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Publication/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var APIResponse = httpClient.DeleteAsync(baseAddress + "deletePublication/" + id
                ).ContinueWith(postTask => postTask.Result.EnsureSuccessStatusCode());
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        // GET: User/statLockUnlockUser
        public ActionResult statpublication()
        {
            try
            {
                var APIResponse = httpClient.GetAsync(baseAddress + "statpublications");
                if (APIResponse.Result.IsSuccessStatusCode)
                {
                    List<LockUnlockUser> dataPoints = new List<LockUnlockUser>();

                    List<LockUnlockUser> list = (List<LockUnlockUser>)APIResponse.Result.Content.ReadAsAsync<IEnumerable<LockUnlockUser>>().Result;
                    if (list != null)
                    {

                        double sum = 0;
                        foreach (var ockUnlockUser in list)
                        {
                            sum = sum + ockUnlockUser.y;
                        }

                        foreach (var ockUnlockUser in list)
                        {
                            dataPoints.Add(new LockUnlockUser(ockUnlockUser.label, (ockUnlockUser.y / sum) * 100));
                        }


                        ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
                        return View();
                    }
                }
            }
            catch
            {
                return View();
            }

            return View();



        }

        [HttpPost]
        public ActionResult CreateComment(Comment comment)
        {
            try
            {
                comment.idUser = Convert.ToInt16(Session["id"].ToString());
                var APIResponse = httpClient.PostAsJsonAsync<Comment>(baseAddress + "addComment/",
                comment).ContinueWith(postTask => postTask.Result.EnsureSuccessStatusCode());
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

    }
}
