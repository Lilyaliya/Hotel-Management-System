using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Hotel.Models;
using Hotel.Authentication;
using System.Web.Security;

namespace Hotel.Controllers
{
    public class ActorController : Controller
    {

        hosteldat2Entities entities;
        public ActorController()
        {
            entities = new hosteldat2Entities();
        }
        // GET: Actor
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult GetAllEmployees()
        {
            IEnumerable<Hotel.ViewModel.EmployeesView> listOfEmployees = (from emps in entities.Actor
                                                                          where (emps.Role == "AGENT" || emps.Role == "CLEANER") && (emps.VerifiedBy == null || emps.VerifiedBy == 0)
                                                                       select new ViewModel.EmployeesView
                                                                       {
                                                                           ID = emps.ID,
                                                                           FullName = emps.FullName,
                                                                           Email = emps.Email,
                                                                           Role = emps.Role,
                                                                           ContactPhone = emps.ContactPhone,
                                                                           Passport = emps.Passport,
                                                                       }).ToList();
            return PartialView("_EmployeesPartial", listOfEmployees);
        }

        public PartialViewResult GetChange()
        {
            IEnumerable<Hotel.Models.Change> listOfChange = (from el in entities.Change
                                                                          
                                                                          select el
                                                                          );
            return PartialView("_ChangeListPartial", listOfChange);
        }

        [HttpGet]
        public JsonResult DeclineActor(int id)
        {
            Actor result = entities.Actor.Single(model => model.ID == id);
            entities.Actor.Remove(result);
            entities.SaveChanges();
            return Json(new { message = "Данные о заявке были удалены!", success = true }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult VerifyActor(int id) /*System.Security.Principal.IPrincipal identity*/
        {
            var user = (CustomMemberShipUser)Membership.GetUser(true);
            var result = entities.Actor.Single(model => model.ID == id);
            var idi = user.ID;
            string successMessage = "Сотрудник " + result.FullName +" принят в нашу команду!";
            if (user != null)
            {
                result.VerifiedBy = idi;
                entities.SaveChanges();
            }
            else
                successMessage = "Упс! Что-то пошло не так с вашим запросом...";
            return Json(data: new { message = successMessage, success = true }, JsonRequestBehavior.AllowGet);

        }

    }
}