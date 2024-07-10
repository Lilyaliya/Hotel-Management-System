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
    public class ChangeController : Controller
    {
        hosteldat2Entities entities;

        public ChangeController()
        {
            entities = new hosteldat2Entities();
        }

        // GET: Change
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult RegisterChange()
        {
            var Change = new Change();
            var user = (CustomMemberShipUser)Membership.GetUser(true);
            var result = entities.Actor.Single(model => model.ID == user.ID);
            var ifExist = (from elem in entities.Change
                           
                           where System.Data.Entity.DbFunctions.DiffDays(elem.ChangeDate, DateTime.Now) < 1 
                           select elem).ToList();
            if (ifExist.Count == 0)
            {
                
                Change.Actor.Add(result);
                Change.ChangeDate = DateTime.Now;
                Change.Actors = 1;
                entities.Change.Add(Change);
            }
            else // иначе обновляется текущая смена
            {
                if (entities.Change.SingleOrDefault(model => System.Data.Entity.DbFunctions.DiffDays(model.ChangeDate, DateTime.Now) < 1) == null)
                {
                    Change.ChangeDate = DateTime.Now;
                }
                else
                {
                    Change = entities.Change.SingleOrDefault(model => System.Data.Entity.DbFunctions.DiffDays(model.ChangeDate, DateTime.Now) < 1);
                }
                var hasActor = Change.Actor.FirstOrDefault(model => model.ID == user.ID);
                if (hasActor == null)
                {
                    Change.Actor.Add(result);
                    Change.Actors += 1;
                    
                }
            }
            entities.SaveChanges();
            return Json(data: new { message = "Вы успешно зарегистрировались в смене", success = true},JsonRequestBehavior.AllowGet);
        }
    }
}