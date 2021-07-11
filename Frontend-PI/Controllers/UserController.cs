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
    public class UserController : Controller
    {
        HttpClient httpClient;
        string baseAddress;
        public UserController()
        {
            baseAddress = "http://localhost:8081/SpringMVC/servlet/";
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseAddress);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
                                                                                                                 //var _AccessToken = Session[" AccessToken "];
                                                                                                                 // httpClient.DefaultRequestHeaders.Add(" Authorization ", String.Format(" Bearer {0} ", _AccessToken));
        }
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            var APIResponse = httpClient.GetAsync(baseAddress + "findUser/10");

            //HttpResponseMessage responseMessage = httpClient.GetAsync("findUser/6").Result;

            if (APIResponse.Result.IsSuccessStatusCode)
            {
                ViewBag.result = APIResponse.Result.Content.ReadAsAsync<User>().Result;
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

        // GET: User/Create
        public ActionResult Create()
        {
            return View();

        }

        // POST: User/Create
        [HttpPost]
        public ActionResult Create(User user)
        {
            try
            {
                var APIResponse = httpClient.PostAsJsonAsync<User>(baseAddress + "addUser/",
                user).ContinueWith(postTask => postTask.Result.EnsureSuccessStatusCode());
                return RedirectToAction("ConfirmUser");
            }
            catch
            {
                return View();
            }
        }

        // POST: User/ConfirmUser
        public ActionResult ConfirmUser(User user)
        {
            if (user.username != null && user.confirmCode != null)
            {
                try
                {
                    var APIResponse = httpClient.PostAsJsonAsync<User>(baseAddress + "confirmUser/",
                    user);
                    //.ContinueWith(postTask => postTask.Result.EnsureSuccessStatusCode());
                    if (APIResponse.Result.IsSuccessStatusCode)
                    {
                        User newUser = APIResponse.Result.Content.ReadAsAsync<User>().Result;
                        if (newUser != null)
                        {
                            return RedirectToAction("LoginUser");
                        }
                        else
                        {
                            return View("Error");
                        }
                    }
                }
                catch
                {
                    return View();
                }
            }
            return View();
        }

        // POST: User/LoginUser
        public ActionResult LoginUser(User user)
        {
            if (user.username != null && user.password != null)
            {
                try
                {
                    var APIResponse = httpClient.PostAsJsonAsync<User>(baseAddress + "loginUser",
                    user);
                    //.ContinueWith(postTask => postTask.Result.EnsureSuccessStatusCode());
                    if (APIResponse.Result.IsSuccessStatusCode)
                    {
                        User newUser = APIResponse.Result.Content.ReadAsAsync<User>().Result;
                        if (newUser != null)
                        {
                            Session["Id"] = newUser.id;
                            Session["FirstName"] = newUser.firstName;
                            Session["LastName"] = newUser.lastName;
                            Session["Username"] = newUser.username;
                            Session["UserRole"] = newUser.userRole;
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            return View("Lockout");
                        }
                    }
                }
                catch
                {
                    return View();
                }
            }
            return View();
        }


        // POST: User/LogoutUser
        public ActionResult LogoutUser()
        {
            Session["Id"] = null;
            Session["FirstName"] = null;
            Session["LastName"] = null;
            Session["Username"] = null;
            Session["UserRole"] = null;
            return RedirectToAction("Index", "Home");
        }



        // POST: User/ResetPassword
        public ActionResult ResetPassword(User user)
        {
            if (user.username != null)
            {
                try
                {
                    var APIResponse = httpClient.GetAsync(baseAddress + "resetPassword?username=" + user.username
                    );
                    //.ContinueWith(postTask => postTask.Result.EnsureSuccessStatusCode());
                    if (APIResponse.Result.IsSuccessStatusCode)
                    {
                        User newUser = APIResponse.Result.Content.ReadAsAsync<User>().Result;
                        if (newUser != null)
                        {
                            return RedirectToAction("UpdatePassword");
                        }
                        else
                        {
                            return View("Lockout");
                        }
                    }
                }
                catch
                {
                    return View();
                }
            }
            return View();
        }


        // POST: User/UpdatePassword
        public ActionResult UpdatePassword(User user)
        {
            if (user.username != null && user.password != null && user.confirmCode != null)
            {
                try
                {
                    var APIResponse = httpClient.PostAsJsonAsync<User>(baseAddress + "updatePassword/",
                             user);

                    //.ContinueWith(postTask => postTask.Result.EnsureSuccessStatusCode());
                    if (APIResponse.Result.IsSuccessStatusCode)
                    {
                        User newUser = APIResponse.Result.Content.ReadAsAsync<User>().Result;
                        if (newUser != null)
                        {
                            return RedirectToAction("LoginUser");
                        }
                        else
                        {
                            return View("Lockout");
                        }
                    }
                }
                catch
                {
                    return View();
                }
            }
            return View();

        }

        // GET: User/Edit/5
        /* public ActionResult Edit(int id)
         {

             return View();
         }*/

        // POST: User/UpdateUser
        public ActionResult UpdateUser(User user)
        {


            if (user!=null && user.username != null)
            {
                try
                {
                    var APIResponse = httpClient.PostAsJsonAsync<User>(baseAddress + "updateUser/",
                    user);
                    if (APIResponse.Result.IsSuccessStatusCode)
                    {
                        User newUser = APIResponse.Result.Content.ReadAsAsync<User>().Result;
                        if (newUser != null)
                        {
                            return RedirectToAction("LoginUser");
                        }
                        else
                        {
                            return View("Error");
                        }
                    }
                }
                catch
                {
                    return View();
                }
            }
            else
            {
                user = getUser(16);
                if (user != null)
                {
                    user.password = null;
                    return View(user);
                }

            }
            return View();
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var APIResponse = httpClient.DeleteAsync(baseAddress + "deleteUser/" + id
                ).ContinueWith(postTask => postTask.Result.EnsureSuccessStatusCode());
                return RedirectToAction("LoginUser");
            }
            catch
            {
                return View();
            }
        }

        // POST: User/Delete/5
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


        public User getUser(int idUser)
        {

            try
            {
                var APIResponse = httpClient.GetAsync(baseAddress + "findUser/" + idUser);
                if (APIResponse.Result.IsSuccessStatusCode)
                {
                    User newUser = APIResponse.Result.Content.ReadAsAsync<User>().Result;
                    if (newUser != null)
                    {
                        return newUser;
                    }
                }
            }
            catch
            {
                return null;
            }

            return null; 


        }
    }




}
