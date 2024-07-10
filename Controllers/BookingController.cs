using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hotel.Models;
using Hotel.ViewModel;
using System.Web.Security;
using Hotel.Authentication;

namespace Hotel.Controllers
{
    public class BookingController : Controller
    {
        hosteldat2Entities entities;
        public BookingController()
        {
            entities = new hosteldat2Entities();
        }
        // GET: Booking
        public ActionResult Index()
        {
            BookingViewModel viewModel = new BookingViewModel();
            var union = (from el in entities.Room
                         select el.RoomNum);
            var history = (from el in entities.Booking
                           where el.Departure >= DateTime.Now
                           select el.BookingRooms.FirstOrDefault().Room);
            var except = union.Except(history);
            var totalRooms = (from e in except
                              select new SelectListItem() { Text = e.ToString(), Value =e.ToString()});
            viewModel.ListRooms = totalRooms.ToList();
            viewModel.ListClients = (from cl in entities.Actor
                                     where cl.Role == "USER"
                                     select new SelectListItem() { Text = cl.FullName.ToString(), Value = cl.ID.ToString() }).ToList();
            viewModel.CreatedAt = DateTime.Now;
            viewModel.Arrival = DateTime.Now;
            viewModel.Departure = DateTime.Now.AddDays(1);
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult Index(BookingViewModel booking)
        {

            int agentID = 0;
            int days = Convert.ToInt32((booking.Departure - booking.Arrival).TotalDays);
            Room room = entities.Room.Single(model => model.RoomNum == booking.Room);
            decimal cost = room.cost;
            decimal totalCost = cost * days;
            booking.Bill = totalCost;
            booking.ClientID = Convert.ToInt32(booking.Client);

            var Client = entities.Actor.Single(model => model.ID == booking.ClientID);
            booking.FullName = Client.FullName;
            booking.Contacts = Client.ContactPhone;


            var user = (CustomMemberShipUser)Membership.GetUser(true);
            if (user != null)
            {
                agentID = user.ID;
            }
            booking.Agent = agentID;
            booking.CreatedAt = DateTime.Now;
            BookingRooms booking1 = new BookingRooms() { Room = booking.Room,  };
            Actor Agent = entities.Actor.Single(model => model.ID == booking.Agent);
            Actor ClientInBook = entities.Actor.Single(moel => moel.ID == booking.ClientID);
            Booking book = new Booking()
            {
                Agent = booking.Agent,
                Client = booking.ClientID,
                CreatedAt = booking.CreatedAt,
                Arrival = booking.Arrival,
                Departure = booking.Departure,
                BookingRooms = new List<BookingRooms>() { booking1 },
                Actor = Agent,
                Actor1 = ClientInBook,
            };

            Receipt check = new Receipt()
            {
                Agent = booking.Agent,
                Client = booking.ClientID,
                Bill = booking.Bill,
            };
            book.Receipt = check.ID;
            entities.Booking.Add(book);
            
            entities.Receipt.Add(check);
            entities.SaveChanges();
            return Json(new { message = "Запись успешно добавлена", success = true}, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetBill(int roomNum, DateTime departure, DateTime arrival)
        {
            int days = Convert.ToInt32((departure - arrival).TotalDays);
            Room room = entities.Room.Single(model => model.RoomNum == roomNum);
            decimal cost = room.cost;
            decimal totalCost = cost * days;

            return Json(new { roomNum = roomNum, total = totalCost}, JsonRequestBehavior.AllowGet);
        }

        

        [HttpGet]
        public PartialViewResult GetBookingHistory()
        {

            List<BookingViewModel> listBookings = new List<BookingViewModel>();
            listBookings = (from elem in entities.Booking
                            join receipt in entities.Receipt on elem.Receipt equals receipt.ID
                            select new BookingViewModel()
                            {
                                FullName = elem.Actor1.FullName,
                                Contacts = elem.Actor1.ContactPhone,
                                Room = elem.BookingRooms.FirstOrDefault().Room,
                                Arrival = elem.Arrival,
                                Departure = elem.Departure,
                                Bill = receipt.Bill,
                                Days = System.Data.Entity.DbFunctions.DiffDays(elem.Arrival, elem.Departure).Value
                            }
                                ).ToList();
            return PartialView("_BookingPartialView", listBookings);
        }

        //[HttpGet] // получаем свободные комнаты, за исключением грязных
        public ActionResult MinusRoomList(DateTime departure, DateTime arrival)
        {
            BookingViewModel booking = new BookingViewModel();
            var union = (from el in entities.Room
                         where el.dirty == false
                         select el.RoomNum);
            var history = (from b in entities.Booking
                           where
                           b.Departure >= arrival || b.Arrival >= departure
                           select b.BookingRooms.FirstOrDefault().Room);
            var except = union.Except(history);
            var totalRooms = (from e in except
                              select new SelectListItem() { Text = e.ToString(), Value = e.ToString() });
            booking.ListRooms = totalRooms.ToList();
            return Json(totalRooms.Select(s=> new { 
                Text = s.Text,
                Value = s.Value,
            }).ToList(), JsonRequestBehavior.AllowGet);
        }

        //[HttpGet] // получаем свободные комнаты
        public ActionResult PlusRoomList(DateTime departure, DateTime arrival)
        {
            BookingViewModel booking = new BookingViewModel();
            var union = (from el in entities.Room
                         select el.RoomNum);
            var history = (from b in entities.Booking
                           where
                           //System.Data.Entity.DbFunctions.DiffDays(el.Arrival, arrival) >= 0
                           //||
                           //System.Data.Entity.DbFunctions.DiffDays( departure, el.Departure) >= 0
                           b.Departure >= arrival || b.Arrival >= departure
                           select b.BookingRooms.FirstOrDefault().Room);
            var except = union.Except(history);
            var totalRooms = (from e in except
                              select new SelectListItem() { Text = e.ToString(), Value = e.ToString() });
            booking.ListRooms = totalRooms.ToList();
            return Json(totalRooms.Select(s => new {
                Text = s.Text,
                Value = s.Value,
            }).ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}