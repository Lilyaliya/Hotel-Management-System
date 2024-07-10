using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hotel.Models;
using Hotel.Authentication;
using System.Web.Security;

namespace Hotel.Controllers
{
    public class CleanerController : Controller
    {
        hosteldat2Entities entities;
        public CleanerController()
        {
            entities = new hosteldat2Entities();
        }

        // GET: Cleaner
        public ActionResult Index()
        {
            Hotel.ViewModel.CleanJournalView journal = new Hotel.ViewModel.CleanJournalView();
            journal.ListRooms = (from elem in entities.Room
                                 where elem.dirty == true
                                       select new SelectListItem() { Text = elem.RoomNum.ToString(), Value = elem.RoomNum.ToString() }).ToList();
            journal.Today = DateTime.Now;
            var change = entities.Change.SingleOrDefault(model => System.Data.Entity.DbFunctions.DiffDays(model.ChangeDate, DateTime.Now) < 1);
            if (change != null)
            {
                journal.ChangeID = ((Change)change).ID;
            }
            return View(journal);
        }

        public ActionResult UpdRooms()
        {
            Hotel.ViewModel.CleanJournalView journal = new Hotel.ViewModel.CleanJournalView();
            journal.ListRooms = (from elem in entities.Room
                                 where elem.dirty == true
                                 select new SelectListItem() { Text = elem.RoomNum.ToString(), Value = elem.RoomNum.ToString() }).ToList();
            
            return Json(
                journal.ListRooms.Select(s => new
                {
                    Text = s.Text,
                    Value = s.Value,
                }).ToList()
               , JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Index(Hotel.ViewModel.CleanJournalView entry)
        {
            //return View();
            string successMessage = "Запись была добавлена в журнал учета уборки!";
            var user = (CustomMemberShipUser)Membership.GetUser(true);
            var result = entities.Actor.Single(model => model.ID == user.ID);
            Cleaning newEntry = new Cleaning()
            {
                Room = Convert.ToInt32(entry.RoomView),
                ChangeID = entry.ChangeID,
                Actor = result.ID,
                Actor1 = result,
                Today = DateTime.Now,
            };
            Room cleanedOne = entities.Room.SingleOrDefault(model => model.RoomNum == newEntry.Room);
            if (cleanedOne != null)
            {
                cleanedOne.dirty = false;
            }
            entities.Cleaning.Add(newEntry);
            entities.SaveChanges();
            return Json(data: new { message = successMessage, success = true }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult GetAllEntries()
        {


            IEnumerable<Hotel.ViewModel.CleanJournalView> listOfEntries = (from elem in entities.Cleaning
                                                                           join actor in entities.Actor on elem.Actor equals actor.ID
                                                                       select new ViewModel.CleanJournalView
                                                                       {
                                                                           Room = elem.Room,
                                                                           
                                                                           ChangeID = elem.ChangeID,
                                                                           Today =elem.Today,
                                                                           Actor = elem.Actor,
                                                                           FullName = actor.FullName,
                                                                       }).ToList();
            return PartialView("_CleanJournalPartial", listOfEntries);
        }
    }
}