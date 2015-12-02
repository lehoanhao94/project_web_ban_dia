using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorldOfDiscs.Models;

namespace WorldOfDiscs.Controllers
{
    public class FeedbackController : Controller
    {
        //cấu hình ghi log 
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(FeedbackController));

        WorldOfDiscsEntities db = new WorldOfDiscsEntities();

        public ActionResult SendFeedback(FormCollection f)
        {
            try
            {
                Feedback fb = new Feedback();
                fb.Id_User = int.Parse(Session["LogedUserID"].ToString());
                fb.Date = DateTime.Now;
                fb.Content = f.Get("txtContent").ToString();
                db.Feedbacks.Add(fb);
                db.SaveChanges();
                return RedirectToAction("Feedback", "Home");
            }
            catch(Exception ex)
            {
                #region Ghi log
                log.Error(ex);
                #endregion
                return null;
            }
        }

        public ViewResult DetailFeed(int Id_Feed)
        {
            Feedback fb = db.Feedbacks.Single(n => n.Id == Id_Feed);
            ReplyFeedback repfb = db.ReplyFeedbacks.SingleOrDefault(n => n.Id_Feedback == Id_Feed);
            ViewBag.Feedback = fb;
            if(repfb == null)
            {
                ViewBag.ReplyFeedback = null;
            }
            else
            {
                ViewBag.ReplyFeedback = repfb;
            }
            return View();
        }
    }
}