using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hotel.Authentication;
using Hotel.Models;
using System.Web.Security;

namespace Hotel.Controllers
{
    public class HomeController : Controller
    {

        hosteldat2Entities entities;

        public HomeController()
        {
            entities = new hosteldat2Entities();
        }

        public ActionResult Index()
        {
            //GetChange();
            return View();
        }
        public ActionResult Rooms()
        {

            return View();
        }

        [HttpGet]
        public ActionResult GetChange()
        {
            Boolean regChange = false;
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var identity = ((CustomPrincipal)System.Web.HttpContext.Current.User);
                if (identity.Roles.Contains("AGENT") || identity.Roles.Contains("CLEANER"))
                {

                    var Change = new Change();
                    var user = (CustomMemberShipUser)Membership.GetUser(true);
                    var result = entities.Actor.Single(model => model.ID == user.ID);
                    var ifExist = (from elem in entities.Change

                                   where System.Data.Entity.DbFunctions.DiffDays(elem.ChangeDate, DateTime.Now) < 1
                                   select elem).ToList();
                    if (ifExist.Count == 0)
                        regChange = true;
                    else
                    {
                        Change = entities.Change.SingleOrDefault(model => System.Data.Entity.DbFunctions.DiffDays(model.ChangeDate, DateTime.Now) < 1);
                        var hasActor = Change.Actor.FirstOrDefault(model => model.ID == user.ID);
                        if (hasActor == null)
                            regChange = true;
                    }
                }
            }
            return Json(data: new { message = "Для доступа к сервису подтвердите запись в смене!", success = regChange},JsonRequestBehavior.AllowGet);
        }


        public PartialViewResult GetAllRooms()
        {
            IEnumerable<Hotel.ViewModel.RoomDetailsView> listOfRoom = (from rooms in entities.Room
                                                                       select new ViewModel.RoomDetailsView
                                                                       {
                                                                           RoomNum = rooms.RoomNum,
                                                                           RoomImage = rooms.Image,
                                                                           C_Floor = rooms.C_Floor,
                                                                           Places = rooms.Places,
                                                                           cost = rooms.cost,
                                                                           dirty = rooms.dirty ? "Да" : "Нет",
                                                                           CategoryClass = rooms.CategoryClass,
                                                                           CategoryEating = rooms.CategoryEating,
                                                                           CategoryWindow = rooms.CategoryWindow,
                                                                       }).ToList();
            return PartialView("_HomeIndex", listOfRoom);
            //return View(Index, listOfRoom);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Service()
        {
            ViewBag.Message = "There is the service page.";

            return View();
        }
    }
}