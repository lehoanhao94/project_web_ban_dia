using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorldOfDiscs.Models;

namespace WorldOfDiscs.Controllers
{
    public class ManagingOrderController : Controller
    {
        WorldOfDiscsEntities db = new WorldOfDiscsEntities();
        public ActionResult Done(int Id_Order)
        {
            Order order = db.Orders.Find(Id_Order);
            order.Status = 1;
            order.IsPaid = 1;
            db.SaveChanges();
            return RedirectToAction("ManagingOrder", "Admin");
        }

        public ActionResult Cancel(int Id_Order)
        {
            Order order = db.Orders.Find(Id_Order);
            order.Status = 1;
            order.IsPaid = 0;
            db.SaveChanges();
            return RedirectToAction("ManagingOrder", "Admin");
        }
    }
}