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
    public class AdminController : Controller
    {
        //cấu hình ghi log 
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(AdminController));

        WorldOfDiscsEntities db = new WorldOfDiscsEntities();
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["LogedUserID"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string Email, string Password)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var v = db.Users.Where(a => a.Email.Equals(Email) && a.Password.Equals(Password) && a.isAdmin == 1).FirstOrDefault();
                    if (v != null)
                    {
                        Session["LogedUserID"] = v.Id.ToString();
                        Session["LogedUserName"] = v.FullName.ToString();
                        Session["Admin"] = v;
                        return RedirectToAction("Index", "Admin");
                    }
                }
                ViewBag.ErrorLogin = "Error: Incorrect email or password.";
                return View();
            }
            catch(Exception ex)
            {
                #region Ghi log
                log.Error(ex);
                #endregion
                return null;
            }
            
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Admin");
        }

        public ActionResult Profile()
        {
            if (Session["LogedUserID"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            int Id_User = int.Parse((Session["LogedUserID"]).ToString());
            User user = db.Users.Single(n => n.Id == Id_User);
            return View(user);
        }

        public ActionResult ManagingUserAccounts(int? page)
        {
            if (Session["LogedUserID"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            List<User> lstuser = db.Users.OrderByDescending(n=>n.Id).ToList();
            //Số đĩa trên trang
            int pageSize = 10;
            //Số trang
            int pageNumber = (page ?? 1);
            return View(lstuser.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ManagingDiscCategories(int? page)
        {
            if (Session["LogedUserID"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            List<Category> lstcategory = db.Categories.ToList();
            //Số đĩa trên trang
            int pageSize = 10;
            //Số trang
            int pageNumber = (page ?? 1);
            return View(lstcategory.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ManagingDiscDetails(int? page)
        {
            if (Session["LogedUserID"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            List<Disc> lstdisc = db.Discs.ToList();
            //Số đĩa trên trang
            int pageSize = 10;
            //Số trang
            int pageNumber = (page ?? 1);
            return View(lstdisc.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ManagingFeedback()
        {
            if (Session["LogedUserID"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            List<Feedback> lstfeedback = db.Feedbacks.OrderByDescending(n=>n.Id).ToList();
            List<ReplyFeedback> lstrefeed = db.ReplyFeedbacks.ToList();
            ViewData["lstrefeed"] = lstrefeed;
            return View(lstfeedback);
        }

        public ActionResult ManagingOrder(int? page)
        {
            if (Session["LogedUserID"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }

            //Số đĩa trên trang
            int pageSize = 10;
            //Số trang
            int pageNumber = (page ?? 1);
            List<Order> lstorder= db.Orders.OrderBy(n=>n.Status).ToList();
            List<Detail_Order> lstdeorder = db.Detail_Order.ToList();
            List<Direct_Payment_Method> lstdpm = db.Direct_Payment_Method.ToList();
            ViewData["lstdeorder"] = lstdeorder;
            ViewData["lstdpm"] = lstdpm;
            return View(lstorder.ToPagedList(pageNumber,pageSize));
        }

        public ActionResult MonthlyStatistics()
        {
            if (Session["LogedUserID"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }

        public ActionResult QuarterlyStatistics()
        {
            if (Session["LogedUserID"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }

        public ActionResult YearlyStatistics()
        {
            if (Session["LogedUserID"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }
        

        
    }
}