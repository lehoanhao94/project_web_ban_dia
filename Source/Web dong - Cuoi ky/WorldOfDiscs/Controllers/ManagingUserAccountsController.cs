using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorldOfDiscs.Models;
using PagedList;
using PagedList.Mvc;

namespace WorldOfDiscs.Controllers
{
    public class ManagingUserAccountsController : Controller
    {
        WorldOfDiscsEntities db = new WorldOfDiscsEntities();
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(User user)
        {
            User us = db.Users.SingleOrDefault(n => n.Email == user.Email);
            if (us == null)
            {
                db.Users.Add(user);
                db.SaveChanges();
                user = null;
                ViewBag.Success = "Account successfully created !";
                ViewBag.Error = "";
                return View();
            }
            else
            {
                ViewBag.Error = "The Email address used to register already exists !";
                ViewBag.Success = "";
                return View();
            }
        }

        public ActionResult Delete(int Id_User)
        {
            try
            {
                User us = db.Users.SingleOrDefault(n => n.Id == Id_User);
                if (us == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                db.Users.Remove(us);
                db.SaveChanges();
                Session["Error"] = null;
                Session["Success"] = "The user account was deleted successfully !";
                return RedirectToAction("ManagingUserAccounts", "Admin");
            }
            catch(Exception)
            {
                Session["Error"] = "Unable to delete the user account !";
                Session["Success"] = null;
                return RedirectToAction("ManagingUserAccounts", "Admin");
            }
            
        }

        public ActionResult Update(int Id_User)
        {
            if (Session["LogedUserID"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            User user = db.Users.Single(n => n.Id == Id_User);
            return View(user);
        }

        [HttpPost]
        public ActionResult Update(User user, FormCollection f)
        {
            try
            {
                int Id_User = db.Users.Single(n => n.Email.Equals(user.Email)).Id;
                User us = db.Users.Find(Id_User);
                us.FullName = user.FullName;
                us.Email = user.Email;
                us.Address = user.Address;
                us.Mobile = user.Mobile;
                us.Birthday = user.Birthday;
                us.Avatar = user.Avatar;
                us.Password = user.Password;
                if (f.GetValue("isAdmin") != null)
                {
                    us.isAdmin = 1;
                }
                db.SaveChanges();
                Session["Success"] = "The user account was updated successfully !";
                Session["Error"] = null;
                return RedirectToAction("ManagingUserAccounts", "Admin");
            }
            catch(Exception)
            {
                Session["Error"] = "Unable to delete the user account !";
                Session["Success"] = null;
                return RedirectToAction("ManagingUserAccounts", "Admin");
            }
            
        }
    }
}