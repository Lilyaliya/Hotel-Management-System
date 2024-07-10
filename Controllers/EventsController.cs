using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Web.Mvc;
using Hotel.Models;
using System.Security;
using Hotel.Authentication;

namespace Hotel.Controllers
{
    public class EventsController : Controller
    {
        hosteldat2Entities entities;
        IEnumerable< Hotel.Models.TempRooms> choosenRooms;
        int userID;
        public EventsController()
        {
            var user = (CustomMemberShipUser)Membership.GetUser(true);
            if (user != null)
                userID = user.ID;
            entities = new hosteldat2Entities();
            choosenRooms = (from el in entities.TempRooms
                            where el.ActorID == userID
                            select el);
            //choosenRooms = ;
        }
        // GET: Events
        public ActionResult Index()
        {
            var rooms = (from el in entities.Room
                         select new SelectListItem() { Text = el.RoomNum.ToString(), Value = el.RoomNum.ToString() }).ToList();
            ViewModel.EventViewModel viewModel = new ViewModel.EventViewModel();
            
            viewModel.ListRooms = rooms.ToList();
            viewModel.ListClients = (from cl in entities.Actor
                                     where cl.Role == "USER"
                                     select new SelectListItem() { Text = cl.FullName.ToString(), Value = cl.ID.ToString() }).ToList();
            viewModel.ListCategoryClass = (from el in entities.RoomClass
                                           select new SelectListItem() { Text = el.Class.ToString(), Value = el.Class.ToString()}).ToList();
            viewModel.C_Date = DateTime.Now;
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(Hotel.ViewModel.EventViewModel newEvent)
        {
            List<Hotel.ViewModel.ItemRooms> rooms = (from el in entities.TempRooms
                                                            where el.ActorID == userID 
                                                            select new ViewModel.ItemRooms() { Room = el.Room, CategoryClass = el.CategoryClass}).ToList();
            newEvent.Rooms = rooms;
            int id = Convert.ToInt32(newEvent.Client);
            var client = entities.Actor.SingleOrDefault(model => model.ID == id);
            if (client != null)
            {
                newEvent.FullName = client.FullName;
                newEvent.ContactPhone = client.ContactPhone;
                newEvent.ClientID = client.ID;
            }
            List<EventRooms> event1 = new List<EventRooms>() { };
            foreach (var e in newEvent.Rooms)
            {
                event1.Add(new EventRooms() { Room = e.Room});
            }
            var act = entities.Actor.SingleOrDefault(model=> model.ID == userID);
            Event newEventino = new Event()
            {
                Agent = userID,
                Client = newEvent.ClientID,
                Name = newEvent.Name,
                Description = newEvent.Description,
                C_Date = newEvent.C_Date,
                Visitors = newEvent.Visitors,
                EventRooms = event1,
                Actor = act,
                Actor1 = client,
            };

            Receipt check = new Receipt()
            {
                Agent = userID,
                Client = newEvent.ClientID,
                Bill = newEvent.Bill,
            };

            newEventino.ReceiptID = check.ID;
            entities.Event.Add(newEventino);
            entities.Receipt.Add(check);
            entities.SaveChanges();
            return Json(new { message = "Запись успешно добавлена!", success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateRooms(string category)
        {
            var rooms = (from el in entities.Room
                         where el.CategoryClass == category
                         select el.RoomNum);
            IEnumerable<int> union = null;
            List<SelectListItem> total  = (from el in entities.Room
                                           where el.CategoryClass == category
                                           select new SelectListItem() { Text = el.RoomNum.ToString(), Value = el.RoomNum.ToString()}).ToList();
            if (choosenRooms.Count() != 0)
            {
                union = (from el in entities.TempRooms
                             where el.CategoryClass == category && el.ActorID == userID
                             select el.Room);
                var except = rooms.Except(union);
                total = (from e in except
                             select new SelectListItem() { Text = e.ToString(), Value = e.ToString() }).ToList();
               
            }
            return Json(data: total.Select(s => new {
                Text = s.Text,
                Value = s.Value,
            }).ToList(), JsonRequestBehavior.AllowGet);
        }
        //[HttpPost]
        public ActionResult AddToList(string room, string categoryClass)
        {
            TempRooms item = new TempRooms()
            {
                ActorID = userID,
                Room = Convert.ToInt32(room),
                CategoryClass = categoryClass
            };
            int r = Convert.ToInt32(room);
            entities.TempRooms.Add(item);
            entities.SaveChanges();
            //choosenRooms.Add(item);
            //GetChoosenRooms();


            var rooms = (from el in entities.Room
                         where el.CategoryClass == categoryClass
                         select el.RoomNum);
            IEnumerable<int> union = null;
            List<SelectListItem> total = (from el in entities.Room
                                          where el.CategoryClass == categoryClass
                                          select new SelectListItem() { Text = el.RoomNum.ToString(), Value = el.RoomNum.ToString() }).ToList();
            if (choosenRooms.Count() != 0)
            {
                union = (from el in entities.TempRooms
                         where el.CategoryClass == categoryClass
                         select el.Room);
                var except = rooms.Except(union);
                total = (from e in except
                         select new SelectListItem() { Text = e.ToString(), Value = e.ToString() }).ToList();

            }

            return Json(data:  total.Select(s=> new { 
                Text = s.Text,
                Value = s.Value,
            }), JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        public ActionResult RemoveFromList(int room, string categoryClass)
        {
            Hotel.Models.TempRooms item = new TempRooms()
            {
                ActorID = userID,
                Room = Convert.ToInt32(room),
                CategoryClass = categoryClass
            };
            TempRooms markDel = entities.TempRooms.Single(model=> model.ActorID == item.ActorID && model.Room == item.Room && model.CategoryClass == item.CategoryClass);
            entities.TempRooms.Remove(markDel);
            entities.SaveChanges();
            //choosenRooms.Remove(item);

            var rooms = (from el in entities.Room
                         where el.CategoryClass == categoryClass
                         select el.RoomNum);
            IEnumerable<int> union = null;
            List<SelectListItem> total = (from el in entities.Room
                                          where el.CategoryClass == categoryClass
                                          select new SelectListItem() { Text = el.RoomNum.ToString(), Value = el.RoomNum.ToString() }).ToList();
            if (choosenRooms.Count() != 0)
            {
                union = (from el in entities.TempRooms
                         where el.CategoryClass == categoryClass && el.ActorID == userID
                         select el.Room);
                var except = rooms.Except(union);
                total = (from e in except
                         select new SelectListItem() { Text = e.ToString(), Value = e.ToString() }).ToList();
            }


            //GetChoosenRooms();
            return Json(data: total.Select(s => new {
                Text = s.Text,
                Value = s.Value,
            }), JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        public PartialViewResult GetChoosenRooms()
        {
            choosenRooms = (from el in entities.TempRooms
                            where el.ActorID == userID
                            select el);
            return PartialView("_EventRoomsPartial", choosenRooms);
        }


        public PartialViewResult GetEvents()
        {
            var events = (from el in entities.Event
                          join receipt in entities.Receipt on el.ReceiptID equals receipt.ID
                          select new ViewModel.EventViewModel() { 
                              FullName = el.Actor1.FullName,
                              ContactPhone = el.Actor1.ContactPhone,
                              Name = el.Name,
                              C_Date = el.C_Date,
                              Visitors = el.Visitors,
                              Rooms =  el.EventRooms.Select(s=> new ViewModel.ItemRooms { 
                                  Room = s.Room,
                                  CategoryClass = entities.Room.FirstOrDefault(model=> model.RoomNum == s.Room).RoomClass.Class,
                              }).ToList(),
                              Bill = receipt.Bill
                          }).ToList();
            return PartialView("_EventsMAINPartial", events);
        }
        //[HttpGet]
        public ActionResult GetBill()
        {
            var rooms = (from el in entities.TempRooms
                         where el.ActorID == userID
                         select el.Room);
            decimal totalCost = 0;

            foreach (var e in rooms)
            {
                
                var nextRoom = entities.Room.SingleOrDefault(model=> model.RoomNum == e);
                if (nextRoom != null)
                    totalCost += nextRoom.cost;
            }

            return Json(data: new { success = true, total = totalCost},  JsonRequestBehavior.AllowGet);
        }

    }
}