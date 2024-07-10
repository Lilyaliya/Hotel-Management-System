using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Hotel.Models;
using System.IO;

namespace Hotel.Controllers
{
    public class RoomController : Controller
    {
        hosteldat2Entities entities;
        public RoomController()
        {
            entities = new hosteldat2Entities();
        }
        // GET: Room
        public ActionResult Index()
        {
            Hotel.ViewModel.RoomViewModel rooms = new Hotel.ViewModel.RoomViewModel();
            rooms.ListCategoryClass = (from elem in entities.RoomClass
                                   select new SelectListItem() { Text = elem.Class, Value = elem.Class.ToString() }).ToList();
            rooms.ListCategoryWindow = (from elem in entities.RoomWindow
                                    select new SelectListItem() { Text = elem.Window, Value = elem.Window.ToString() }).ToList();
            rooms.ListCategoryEating = (from elem in entities.RoomEating
                                    select new SelectListItem() { Text = elem.Eating, Value = elem.Eating.ToString() }).ToList();
            
            IEnumerable<SelectListItem> list = new List<SelectListItem> { new SelectListItem() { Text = "Нет", Value = "Нет" }, new SelectListItem() { Text = "Да", Value = "Да" } };
            rooms.ListDirty = list.ToList();
            return View(rooms);
        }
        [HttpPost]
        public ActionResult Index(Hotel.ViewModel.RoomViewModel roomModel)
        {

            string successMessage;
            string ImageUniqueName = String.Empty;
            string ActualFileName = String.Empty;
            var el = entities.Room.FirstOrDefault(x => x.RoomNum == roomModel.RoomNum);

            if (el == null)
            {
                ImageUniqueName = Guid.NewGuid().ToString();
                ActualFileName = ImageUniqueName + Path.GetExtension(roomModel.Image.FileName);
                roomModel.Image.SaveAs(Server.MapPath("~/RoomImages/" + ActualFileName));
                //entities
                Room room = new Room()
                {
                    RoomNum = roomModel.RoomNum,
                    cost = roomModel.cost,
                    Places = roomModel.Places,
                    dirty = roomModel.dirty == "Да" ? true : false,
                    C_Floor = roomModel.C_Floor,
                    CategoryClass = roomModel.CategoryClass,
                    CategoryEating = roomModel.CategoryEating,
                    CategoryWindow = roomModel.CategoryWindow,
                    Image = ActualFileName,
                };
                successMessage = "Комната зарегистрирована!";
                entities.Room.Add(room);
            }
            else
            {
                Room updRoom = entities.Room.Single(model => model.RoomNum == roomModel.RoomNum);
                if (roomModel.Image != null)
                {
                    ImageUniqueName = Guid.NewGuid().ToString();
                    ActualFileName = ImageUniqueName + Path.GetExtension(roomModel.Image.FileName);
                    roomModel.Image.SaveAs(Server.MapPath("~/RoomImages/" + ActualFileName));
                    updRoom.Image = ActualFileName;
                }
                updRoom.RoomNum = roomModel.RoomNum;
                updRoom.cost = roomModel.cost;
                updRoom.Places = roomModel.Places;
                updRoom.dirty = roomModel.dirty == "Да" ? true : false;
                updRoom.C_Floor = roomModel.C_Floor;
                updRoom.CategoryClass = roomModel.CategoryClass;
                updRoom.CategoryEating = roomModel.CategoryEating;
                updRoom.CategoryWindow = roomModel.CategoryWindow;
                successMessage = "Изменения были внесены успешно!";
            }
            entities.SaveChanges();
            return Json(data: new { message=successMessage, success = true}, JsonRequestBehavior.AllowGet);
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
            return PartialView("_RoomDetailsPartial", listOfRoom);
        }
        [HttpGet]
        public JsonResult EditRoomdetails(int roomNum)
        {
            var result = entities.Room.Single(model => model.RoomNum == roomNum);
            ViewModel.RoomViewModel element = new ViewModel.RoomViewModel()
            {
                RoomNum = result.RoomNum,
                C_Floor = result.C_Floor,
                Places = result.Places,
                RoomImage = result.Image,
                cost = result.cost,
                dirty = result.dirty ? "Да": "Нет",
                CategoryClass = result.CategoryClass,
                CategoryEating = result.CategoryEating,
                CategoryWindow = result.CategoryWindow,
            };
            return Json(element, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult DeleteRoomdetails(int roomNum)
        {
            Room result = entities.Room.Single(model => model.RoomNum == roomNum);
            entities.Room.Remove(result);
            entities.SaveChanges();
            return Json(new { message = "Данные о комнате были удалены!", success = true}, JsonRequestBehavior.AllowGet);

        }
    }
}