using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorldOfDiscs.Models;
using PagedList;
using PagedList.Mvc;
using log4net;

namespace WorldOfDiscs.Controllers
{
    public class HomeController : Controller
    {
        //cấu hình ghi log 
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(HomeController));

        // GET: Home
        WorldOfDiscsEntities db = new WorldOfDiscsEntities();
        public ActionResult Index()
        {
            #region Ghi log
            log.Debug("Debug message");
            log.Warn("Warn message");
            log.Error("Error message");
            log.Fatal("Fatal message");
            #endregion
            return View(db.Discs.Where(n=>n.IsNew==1).ToList());
        }

        public ActionResult SupportOnline()
        {
            return View();
        }

        public ActionResult Forum(int? page)
        {
            if(Session["LogedUserID"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            //Số bài viết trên trang
            int pageSize = 10;
            //Số trang
            int pageNumber = (page ?? 1);

            List<Forum_Post> lstFP = db.Forum_Post.OrderByDescending(n=>n.Id).ToList();
            return View(lstFP.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Feedback()
        {
            if (Session["LogedUserID"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            int Id_User = int.Parse(Session["LogedUserID"].ToString());
            List<Feedback> lstFeedback = db.Feedbacks.Where(n => n.Id_User == Id_User).OrderByDescending(n => n.Id).ToList();
            return View(lstFeedback);
        }
    }
}