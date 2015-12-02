using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorldOfDiscs.Models;


namespace WorldOfDiscs.Controllers
{
    public class ManagingFeedbackController : Controller
    {
        WorldOfDiscsEntities db = new WorldOfDiscsEntities();

        public ActionResult ReplyFeedback(int Id_Feed, FormCollection f)
        {
            ReplyFeedback rf = new ReplyFeedback();
            rf.Id_Feedback = Id_Feed;
            rf.Content = f.Get("txtRepFeed").ToString();
            rf.Date = DateTime.Now;
            db.ReplyFeedbacks.Add(rf);
            db.SaveChanges();
            return RedirectToAction("ManagingFeedback", "Admin");
        }
    }
}