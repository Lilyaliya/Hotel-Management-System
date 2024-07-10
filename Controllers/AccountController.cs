using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;
using Hotel.Models;
using Hotel.Authentication;
using Hotel.ViewModel;

namespace Hotel.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        
        [HttpGet]
        public ActionResult Login(string returnUrl = "")
        {
            if (User.Identity.IsAuthenticated)
            {
                return LogOut();
            }
            ViewBag.returnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginView loginView, string returnUrl = "")
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(loginView.UserName, loginView.Password))
                {
                    var user = (CustomMemberShipUser)Membership.GetUser(loginView.UserName, false);
                    if (user != null)
                    {
                        if ((user.Role == "AGENT" || user.Role == "CLEANER") && user.VerifiedBy != 0 && user.VerifiedBy != null || user.Role == "USER" || user.Role == "MANAGER")
                        {
                            CustomSerializeModel userModel = new CustomSerializeModel()
                            {
                                ID = user.ID,
                                FullName = user.FullName,
                                Login = user.Login,
                                Role = new List<string>() { user.Role }
                            };

                            string userData = JsonConvert.SerializeObject(userModel);
                            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket
                                (
                                1, loginView.UserName, DateTime.Now, DateTime.Now.AddMinutes(15), false, userData
                                );

                            string enTicket = FormsAuthentication.Encrypt(authTicket);
                            HttpCookie faCookie = new HttpCookie("Cookie1", enTicket);
                            Response.Cookies.Add(faCookie);
                        }
                        else
                        {
                            ModelState.AddModelError("", "Отказано в доступе! Ваша заявка не подтверждена менеджером. ^_^ ");
                            return View(loginView);
                        }
                    }

                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            ModelState.AddModelError("", "Что-то пошло не так: Логин или Пароль некорректны ^_^ ");
            return View(loginView);
        }

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Registration(RegistrationView registrationView)
        {
            bool statusRegistration = false;
            string messageRegistration = string.Empty;

            if (ModelState.IsValid)
            {
                // Email Verification
                string userName = Membership.GetUserNameByEmail(registrationView.Email);
                
                if (!string.IsNullOrEmpty(userName))
                {
                    ModelState.AddModelError("", "Данный почтовый адрес уже занят...");
                    return View(registrationView);
                }

                //Save User Data 
                using (hosteldat2Entities dbContext = new hosteldat2Entities())
                {
                    if (dbContext.Actor.FirstOrDefault(x=>x.Login == registrationView.Username) != null)
                    {
                        ModelState.AddModelError("", "Пользователь с указанным логином уже существует...");
                        return View(registrationView);
                    }
                    var user = new Actor()
                    {
                        Login = registrationView.Username,
                        FullName = registrationView.FullName,
                        Email = registrationView.Email,
                        ContactPhone = registrationView.ContactPhone,
                        Role = "USER",
                        Passport = registrationView.Passport,
                        Password = registrationView.Password,
                        
                    };

                    dbContext.Actor.Add(user);
                    dbContext.SaveChanges();
                }

                
                messageRegistration = "Поздравляем с успешной регистрацией. ^_^";
                statusRegistration = true;
            }
            else
            {
                messageRegistration = "Упс! Что-то пошло не так...";
            }
            ViewBag.Message = messageRegistration;
            ViewBag.Status = statusRegistration;

            return View(registrationView);
        }

        [HttpGet]
        public ActionResult RegistrationEmp()
        {
            RegistrationViewEmp employee = new RegistrationViewEmp();
            IEnumerable<SelectListItem> list = new List<SelectListItem> { new SelectListItem() { Text = "Агент по бронированию", Value = "AGENT" }, new SelectListItem() { Text = "Сотудник клининга", Value = "CLEANER" } };
            employee.ListRoles = list.ToList();
            return View(employee);
        }

        [HttpPost]
        public ActionResult RegistrationEmp(RegistrationViewEmp registrationView)
        {

            bool statusRegistration = false;
            string messageRegistration = string.Empty;

            if (ModelState.IsValid)
            {
                // Email Verification
                string userName = Membership.GetUserNameByEmail(registrationView.Email);

                if (!string.IsNullOrEmpty(userName))
                {
                    ModelState.AddModelError("", "Данный почтовый адрес уже занят...");
                    return View(registrationView);
                }

                //Save User Data 
                using (hosteldat2Entities dbContext = new hosteldat2Entities())
                {
                    if (dbContext.Actor.FirstOrDefault(x => x.Login == registrationView.Username) != null)
                    {
                        ModelState.AddModelError("", "Пользователь с указанным логином уже существует...");
                        return View(registrationView);
                    }
                    var user = new Actor()
                    {
                        Login = registrationView.Username,
                        FullName = registrationView.FullName,
                        Email = registrationView.Email,
                        ContactPhone = registrationView.ContactPhone,
                        Role = registrationView.Role,
                        Passport = registrationView.Passport,
                        Password = registrationView.Password,

                    };

                    dbContext.Actor.Add(user);
                    dbContext.SaveChanges();
                }

                messageRegistration = "Заявка была отправлена менеджеру. Скоро с вами свяжутся ^_^";
                statusRegistration = true;
            }
            else
            {
                messageRegistration = "Упс! Что-то пошло не так...";
            }
            ViewBag.Message = messageRegistration;
            ViewBag.Status = statusRegistration;

            return View(registrationView);

        }

        public ActionResult LogOut()
        {
            HttpCookie cookie = new HttpCookie("Cookie1", "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);

            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account", null);
        }

        
    }
}