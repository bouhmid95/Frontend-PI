using Frontend_PI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using CaptchaMvc.HtmlHelpers;


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
        // GET: Home
        public ActionResult Index()
        {
            var APIResponse = httpClient.GetAsync(baseAddress + "findAllUser");
            if (APIResponse.Result.IsSuccessStatusCode)
            {
                ViewBag.result = APIResponse.Result.Content.ReadAsAsync<IEnumerable<User>>().Result;
                return View(ViewBag.result);
            }
            return View();
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                User user = getUser(id);
                if (user != null)
                    return View(user);
            }
            catch
            {
                return View("Error");
            }
            return View();
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

                if (this.IsCaptchaValid("Captcha is not valid"))
                {






                    var APIResponse = httpClient.PostAsJsonAsync<User>(baseAddress + "addUser/",
                    user).ContinueWith(postTask => postTask.Result.EnsureSuccessStatusCode());

                    return RedirectToAction("ConfirmUser", user);
                }
                ViewBag.ErrMessage = "Error: captcha is not valid.";
                return View();
            }
            catch
            {
                return View("Error");
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
                    return View("Error");
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
                            ViewBag.ErrLogin = "Votre pseudo ou le mot de passe est incorrect, veuillez réessayer.";
                        }
                    }
                    else
                    {
                        ViewBag.ErrLogin = "Votre pseudo ou le mot de passe est incorrect, veuillez réessayer.";
                    }
                }
                catch
                {
                    ViewBag.ErrLogin = "Votre pseudo ou le mot de passe est incorrect, veuillez réessayer.";
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
                            return View("LoginFailed");
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
                            return View("LoginFailed");
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
         public ActionResult Edit(int id,User user)
         {


         

            if (user != null && user.username != null)
            {
                try
                {
                    user.id = id;

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
                user = getUser(id);
                if (user != null)
                {
                    user.password = null;
                    return View(user);
                }

            }
            return View();
        }

        // POST: User/UpdateUser
        public ActionResult UpdateUser(User user)
        {
            if (Session["Id"] == null)
                return View();

            if (user!=null && user.username != null)
            {
                try
                {
                    user.id = Int16.Parse(Session["Id"].ToString());

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
                user = getUser(Int16.Parse(Session["Id"].ToString()));
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



        // GET: User/userByFirstLastname
        public ActionResult userByFirstLastname(User user)
        {
            if (user.firstName != null || user.lastName != null)
            {
                try
                 {
                var APIResponse = httpClient.GetAsync(baseAddress + "userByFirstLastname?firstName="+ user.firstName + "&lastName="+ user.lastName);
                    if (APIResponse.Result.IsSuccessStatusCode)
                    {
                    var users = APIResponse.Result.Content.ReadAsAsync<IEnumerable<User>>().Result;
                    User newUser = APIResponse.Result.Content.ReadAsAsync<User>().Result;
                        if (users != null)
                        {
                            return View("Index", users);
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


        // GET: User/statLockUnlockUser
        public ActionResult statLockUnlockUser()
        {
             try
              {
                  var APIResponse = httpClient.GetAsync(baseAddress + "statLockUnlockUser");
                  if (APIResponse.Result.IsSuccessStatusCode)
                  {
                    List<LockUnlockUser> dataPoints = new List<LockUnlockUser>();

                    var list = APIResponse.Result.Content.ReadAsAsync<IEnumerable<var>>().Result;
                      if (lockUnlockUsers != null)
                      {
                        foreach(var ockUnlockUser in lockUnlockUsers)
                        {

                        }

                        dataPoints.Add(new LockUnlockUser("NXP", 14));
                        dataPoints.Add(new LockUnlockUser("Infineon", 10));
                        ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
                        return View(lockUnlockUsers);
                      }
                  }
              }
              catch
              {
                  return View();
              }

          return View();



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
